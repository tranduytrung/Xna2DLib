using Dovahkiin.Constant;
using Dovahkiin.Control;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;
using Dovahkiin.Model.Party;
using Dovahkiin.Repository;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private Sprite _memberTitle;
        private Sprite _itemTitle;
        private Sprite _memberDescritionTitle;
        private Sprite _itemDescritionTitle;

        private StackPanel _leftPanel;
        private StackPanel _rightPanel;
        private StackPanel _memberPanel;
        private StackPanel _itemPanel;
        private StackPanel _descriptionPanel;
        //private StackPanel _itemDescriptionPanel;
        private ManualParty _model;

        private Button _backButton;
        private Button _useButton;
        private ToggleButton _selectedMember;
        private ToggleButton _selectedItem;

        private const int MAX_ROW_LENGHT = 8;
        private const int MAX_SUBSTACK_RIGHT_PANEL = 7;
        public InventoryScreen(Game game)
            : base(game)
        {
            _mainCanvas = new Canvas();
            _leftPanel = new StackPanel();
            _rightPanel = new StackPanel();
            _memberPanel = new StackPanel();
            _itemPanel = new StackPanel();
            _descriptionPanel = new StackPanel();
            //_itemDescriptionPanel = new StackPanel();

            #region Titles
            _memberTitle = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.TitleMember)));
            _itemTitle = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.TitleItem)));
            _memberDescritionTitle = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.TitleDescription)));
            _itemDescritionTitle = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.TitleDescription)));
            #endregion

            // Back button
            _backButton = ControlFactory.CreateButton("Back");
            _backButton.SetValue(Panel.MarginProperty, new Margin(50, 12));
            _backButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _backButton.Click += OnBackButtonClick;

            _useButton = ControlFactory.CreateButton("Use");
            _useButton.SetValue(Panel.MarginProperty, new Margin(50, 12));
            _useButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _useButton.Click += OnUseButtonClick;
        }

        protected override void LoadContent()
        {
            #region Canvas
            PresentableContent = _mainCanvas;
            #endregion

            _background = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.MainMenuBackground)));
            _background.SpriteMode = SpriteMode.Fit;
            _mainCanvas.Children.Add(_background);

            

            _dockPanel = new DockPanel();
            _dockPanel.AutoFillLastChild = true;
            _mainCanvas.Children.Add(_dockPanel);

            #region Left Panel
            
            _leftPanel.Orientation = StackOrientation.Vertical;
            _leftPanel.SetValue(DockPanel.DockProperty, Dock.Left);
            _leftPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            _leftPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _leftPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.001f);
            _leftPanel.SetValue(Panel.MarginProperty, new Margin(20, 10, 0, 30));
            _dockPanel.Children.Add(_leftPanel);

            #region Member Panel
            //_leftPanel.Children.Add(_memberIitle);

            _leftPanel.Children.Add(_memberPanel);
            #endregion

            #region Item Panel
            //_rightPanel.Children.Add(_itemTitle);

            _leftPanel.Children.Add(_itemPanel);

            #endregion
            #endregion


            #region Member Stack
            _memberPanel.Width = 600;
            _memberPanel.Orientation = StackOrientation.Vertical;
            _memberPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Top);
            //_memberPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            _memberPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);
            _memberPanel.SetValue(Panel.MarginProperty, new Margin(10, 10, 10, 10));
            #endregion

            #region bottom pannel

            _backButton.SetValue(DockPanel.DockProperty, Dock.Bottom);
            _dockPanel.Children.Add(_backButton);

            #endregion

            
            #region Right Panel
            _rightPanel.Orientation = StackOrientation.Vertical;
            _rightPanel.SetValue(DockPanel.DockProperty, Dock.Right);
            _rightPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            _rightPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _rightPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.001f);
            //_rightPanel.SetValue(Panel.MarginProperty, new Margin(0, 30, 20, 30));
            _dockPanel.Children.Add(_rightPanel);

            #endregion


            #region Item Stack
            _itemPanel.Width = 600;
            _itemPanel.Orientation = StackOrientation.Vertical;
            _itemPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            //_itemPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _itemPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);
            _itemPanel.SetValue(Panel.MarginProperty, new Margin(10, 10, 10, 10));
            #endregion

            #region Description Panel
            _descriptionPanel.Orientation = StackOrientation.Vertical;
            _descriptionPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Top);
            _descriptionPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _descriptionPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.7f);
            _descriptionPanel.SetValue(Panel.MarginProperty, new Margin(10, 10, 10, 10));

            //_itemDescriptionPanel.Orientation = StackOrientation.Vertical;
            //_itemDescriptionPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Top);
            //_itemDescriptionPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            //_itemDescriptionPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.7f);
            //_itemDescriptionPanel.SetValue(Panel.MarginProperty, new Margin(10, 10, 10, 10));
            #endregion

        }

        public override void OnTransitFrom()
        {
            base.OnTransitFrom();

            _model = (ManualParty)Dovahkiin.GamePlayScreen.ControllingObject.Model;

            //if (_leftPanel.Children.ToArray().Length > 0)
            //    _leftPanel.Children.Clear();
            //if (_rightPanel.Children.ToArray().Length > 0)
            //    _rightPanel.Children.Clear();

            //#region Member Panel
            //_leftPanel.Children.Add(_memberTitle);
            AddMemberToPanel();
            AddItemToPanel();

        }

        private void AddItemToPanel()
        {
            _itemPanel.Children.Clear();

            IEnumerable<ICarriable> items = _model.CarryingItems;
            int size = items.ToArray().Length;

            StackPanel subStack = NewSubStack();
            _itemPanel.Children.Add(subStack);
            for (int i = 0; i < MAX_ROW_LENGHT * MAX_SUBSTACK_RIGHT_PANEL; ++i)
            {
                if (subStack.Children.ToArray().Length >= MAX_ROW_LENGHT)
                {
                    subStack = NewSubStack();
                    _itemPanel.Children.Add(subStack);
                }

                ToggleButton button = null;
                if (i < size)
                {
                    button = ControlFactory.CreateInventoryButton(items.ToArray()[i].ResouceId);
                    button.Tag = items.ToArray()[i];
                }
                else
                {
                    button = ControlFactory.CreateInventoryButton(Textures.EmptyBox);
                    button.Tag = null;
                }
                
                button.Click += OnItemClick;
                subStack.Children.Add(button);
                
            }
        }

        private void AddMemberToPanel()
        {
            _memberPanel.Children.Clear();

            IEnumerable<ICreature> members = _model.Members;
            int size = members.ToArray().Length;

            StackPanel subStack = NewSubStack();
            _memberPanel.Children.Add(subStack);
            for (int i = 0; i < MAX_ROW_LENGHT; ++i)
            {
                ToggleButton button = null;
                if (i < size)
                {
                    button = ControlFactory.CreateInventoryButton(members.ToArray()[i].ResouceId);
                    subStack.Children.Add(button);
                    button.Tag = members.ToArray()[i];
                }
                else
                {
                    button = ControlFactory.CreateInventoryButton(Textures.EmptyBox);
                    subStack.Children.Add(button);
                    button.Tag = null;
                }

                button.Click += OnMemberClick;
            }
        }

        private StackPanel NewSubStack()
        {
            StackPanel subStack = new StackPanel();
            subStack.Orientation = StackOrientation.Horizontal;
            subStack.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            return subStack;
        }

        private void OnBackButtonClick(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(Dovahkiin.GamePlayScreen);
        }

        private void OnItemClick(object sender, MouseEventArgs e)
        {
            _descriptionPanel.Children.Clear();
            _rightPanel.Children.Remove(_descriptionPanel);

            ToggleButton button = (ToggleButton)sender;
            if (button.Tag != null)
            {
                if (_selectedItem != null)
                {
                    _selectedItem.IsToggled = false;
                }

                _selectedItem = button;
                _descriptionPanel.Children.Add(_itemDescritionTitle);
                ICarriable item = (ICarriable)button.Tag;

                var font = Resouces.GetFont(Fonts.DescriptionFont);

                SpriteText spriteText = new SpriteText(font) { Text = item.Description };
                _descriptionPanel.Children.Add(spriteText);

                if (item is Usable)
                {
                    StackPanel subStack = NewSubStack();
                    ToggleButton adjustButton = ControlFactory.CreateIncDecButton("+");
                    adjustButton.Click += OnIncreaseButtonClick;
                    subStack.Children.Add(adjustButton);
                    adjustButton = ControlFactory.CreateIncDecButton("-");
                    adjustButton.Click += OnDecreaseButtonClick;
                    subStack.Children.Add(adjustButton);

                    _descriptionPanel.Children.Add(subStack);
                    _descriptionPanel.Children.Add(_useButton);
                }

                _rightPanel.Children.Add(_descriptionPanel);
            }
        }

        private void OnMemberClick(object sender, MouseEventArgs e)
        {
            _descriptionPanel.Children.Clear();
            _rightPanel.Children.Remove(_descriptionPanel);

            ToggleButton button = (ToggleButton)sender;
            if (button.Tag != null)
            {
                if (_selectedMember != null)
                {
                    _selectedMember.IsToggled = false;
                }

                _selectedMember = button;
                _descriptionPanel.Children.Add(_memberDescritionTitle);
                ICreature member = (ICreature)button.Tag;

                var font = Resouces.GetFont(Fonts.DescriptionFont);
                string str = "HP: " + member.HitPoint;
                str += "\nMP: " + member.MagicPoint;
                SpriteText spriteText = new SpriteText(font) 
                {
                    Text = str
                };

                spriteText.Width = 600;
                _descriptionPanel.Children.Add(spriteText);

                _rightPanel.Children.Insert(0, _descriptionPanel);
            }
        }

        private void OnUseButtonClick(object sender, MouseEventArgs e)
        {
            if (_selectedItem == null || _selectedMember == null)
                return;

            var item = _selectedItem.Tag as Usable;
            if (item == null)
                return;

            var creature = (ICreature) _selectedMember.Tag;
            _model.DoAction(new UseItem() {Target = creature, UsableItem = item});
        }

        private void OnDecreaseButtonClick(object sender, MouseEventArgs e)
        {
            // TODO: Decrease use time of item
        }

        private void OnIncreaseButtonClick(object sender, MouseEventArgs e)
        {
            // TODO: Increase use time of item
        }
    }
}
