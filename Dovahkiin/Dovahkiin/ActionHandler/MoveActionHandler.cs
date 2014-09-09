using System;
using Dovahkiin.Model.Action;
using Dovahkiin.Model.Core;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Animation;
using tranduytrung.Xna.Engine;

namespace Dovahkiin.ActionHandler
{
    public class MoveActionHandler : IActionHandler
    {
        private Storyboard _moveStoryboard;

        public bool Handle(IAction action)
        {
            var moveAction = action as MoveAction;

            if (moveAction == null)
                return false;

            var movableObject = moveAction.Target as IMovable;
            if (movableObject == null)
                return false;

            AnimationManager.EndAnimation(_moveStoryboard);
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
            AnimationManager.BeginAnimation(_moveStoryboard);

            return true;
        }
    }
}
