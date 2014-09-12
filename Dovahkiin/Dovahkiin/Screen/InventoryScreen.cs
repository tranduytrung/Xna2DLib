using Dovahkiin.Constant;
using Dovahkiin.Model.Core;
using Dovahkiin.Model.Creatures;
using Dovahkiin.Model.Item;
using Dovahkiin.Model.Party;
using Dovahkiin.Repository;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace Dovahkiin.Screen
{
    public class InventoryScreen : ComponentBase
    {
        private Canvas _mainCanvas;
        private DockPanel _dockPanel;
        private Sprite _background;

        private StackPanel _memberPanel;
        private StackPanel _itemPanel;
        private ManualParty _model;
        private int _maxRowLength;
        public InventoryScreen(Game game)
            : base(game)
        {
            _memberPanel = new StackPanel();
            _itemPanel = new StackPanel();
            _maxRowLength = 4;
        }

        protected override void LoadContent()
        {
            #region Canvas
            _mainCanvas = new Canvas();
            PresentableContent = _mainCanvas;

            _background = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.MainMenuBackground)));
            _background.SpriteMode = SpriteMode.Fit;
            _mainCanvas.Children.Add(_background);
            #endregion

            _dockPanel = new DockPanel();
            _dockPanel.AutoFillLastChild = true;
            _mainCanvas.Children.Add(_dockPanel);

            #region Member Stack
            _memberPanel.Width = 600;
            _memberPanel.Orientation = StackOrientation.Vertical;
            _memberPanel.SetValue(DockPanel.DockProperty, Dock.Left);
            _memberPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            _memberPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            _memberPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.001f);
            _memberPanel.SetValue(Panel.MarginProperty, new Margin(20, 30, 0, 50));
            _dockPanel.Children.Add(_memberPanel);

            
            #endregion
            _itemPanel.Width = 635;
            _itemPanel.Orientation = StackOrientation.Vertical;
            _itemPanel.SetValue(DockPanel.DockProperty, Dock.Right);
            _itemPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            _itemPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            _itemPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.001f);
            _itemPanel.SetValue(Panel.MarginProperty, new Margin(0, 30, 20, 50));
            _dockPanel.Children.Add(_itemPanel);
            #region Item Stack

            #endregion


        }

        public override void OnTransitFrom()
        {
            base.OnTransitFrom();

            _model = (ManualParty)Dovahkiin.GamePlayScreen.ControllingObject.Model;
            
            #region Member Panel
            AddMemberToPanel();
            #endregion
            
            //return;
            #region Item Panel
            AddItemToPanel();
            #endregion
        }

        private void AddItemToPanel()
        {
            ICollection<ICarriable> items = _model.CarryingItems;
            int size = items.ToArray().Length;

            StackPanel subStack = new StackPanel();
            subStack.Width = 200;
            subStack.Orientation = StackOrientation.Horizontal;
            subStack.SetValue(DockPanel.DockProperty, Dock.Right);
            subStack.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            _itemPanel.Children.Add(subStack);
            for (int i = 0; i < size; ++i)
            {
                Usable item = (Usable)items.ToArray()[i];
                int resourceId = 0;
                if (item is SmallBloodPotion)
                {
                    resourceId = Textures.SmallBloodPotion;
                }
                else if (item is BloodPotion)
                {
                    resourceId = Textures.BloodPotion;
                }

                if (subStack.Children.ToArray().Length >= _maxRowLength)
                {
                    subStack = new StackPanel();
                    subStack.Width = 200;
                    subStack.Orientation = StackOrientation.Horizontal;
                    subStack.SetValue(DockPanel.DockProperty, Dock.Right);
                    subStack.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    _memberPanel.Children.Add(subStack);
                }
                subStack.Children.Add(ControlFactory.CreateInventoryButton(resourceId));
            }
        }

        private void AddMemberToPanel()
        {
            IEnumerable<ICreature> members = _model.Members;
            int size = members.ToArray().Length;
            
            int clanType;
            switch (_model.Clan)
            {
                case ClanType.Human:
                    clanType = Textures.Knight;
                    break;
                default:
                    clanType = Textures.Knight;
                    break;
            }

            StackPanel subStack = new StackPanel();
            subStack.Width = 200;
            subStack.Orientation = StackOrientation.Horizontal;
            subStack.SetValue(DockPanel.DockProperty, Dock.Left);
            subStack.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            _memberPanel.Children.Add(subStack);
            for (int i = 0; i < size; ++i)
            {
                if (subStack.Children.ToArray().Length >= _maxRowLength)
                {
                    subStack = new StackPanel();
                    subStack.Width = 200;
                    subStack.Orientation = StackOrientation.Horizontal;
                    subStack.SetValue(DockPanel.DockProperty, Dock.Left);
                    subStack.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    _memberPanel.Children.Add(subStack);
                }
                subStack.Children.Add(ControlFactory.CreateInventoryButton(clanType));
            }
        }
    }
}
