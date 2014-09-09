using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dovahkiin.ActionHandler;

namespace Dovahkiin.Model.Core
{
    public abstract class Actor
    {
        private class ActionHandlerCollection : KeyedCollection<Type, IActionHandler>
        {
            protected override Type GetKeyForItem(IActionHandler item)
            {
                return item.GetType();
            }
        }

        private readonly ActionHandlerCollection _actionHandlers;
        public IEnumerable<IActionHandler> ActionHandlers
        {
            get { return _actionHandlers; }
        }

        protected Actor()
        {
            _actionHandlers = new ActionHandlerCollection();
        }

        public bool DoAction(IAction action)
        {
            return _actionHandlers.Any(handler => handler.Handle(action));
        }

        internal void AddActionHandler(IActionHandler handler)
        {
            _actionHandlers.Add(handler);
        }

        internal void RemoveActionHandler(Type hanlderType)
        {
            _actionHandlers.Remove(hanlderType);
        }

        public bool HasActionHandler(Type handlerType)
        {
            return _actionHandlers.Contains(handlerType);
        }
    }
}
