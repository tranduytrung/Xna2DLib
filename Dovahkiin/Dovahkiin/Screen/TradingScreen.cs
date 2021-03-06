﻿using Dovahkiin.Broker;
using Dovahkiin.Constant;
using Dovahkiin.Control;
using Dovahkiin.Model.Core;
using Dovahkiin.Model.Party;
using Dovahkiin.Repository;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;
using tranduytrung.Xna.Helper;

namespace Dovahkiin.Screen
{
    public class TradingScreen : ComponentBase
    {
        private Canvas _mainCanvas;
        private DockPanel _dockPanel;
        private Sprite _background;
        private Sprite _mineTitle;
        private Sprite _theirsTitle;
        private Sprite _descriptionTitle;

        private StackPanel _leftPanel;
        private StackPanel _rightPanel;
        private StackPanel _minePanel;
        private StackPanel _theirsPanel;
        private StackPanel _descriptionPanel;
        private StackPanel _tradePanel;
        private StackPanel _acceptPanel;
        private ManualParty _model;
        private BrokerClient _trader;

        private Button _buyButton;
        private Button _sellButton;
        private Button _acceptButton;
        private Button _cancelButton;

        private ToggleButton _selectedItemButton;
        private ICarriable _selectedItem;
        private int _selectedItemTempUseTime;
        private int _amountChanged;

        private const int MAX_ROW_LENGHT = 9;
        private const int MAX_SUBSTACK_RIGHT_PANEL = 4;

        private Dictionary<ICarriable, int> _sellItems;
        private Dictionary<ICarriable, int> _buyItems;

        private List<ICarriable> _demandList;
        private List<ICarriable> _offerList;

        public BrokerClient BrokerClient { get; set; }
        public ICarrier Target { get; set; }

        public TradingScreen(Game game)
            : base(game)
        {
            _mainCanvas = new Canvas();
            _leftPanel = new StackPanel();
            _rightPanel = new StackPanel();
            _minePanel = new StackPanel();
            _theirsPanel = new StackPanel();
            _descriptionPanel = new StackPanel();
            _tradePanel = new StackPanel();
            _acceptPanel = new StackPanel();

            #region Titles
            _mineTitle = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.TitleMyItems)));
            _theirsTitle = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.TitleTheirItems)));
            _descriptionTitle = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.TitleDescription)));
            #endregion

            #region Buttons
            var buttonNormalTexture = Resouces.GetTexture(Textures.ButtonNormal);
            var buttonHoverTexture = Resouces.GetTexture(Textures.ButtonHover);
            var buttonPressedTexture = Resouces.GetTexture(Textures.ButtonPressed);
            var buttonFont = Resouces.GetFont(Fonts.ButtonFont);
            
            _buyButton = ControlFactory.CreateButton("Buy");
            _buyButton.SetValue(Panel.MarginProperty, new Margin(50, 5));
            _buyButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _buyButton.Click += OnBuyButtonClick;

            _sellButton = ControlFactory.CreateButton("Sell");
            _sellButton.SetValue(Panel.MarginProperty, new Margin(50, 5));
            _sellButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _sellButton.Click += OnSellButtonClick;

            _acceptButton = ControlFactory.CreateButton("Accept");
            _acceptButton.SetValue(Panel.MarginProperty, new Margin(5, 12));
            _acceptButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _acceptButton.Click += OnAcceptButtonClick;

            _cancelButton = ControlFactory.CreateButton("Cancel");
            _cancelButton.SetValue(Panel.MarginProperty, new Margin(5, 12));
            _cancelButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _cancelButton.Click += OnCancelButtonClick;
            #endregion
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            #region Canvas
            PresentableContent = _mainCanvas;
            #endregion

            _background = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.MainMenuBackground)));
            _background.SpriteMode = SpriteMode.Fit;
            _mainCanvas.Children.Add(_background);



            _dockPanel = new DockPanel();
            _dockPanel.AutoFillLastChild = false;
            _mainCanvas.Children.Add(_dockPanel);

            #region Left Panel
            _leftPanel.Width = 800;
            _leftPanel.Orientation = StackOrientation.Vertical;
            _leftPanel.SetValue(DockPanel.DockProperty, Dock.Left);
            _leftPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            _leftPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _leftPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.001f);
            _leftPanel.SetValue(Panel.MarginProperty, new Margin(20, 30, 0, 30));
            _dockPanel.Children.Add(_leftPanel);
            #endregion

            #region Mine
            _minePanel.Width = 600;
            _minePanel.Orientation = StackOrientation.Vertical;
            _minePanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Top);
            _minePanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);
            _minePanel.SetValue(Panel.MarginProperty, new Margin(10, 10, 10, 10));

            _leftPanel.Children.Add(_mineTitle);
            _leftPanel.Children.Add(_minePanel);
            #endregion

            #region Right Panel
            _rightPanel.Width = 600;
            _rightPanel.Orientation = StackOrientation.Vertical;
            _rightPanel.SetValue(DockPanel.DockProperty, Dock.Right);
            _rightPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            _rightPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _rightPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.001f);
            _rightPanel.SetValue(Panel.MarginProperty, new Margin(0, 30, 20, 30));
            _dockPanel.Children.Add(_rightPanel);
            #endregion

            #region Theirs
            _theirsPanel.Width = 600;
            _theirsPanel.Orientation = StackOrientation.Vertical;
            _theirsPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            _theirsPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);
            _theirsPanel.SetValue(Panel.MarginProperty, new Margin(10, 10, 10, 10));

            _rightPanel.Children.Add(_theirsTitle);
            _rightPanel.Children.Add(_theirsPanel);
            #endregion

            #region Description Panel
            _descriptionPanel.Width = 600;
            _descriptionPanel.Orientation = StackOrientation.Vertical;
            _descriptionPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Top);
            _descriptionPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _descriptionPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.7f);
            _descriptionPanel.SetValue(Panel.MarginProperty, new Margin(10, 10, 10, 10));
            #endregion

            #region Trading Panel
            _tradePanel.Width = 600;
            _tradePanel.Orientation = StackOrientation.Vertical;
            _tradePanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Top);
            _tradePanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _tradePanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.7f);
            _tradePanel.SetValue(Panel.MarginProperty, new Margin(10, 10, 10, 0));

            _rightPanel.Children.Add(_tradePanel);
            #endregion

            #region Accept Panel
            _acceptPanel.Width = 600;
            _acceptPanel.Orientation = StackOrientation.Vertical;
            _acceptPanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Top);
            _acceptPanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _acceptPanel.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.7f);
            _acceptPanel.SetValue(Panel.MarginProperty, new Margin(10, 0, 10, 10));

            StackPanel subStack = NewSubStack();
            subStack.Children.Add(_acceptButton);
            subStack.Children.Add(_cancelButton);
            _acceptPanel.Children.Add(subStack);
            
            _rightPanel.Children.Add(_acceptPanel);
            #endregion

            BrokerClient.DealAccepted += OnDealAccepted;
            BrokerClient.DealChanged += OnDealChanged;
        }

        public override void OnTransitFrom()
        {
            base.OnTransitFrom();

            _model = (ManualParty)Dovahkiin.GamePlayScreen.ControllingObject.Model;
            _trader = BrokerClient;

            _descriptionPanel.Children.Clear();
            _tradePanel.Children.Clear();

            _demandList = null;
            _offerList = null;

            _buyItems = new Dictionary<ICarriable, int>();
            _sellItems = new Dictionary<ICarriable, int>();

            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateBuySellLists();
            AddMyItems();

            AddTheirItems();
        }

        private void UpdateBuySellLists()
        {
            IEnumerable<ICarriable> items;
            // Update my items/sell list
            items = _model.CarryingItems;
            for (int i = 0; i < items.ToArray().Length; ++i)
            {
                if (ExistTypeInList(BrokerClient.DemandItems, items.ToArray()[i]) != null)
                {
                    var toSellItem = ExistTypeInList(BrokerClient.DemandItems, items.ToArray()[i]);
                    if (toSellItem is Usable)
                    {
                        int sellAmount = ((Usable)toSellItem).UsableTimes - ((Usable)items.ToArray()[i]).UsableTimes;
                        if (sellAmount >= 0)
                        {
                            ((Usable)items.ToArray()[i]).UsableTimes = 0;
                            ((Usable)toSellItem).UsableTimes = sellAmount;
                            _sellItems.Add(items.ToArray()[i], ((Usable)items.ToArray()[i]).UsableTimes);
                        }
                        else
                        {
                            ((Usable)items.ToArray()[i]).UsableTimes = -sellAmount;
                            ((Usable)toSellItem).UsableTimes = 0;
                            _sellItems.Add(items.ToArray()[i], -sellAmount);
                        }
                    }
                    else
                    {
                        _sellItems.Add(items.ToArray()[i], 0);
                    }
                }
            }

            // Update their items/buy list
            items = Target.CarryingItems;
            for (int i = 0; i < items.ToArray().Length; ++i)
            {
                if (ExistTypeInList(BrokerClient.OfferItems, items.ToArray()[i]) != null)
                {
                    var toBuyItem = ExistTypeInList(BrokerClient.OfferItems, items.ToArray()[i]);
                    if (toBuyItem is Usable)
                    {
                        int buyAmount = ((Usable)toBuyItem).UsableTimes - ((Usable)items.ToArray()[i]).UsableTimes;
                        if (buyAmount >= 0)
                        {
                            ((Usable)items.ToArray()[i]).UsableTimes = 0;
                            ((Usable)toBuyItem).UsableTimes = buyAmount;
                            _buyItems.Add(items.ToArray()[i], ((Usable)items.ToArray()[i]).UsableTimes);
                        }
                        else
                        {
                            ((Usable)items.ToArray()[i]).UsableTimes = -buyAmount;
                            ((Usable)toBuyItem).UsableTimes = 0;
                            _buyItems.Add(items.ToArray()[i], -buyAmount);
                        }
                    }
                    else
                    {
                        _buyItems.Add(items.ToArray()[i], 0);
                    }
                }
            }
        }

        private ICarriable ExistTypeInList(IEnumerable<ICarriable> list, ICarriable item)
        {
            for (int i = 0; i < list.ToArray().Length; ++i)
            {
                var entry = list.ToArray()[i];
                if (entry.GetType() == item.GetType())
                {
                    return entry;
                }
            }
            return null;
        }

        private void AddTheirItems()
        {
            _theirsPanel.Children.Clear();
            
            IEnumerable<ICarriable> items = Target.CarryingItems;
            int size = items.ToArray().Length;

            StackPanel subStack = NewSubStack();
            _theirsPanel.Children.Add(subStack);
            for (int i = 0; i < MAX_ROW_LENGHT * MAX_SUBSTACK_RIGHT_PANEL; ++i)
            {
                if (subStack.Children.ToArray().Length >= MAX_ROW_LENGHT)
                {
                    subStack = NewSubStack();
                    _theirsPanel.Children.Add(subStack);
                }

                ToggleButton button = null;
                if (i < size)
                {
                    button = ControlFactory.CreateInventoryButton(items.ToArray()[i].ResouceId);
                    if (_buyItems.ContainsKey(items.ToArray()[i]))
                    {
                        button.PresentableContent = ControlFactory.CreateTradingRepresentableContent(button.PresentableContent, true);
                    }
                    button = ControlFactory.CreateInventoryButton(items.ToArray()[i].ResouceId);
                    button.Tag = items.ToArray()[i];
                }
                else
                {
                    button = ControlFactory.CreateInventoryButton(Textures.EmptyBox);
                    button.Tag = null;
                }

                button.Click += OnTheirItemClick;
                subStack.Children.Add(button);
            }
        }

        private void AddMyItems()
        {
            _minePanel.Children.Clear();
            
            IEnumerable<ICarriable> items = _model.CarryingItems;
            int size = items.ToArray().Length;

            StackPanel subStack = NewSubStack();
            _minePanel.Children.Add(subStack);
            for (int i = 0; i < MAX_ROW_LENGHT * MAX_SUBSTACK_RIGHT_PANEL; ++i)
            {
                if (subStack.Children.ToArray().Length >= MAX_ROW_LENGHT)
                {
                    subStack = NewSubStack();
                    _minePanel.Children.Add(subStack);
                }

                ToggleButton button = null;
                if (i < size)
                {
                    button = ControlFactory.CreateInventoryButton(items.ToArray()[i].ResouceId);
                    if (_sellItems.ContainsKey(items.ToArray()[i]))
                    {
                        button.PresentableContent = ControlFactory.CreateTradingRepresentableContent(button.PresentableContent, false);
                    }
                    button.Tag = items.ToArray()[i];
                }
                else
                {
                    button = ControlFactory.CreateInventoryButton(Textures.EmptyBox);
                    button.Tag = null;
                }

                button.Click += OnMyItemClick;
                subStack.Children.Add(button);
            }
        }

        private StackPanel NewSubStack()
        {
            StackPanel subStack = new StackPanel();
            subStack.Orientation = StackOrientation.Horizontal;
            subStack.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            subStack.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            return subStack;
        }
        
        private void OnMyItemClick(object sender, MouseEventArgs e)
        {
            _leftPanel.Children.Remove(_descriptionPanel);
            //_rightPanel.Children.Remove(_tradePanel);
            _tradePanel.Children.Clear();

            ToggleButton button = (ToggleButton)sender;
            if (button.Tag != null)
            {
                if (_selectedItemButton != null)
                {
                    _selectedItemButton.IsToggled = false;
                }

                _selectedItemButton = button;
                ICarriable item = (ICarriable)button.Tag;
                _selectedItem = item;

                if (_selectedItem is Usable)
                {
                    if (_sellItems.ContainsKey(item))
                    {
                        _selectedItemTempUseTime = ((Usable)_selectedItem).UsableTimes + _sellItems[item];
                        _amountChanged = _sellItems[item];
                    }
                    else
                    {
                        _selectedItemTempUseTime = ((Usable)_selectedItem).UsableTimes;
                        _amountChanged = 0;
                    }
                }

                UpdateDescriptionPanel();
                _leftPanel.Children.Add(_descriptionPanel);

                InitializeTradePanel(false, item);
            }
        }

        private void OnTheirItemClick(object sender, MouseEventArgs e)
        {
            _leftPanel.Children.Remove(_descriptionPanel);
            //_rightPanel.Children.Remove(_tradePanel);
            _tradePanel.Children.Clear();

            ToggleButton button = (ToggleButton)sender;
            if (button.Tag != null)
            {
                if (_selectedItemButton != null)
                {
                    _selectedItemButton.IsToggled = false;
                }

                _selectedItemButton = button;
                ICarriable item = (ICarriable)button.Tag;
                _selectedItem = item;

                if (_selectedItem is Usable)
                {
                    if (_sellItems.ContainsKey(item))
                    {
                        _selectedItemTempUseTime = ((Usable)_selectedItem).UsableTimes + _buyItems[item];
                        _amountChanged = _buyItems[item];
                    }
                    else
                    {
                        _selectedItemTempUseTime = ((Usable)_selectedItem).UsableTimes;
                        _amountChanged = 0;
                    }
                }

                UpdateDescriptionPanel();
                _leftPanel.Children.Add(_descriptionPanel);

                InitializeTradePanel(true, item);
            }
        }

        private void InitializeTradePanel(bool toBuy, ICarriable item)
        {
            //_rightPanel.Children.Remove(_tradePanel);
            _tradePanel.Children.Clear();
            StackPanel subStack = NewSubStack();
            if (toBuy)
                subStack.Children.Add(_buyButton);
            else
                subStack.Children.Add(_sellButton);

            if (item is Usable)
            {
                StackPanel adjustPanel = CreateAdjustPanel();
                subStack.Children.Add(adjustPanel);

                var font = Resouces.GetFont(Fonts.DescriptionFont);
                string str = null;
                if (toBuy)
                {
                    str = "Buy: ";
                    str+=Math.Abs(_amountChanged);
                }
                else
                {
                    str = "Sell: ";
                    str+=Math.Abs(_amountChanged);
                }

                str += " unit";
                SpriteText spriteText = new SpriteText(font) { Text = str };
                _tradePanel.Children.Add(spriteText);
            }
            _tradePanel.Children.Add(subStack);
            //_rightPanel.Children.Add(_tradePanel);
        }

        private StackPanel CreateAdjustPanel()
        {
            StackPanel subStack = NewSubStack();
            ToggleButton adjustButton = ControlFactory.CreateIncDecButton("+");
            adjustButton.Click += OnIncreaseButtonClick;
            subStack.Children.Add(adjustButton);
            adjustButton = ControlFactory.CreateIncDecButton("-");
            adjustButton.Click += OnDecreaseButtonClick;
            subStack.Children.Add(adjustButton);

            return subStack;
        }
        
        private void OnDecreaseButtonClick(object sender, MouseEventArgs e)
        {
            if (_selectedItemTempUseTime != 0)
            {
                --_amountChanged;
                --_selectedItemTempUseTime;
                UpdateDescriptionPanel();
            }
        }

        private void OnIncreaseButtonClick(object sender, MouseEventArgs e)
        {
            if (_selectedItemTempUseTime != ((Usable)_selectedItem).UsableTimes)
            {
                ++_amountChanged;
                ++_selectedItemTempUseTime;
                UpdateDescriptionPanel();
            }
        }

        private void UpdateDescriptionPanel()
        {
            _descriptionPanel.Children.Clear();
            _descriptionPanel.Children.Add(_descriptionTitle);

            string useTime = "\nUse times: ";
            if (_selectedItem is Usable)
            {
                Usable usableItem = (Usable)_selectedItem;
                useTime += _selectedItemTempUseTime;
            }
            var font = Resouces.GetFont(Fonts.DescriptionFont);
            SpriteText spriteText = new SpriteText(font) { Text = _selectedItem.Description + useTime };
            _descriptionPanel.Children.Add(spriteText);
        }

        private void OnBuyButtonClick(object sender, MouseEventArgs e)
        {
            if (_amountChanged != 0)
            {
                _selectedItemButton.PresentableContent = ControlFactory.CreateTradingRepresentableContent(_selectedItemButton.PresentableContent, true);

                if (!_buyItems.ContainsKey(_selectedItem))
                {
                    _buyItems.Add(_selectedItem, _amountChanged);
                }
                else
                {
                    _buyItems[_selectedItem] = _amountChanged;
                }

                InitializeTradePanel(true, _selectedItem);
            }
        }

        private void OnSellButtonClick(object sender, MouseEventArgs e)
        {
            if (_amountChanged != 0)
            {
                _selectedItemButton.PresentableContent = ControlFactory.CreateTradingRepresentableContent(_selectedItemButton.PresentableContent, false);

                if (!_sellItems.ContainsKey(_selectedItem))
                {
                    _sellItems.Add(_selectedItem, _amountChanged);
                }
                else
                {
                    _sellItems[_selectedItem] = _amountChanged;
                }

                InitializeTradePanel(false, _selectedItem);
            }
        }

        private void OnAcceptButtonClick(object sender, MouseEventArgs e)
        {
            //_demandList = _buyItems.Keys.ToList();

            //for (int i = 0; i < _demandList.Count; ++i)
            //{
            //    var type = _demandList[i].GetType();
            //    var protoItem = Activator.CreateInstance(type);

            //    if (protoItem is Usable)
            //    {
            //        var cloneItem = ObjectExtensions.Copy(_demandList[i]);
            //        ((Usable)cloneItem).UsableTimes = Math.Abs(_buyItems[_demandList[i]]);
            //        ((Usable)_demandList[i]).UsableTimes = ((Usable)cloneItem).UsableTimes;
            //    }
            //    else
            //    {
            //        var cloneItem = ObjectExtensions.Copy(_demandList[i]);
            //        _demandList[i] = cloneItem;
            //    }
            //}

            _demandList = new List<ICarriable>();
            foreach (var buyItem in _buyItems)
            {
                var type = buyItem.Key.GetType();
                var rawItem = (ICarriable)Activator.CreateInstance(type);
                var usableItem = rawItem as Usable;

                if (usableItem != null)
                    usableItem.UsableTimes = Math.Abs(buyItem.Value);

                _demandList.Add(rawItem);
            }


            //_offerList = _sellItems.Keys.ToList();

            //for (int i = 0; i < _offerList.Count; ++i)
            //{
            //    var type = _offerList[i].GetType();
            //    var protoItem = Activator.CreateInstance(type);

            //    if (protoItem is Usable)
            //    {
            //        var cloneItem = ObjectExtensions.Copy(_offerList[i]);
            //        ((Usable)cloneItem).UsableTimes = Math.Abs(_sellItems[_offerList[i]]);
            //        ((Usable)_offerList[i]).UsableTimes = ((Usable)cloneItem).UsableTimes;
            //    }
            //    else
            //    {
            //        var cloneItem = ObjectExtensions.Copy(_offerList[i]);
            //        _offerList[i] = cloneItem;
            //    }
            //}

            _offerList = new List<ICarriable>();
            foreach (var buyItem in _sellItems)
            {
                var type = buyItem.Key.GetType();
                var rawItem = (ICarriable)Activator.CreateInstance(type);
                var usableItem = rawItem as Usable;

                if (usableItem != null)
                    usableItem.UsableTimes = Math.Abs(buyItem.Value);

                _offerList.Add(rawItem);
            }

            BrokerClient.Submit(_offerList, _demandList);
        }

        private void OnCancelButtonClick(object sender, MouseEventArgs e)
        {
            BrokerClient.Submit(new List<ICarriable>(), new List<ICarriable>());
            GameContext.GameInstance.ChangeScreen(Dovahkiin.GamePlayScreen);
        }

        private void OnDealChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void OnDealAccepted(object sender, EventArgs e)
        {
            //UpdateUI();
            GameContext.GameInstance.ChangeScreen(Dovahkiin.GamePlayScreen);
        }
    }
}
