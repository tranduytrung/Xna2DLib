using System;
using System.Linq;
using Dovahkiin.Constant;
using Dovahkiin.Model.Core;
using Dovahkiin.Repository;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace Dovahkiin.Screen
{
    public class QuickBattleScreen : ComponentBase
    {
        private static Random _random = new Random();

        private DockPanel _dockPanel;
        private StackPanel _preBattlePanel;
        private StackPanel _postBattlePanel;

        public IParty Attacker { get; set; }

        public QuickBattleScreen(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var canvas = new Canvas();
            PresentableContent = canvas;

            var background = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.MainMenuBackground)))
            {
                SpriteMode = SpriteMode.Fit
            };
            canvas.Children.Add(background);

            _dockPanel = new DockPanel();
            _dockPanel.AutoFillLastChild = true;
            canvas.Children.Add(_dockPanel);

            _preBattlePanel = new StackPanel();
            _preBattlePanel.Orientation = StackOrientation.Vertical;
            _preBattlePanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _preBattlePanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            _dockPanel.Children.Add(_preBattlePanel);

            _postBattlePanel = new StackPanel();
            _postBattlePanel.Orientation = StackOrientation.Vertical;;
            _postBattlePanel.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            _postBattlePanel.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            _dockPanel.Children.Add(_postBattlePanel);
        }

        public override void OnTransitFrom()
        {
            DisplayPreBattkeInfo();
        }

        private void DisplayPreBattkeInfo()
        {
            _dockPanel.Children.Clear();
            _dockPanel.Children.Add(_preBattlePanel);
            _preBattlePanel.Children.Clear();

            var infoStack = new StackPanel();
            _preBattlePanel.Children.Add(infoStack);

            var ourPower = CalculatePower((IParty)DataContext.Current.ControllingObject);
            var theirPower = CalculatePower(Attacker);
            var victoryChance = MathHelper.Clamp((float)ourPower * 100 / (theirPower + ourPower), 0, 100);

            var ourStack = new StackPanel();
            ourStack.BackgroundColor = Color.Black * 0.5f;
            ourStack.Margin= new Margin(6,12);
            ourStack.Orientation = StackOrientation.Vertical;
            infoStack.Children.Add(ourStack);
            
            ourStack.Children.Add(new SpriteText(Resouces.GetFont(Fonts.ButtonFont)) {Text = "Your party", TintingColor = Color.YellowGreen});
            ourStack.Children.Add(new SpriteText(Resouces.GetFont(Fonts.DescriptionFont)) { Text = string.Format("Power: {0}", ourPower), TintingColor = Color.PowderBlue });
            ourStack.Children.Add(new SpriteText(Resouces.GetFont(Fonts.DescriptionFont)) { Text = string.Format("Victory chance: {0:F}%", victoryChance), TintingColor = Color.PowderBlue });

            var theirStack = new StackPanel();
            theirStack.BackgroundColor = Color.Black * 0.5f;
            theirStack.Margin = new Margin(6,12);
            theirStack.Orientation = StackOrientation.Vertical;
            infoStack.Children.Add(theirStack);

            theirStack.Children.Add(new SpriteText(Resouces.GetFont(Fonts.ButtonFont)) { Text = "Their party", TintingColor = Color.Salmon });
            theirStack.Children.Add(new SpriteText(Resouces.GetFont(Fonts.DescriptionFont)) { Text = string.Format("Power: {0}", theirPower), TintingColor = Color.PowderBlue });
            theirStack.Children.Add(new SpriteText(Resouces.GetFont(Fonts.DescriptionFont)) { Text = string.Format("Victory chance: {0:F}%", 100 - victoryChance), TintingColor = Color.PowderBlue });

            var battleButton = ControlFactory.CreateButton("to battle");
            battleButton.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            battleButton.Click += ToBattle;
            _preBattlePanel.Children.Add(battleButton);
        }

        private void ToBattle(object sender, MouseEventArgs e)
        {
            var ourPower = CalculatePower((IParty)DataContext.Current.ControllingObject);
            var theirPower = CalculatePower(Attacker);
            var victoryChance = MathHelper.Clamp((float)ourPower * 100 / (theirPower + ourPower), 0, 100);

            
        }

        private static int CalculatePower(IParty party)
        {
            return party.Members.Sum(member => member.Power + member.HitPoint + member.MagicPoint + 20*member.Level);
        }
    }
}