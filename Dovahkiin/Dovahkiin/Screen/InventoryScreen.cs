using Dovahkiin.Constant;
using Dovahkiin.Model.Core;
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
        //private Sprite _title;

        private StackPanel _memberPanel;
        private StackPanel _itemPanel;


        public InventoryScreen(Game game)
            : base(game)
        {
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
            _dockPanel.AutoFillLastChild = false;
            _mainCanvas.Children.Add(_dockPanel);

            #region Member Stack
            _memberPanel = new StackPanel();
            _memberPanel.Width = 635;
            _memberPanel.Orientation = StackOrientation.Vertical;
            _memberPanel.SetValue(DockPanel.DockProperty, Dock.Left);
            _memberPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            _memberPanel.BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.1f);
            _dockPanel.Children.Add(_memberPanel);

            AddMemberToPanel();
            for (int i = 0; i < 2; ++i)
            {
                var subContentStack = new StackPanel();
                subContentStack.Width = 640;
                subContentStack.Orientation = StackOrientation.Horizontal;
                subContentStack.SetValue(DockPanel.DockProperty, Dock.Left);
                subContentStack.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                for (int j = 0; j < 3; ++j)
                {
                    subContentStack.Children.Add(ControlFactory.CreateLeftPanelButton(Textures.MenuBox));
                }
                _memberPanel.Children.Add(subContentStack);
            }
            #endregion
            _itemPanel = new StackPanel();
            _itemPanel.Width = 635;
            _itemPanel.Orientation = StackOrientation.Vertical;
            _itemPanel.SetValue(DockPanel.DockProperty, Dock.Right);
            _itemPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            _itemPanel.BackgroundColor = new Color(255, 255, 255, 100);
            _dockPanel.Children.Add(_itemPanel);

            AddItemToPanel();
            for (int i = 0; i < 2; ++i)
            {
                var subContentStack = new StackPanel();
                subContentStack.Width = 640;
                subContentStack.Orientation = StackOrientation.Horizontal;
                subContentStack.SetValue(DockPanel.DockProperty, Dock.Left);
                subContentStack.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                for (int j = 0; j < 3; ++j)
                {
                    subContentStack.Children.Add(ControlFactory.CreateLeftPanelButton(Textures.MenuBox));
                }
                _itemPanel.Children.Add(subContentStack);
            }
            #region Item Stack

            #endregion


        }

        private void AddItemToPanel()
        {
            //ICollection<ICarriable> carryingItems = ((ManualParty)((GamePlayScreen)Dovahkiin.GamePlayScreen).ControllingObject).CarryingItems;
        }

        private void AddMemberToPanel()
        {
            //IEnumerable<ICreature> members = ((ManualParty)(((GamePlayScreen)Dovahkiin.GamePlayScreen).ControllingObject)).;
        }
    }
}
