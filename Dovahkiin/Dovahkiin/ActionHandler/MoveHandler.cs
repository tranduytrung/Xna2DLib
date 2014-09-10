using System;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;
using tranduytrung.Xna.Animation;
using tranduytrung.Xna.Engine;

namespace Dovahkiin.ActionHandler
{
    public class MoveHandler : IActionHandler
    {
        private Storyboard _moveStoryboard;
        private IAction _currentAction;

        public bool Handle(Actor source, IAction action)
        {
            var moveAction = action as Move;

            if (moveAction == null)
                return false;

            var movableObject = source as IMovable;
            if (movableObject == null)
                return false;

            Stop();
            _currentAction = action;

            if (movableObject.MovingSpeed <= 0)
                return true;

            var dx = moveAction.X - movableObject.X;
            var dy = moveAction.Y - movableObject.Y;
            var distance = Math.Sqrt(dx*dx + dy*dy);
            var time = distance/movableObject.MovingSpeed;

            
            _moveStoryboard =
                StoryBuilder.Select(movableObject)
                    .Animate("X,Y", new object [] {moveAction.X, moveAction.Y}, TimeSpan.FromSeconds(time))
                    .ToStoryboard();

            var finishAnimation = new IntegerlAnimation(() => 0, Finish);
            finishAnimation.BeginTime = TimeSpan.FromSeconds(time);

            _moveStoryboard.Animations.Add(finishAnimation);
            AnimationManager.BeginAnimation(_moveStoryboard);

            return true;
        }

        private void Finish(int value)
        {
            if (_currentAction != null && _currentAction.EndCallback != null)
                _currentAction.EndCallback.Invoke(this);

            _currentAction = null;
        }

        public void Stop()
        {
            AnimationManager.EndAnimation(_moveStoryboard);
            Finish(0);
        }
    }
}
