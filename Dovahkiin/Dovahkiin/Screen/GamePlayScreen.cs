﻿using System;
using System.ComponentModel;
using System.Linq;
using Dovahkiin.ActionHandler;
using Dovahkiin.Constant;
using Dovahkiin.Control;
using Dovahkiin.Extension;
using Dovahkiin.Maps;
using Dovahkiin.Model.Core;
using Dovahkiin.Repository;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;
using Dovahkiin.Broker;

namespace Dovahkiin.Screen
{
    public class GamePlayScreen : ComponentBase
    {
        private ScrollableView _mapView;
        private Cue _music;

        public HybridMap MapControl { get; private set; }
        public CanvasObjectControl ControllingObject { get; private set; }

        public BrokerClient BrokerClient { get; set; }

        public GamePlayScreen(Game game)
            : base(game)
        {
        }

        public override void OnTransitFrom()
        {
            if (!GlobalConfig.MusicEnabled)
                return;

            if (_music.IsPaused)
                _music.Resume();
            else
                _music.Play();
        }

        public override void OnTransitTo()
        {
            if (!GlobalConfig.MusicEnabled)
                return;

            _music.Pause();
        }

        protected override void LoadContent()
        {
            _music = Sounds.GetBackgroundMusic();

            #region Canvas

            var canvas = new Canvas();
            PresentableContent = canvas;

            #endregion

            #region Map view control

            _mapView = new ScrollableView();
            _mapView.Decelerator = 1000;
            canvas.Children.Add(_mapView);

            #endregion

            #region Dock Panel

            var panel = new DockPanel();
            canvas.Children.Add(panel);

            #endregion

            #region Game left Menu

            var leftPanelContainer = new ContentPresenter();
            leftPanelContainer.SetValue(DockPanel.DockProperty, Dock.Left);
            leftPanelContainer.BackgroundColor = ControlConfig.LeftPanelBackgroundColor;
            panel.Children.Add(leftPanelContainer);

            var leftPanel = new StackPanel();
            leftPanel.Orientation = StackOrientation.Vertical;
            leftPanelContainer.PresentableContent = leftPanel;

            var settingButton = ControlFactory.CreateLeftPanelButton(Textures.MenuBox);
            settingButton.Click += OnSettingButtonClick;
            leftPanel.Children.Add(settingButton);

            var inventoryButton = ControlFactory.CreateLeftPanelButton(Textures.InventoryBox);
            inventoryButton.Click += OnInventoryButtonClick;
            leftPanel.Children.Add(inventoryButton);

            #endregion

            #region Context menu

            #endregion


            #region Initialize data
            DataContext.LoadDefault();
            DataContext.Current.MapChanged += OnMapChanged;
            DataContext.Current.ControllingObjectChanged += OnControllingObjectChanged;
            DataContext.Current.Initialize();
            #endregion

            base.LoadContent();
        }

        #region Button Event Handler
        private void OnInventoryButtonClick(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(Dovahkiin.InventoryScreen);
        }

        private void OnSettingButtonClick(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(Dovahkiin.StartupMenuScreen);
        }
        #endregion

        #region Map event handler
        private void OnControllingObjectChanged(object sender, PropertyChangeEventArgs e)
        {
            var newModel = e.NewValue as ICanvasObject;
            ControllingObject = GetCanvasObjectControl(newModel);

            var actor = newModel as Actor;
            if (actor == null)
                return;

            var tradeHandler = (TradeRequestHandler)actor.GetActionHandler(typeof(TradeRequestHandler));
                
            if (tradeHandler == null)
                return;

            tradeHandler.TradeRequest += OnTradeRequest;
            tradeHandler.RequestReponse += OnTradeResponse;

            var attackHandler = (AttackHandler)actor.GetActionHandler(typeof(AttackHandler));
            attackHandler.Attacked += OnAttacked;
        }

        private void OnAttacked(object sender, AttackEventArgs e)
        {
            Dovahkiin.QuickBattleScreen.Attacker = e.Target;
            GameContext.GameInstance.ChangeScreen(Dovahkiin.QuickBattleScreen);
        }

        private void OnTradeResponse(object sender, TradeEventArgs e)
        {
            if (!e.Accepted)
                return;
            
            // TODO: open trade screen
            Dovahkiin.TradingScreen.BrokerClient = e.Client;
            Dovahkiin.TradingScreen.Target = e.Target;
            GameContext.GameInstance.ChangeScreen(Dovahkiin.TradingScreen);
        }

        private void OnTradeRequest(object sender, TradeEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMapChanged(object sender, PropertyChangeEventArgs e)
        {
            var oldMap = e.OldValue as Map;
            if (oldMap != null)
            {
                oldMap.CanvasObjectChanged -= OnCanvasObjectChanged;
                oldMap.MapObjectChanged -= OnMapObjectChanged;
            }

            if (MapControl != null)
            {
                MapControl.RightMouseClick -= OnMoveAction;
            }

            var newMap = e.NewValue as Map;
            if (newMap != null)
            {
                MapControl = Repository.Maps.GetControl(newMap);
                _mapView.PresentableContent = MapControl;

                MapControl.RightMouseClick += OnMoveAction;
                newMap.CanvasObjectChanged += OnCanvasObjectChanged;
                newMap.MapObjectChanged += OnMapObjectChanged;
            }
        }

        private void OnMoveAction(object sender, MouseEventArgs e)
        {
            ControllingObject.MoveTo(e.X, e.Y);
        }

        private void OnMapObjectChanged(object sender, CollectionChangeEventArgs e)
        {
            //TODO: map object changed 
        }

        private void OnCanvasObjectChanged(object sender, CollectionChangeEventArgs e)
        {
            var model = (ICanvasObject)e.Element;
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    var control = new CanvasObjectControl(model);
                    control.Click += OnCanvasObjectClick;
                    MapControl.CanvasObjectCollection.Add(control);
                    break;
                case CollectionChangeAction.Remove:
                    control = GetCanvasObjectControl(model);
                    control.Click -= OnCanvasObjectClick;
                    MapControl.CanvasObjectCollection.Remove(control);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnCanvasObjectClick(object sender, MouseEventArgs e)
        {
            var control = sender as CanvasObjectControl;
            if (control == null) throw new ArgumentNullException("sender", "this error should not be encountered.");

            var actor = control.Model as Actor;
            if (actor == null || control == ControllingObject)
                return;

            Dovahkiin.ActionSuggestionScreen.TargetActor = actor;
            GameContext.GameInstance.ChangeScreen(Dovahkiin.ActionSuggestionScreen);
        }

        private CanvasObjectControl GetCanvasObjectControl(ICanvasObject model)
        {
            return (CanvasObjectControl)MapControl.CanvasObjectCollection.First(item =>
            {
                var canvasObject = item as CanvasObjectControl;
                if (canvasObject == null) return false;
                return canvasObject.Model == model;
            });
        }

        #endregion

    }
}
