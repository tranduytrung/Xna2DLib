using System;
using System.ComponentModel;
using System.Linq;
using Dovahkiin.Control;
using Dovahkiin.Maps;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;
using Dovahkiin.Repository;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace Dovahkiin.Screen
{
    public class GamePlayScreen : ComponentBase
    {
        private ScrollableView _mapView;

        public HybridMap MapControl { get; private set; }
        public CanvasObjectControl ControllingObject { get; private set; }

        public GamePlayScreen(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
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

            DataContext.LoadDefault();
            DataContext.Current.MapChanged += OnMapChanged;
            DataContext.Current.ControllingObjectChanged += OnControllingObjectChanged;
            DataContext.Current.Initialize();
            base.LoadContent();
        }

        private void OnControllingObjectChanged(object sender, PropertyChangeEventArgs e)
        {
            var newModel = e.NewValue as ICanvasObject;
            ControllingObject = GetCanvasObjectControl(newModel);
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
                MapControl.LeftMouseButtonDown -= OnMoveAction;
            }

            var newMap = e.NewValue as Map;
            if (newMap != null)
            {
                MapControl = Repository.Maps.GetControl(newMap);
                MapControl.LeftMouseButtonDown += OnMoveAction;
                _mapView.PresentableContent = MapControl;

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
                    var control = new CanvasObjectControl
                    {
                        PresentableContent = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(model.ResouceId))),
                        Model = model
                    };
                    control.SetValue(HybridMap.XProperty, model.X);
                    control.SetValue(HybridMap.YProperty, model.Y);
                    MapControl.CanvasObjectCollection.Add(control);
                    break;
                case CollectionChangeAction.Remove:
                    MapControl.CanvasObjectCollection.Remove(GetCanvasObjectControl(model));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
    }
}
