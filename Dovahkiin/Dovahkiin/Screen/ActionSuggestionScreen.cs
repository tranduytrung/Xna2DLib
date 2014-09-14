using Dovahkiin.Constant;
using Dovahkiin.Control;
using Dovahkiin.Model.Core;
using Dovahkiin.Repository;
using Dovahkiin.Utility;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Control;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Engine;

namespace Dovahkiin.Screen
{
    public class ActionSuggestionScreen : ComponentBase
    {
        private StackPanel _actionPanel;

        public Actor TargetActor { get; set; }

        public ActionSuggestionScreen(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var canvas = new Canvas();
            PresentableContent = canvas;

            var background = new Sprite(new SingleSpriteSelector(Resouces.GetTexture(Textures.MainMenuBackground)));
            background.SpriteMode = SpriteMode.Fit;
            canvas.Children.Add(background);

            var panelContainer = new DockPanel();
            panelContainer.AutoFillLastChild = true;
            canvas.Children.Add(panelContainer);

            var wrapper = new StackPanel();
            wrapper.SetValue(AlignmentExtension.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            wrapper.SetValue(AlignmentExtension.VerticalAlignmentProperty, VerticalAlignment.Center);
            wrapper.Orientation = StackOrientation.Vertical;
            wrapper.BackgroundColor = Color.Gray * 0.3f;
            panelContainer.Children.Add(wrapper);

            _actionPanel = new StackPanel();
            wrapper.Children.Add(_actionPanel);

            var backButton = ControlFactory.CreateButton("back");
            backButton.Click +=Back;
            wrapper.Children.Add(backButton);
        }

        private void Back(object sender, MouseEventArgs e)
        {
            GameContext.GameInstance.ChangeScreen(Dovahkiin.GamePlayScreen);
        }

        public override void OnTransitFrom()
        {
            var sourceActor = Dovahkiin.GamePlayScreen.ControllingObject.Model as Actor;
            if (sourceActor == null)
                return;

            var actions = TargetActor.GetSuggestionActions(sourceActor);
            _actionPanel.Children.Clear();
            foreach (var action in actions)
            {
                var button = ControlFactory.CreateButton(action.Title);
                button.Tag = action;
                button.Click += DoAction;
                _actionPanel.Children.Add(button);
            }
        }

        private void DoAction(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;
            var sourceActor = (Actor)Dovahkiin.GamePlayScreen.ControllingObject.Model;
            sourceActor.DoAction((IAction) button.Tag);
            GameContext.GameInstance.ChangeScreen(Dovahkiin.GamePlayScreen);
        }
    }
}