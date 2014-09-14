using System;
using System.Linq;
using Dovahkiin.ActionHandler;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Extension
{
    public static class ActorExtension
    {
        public static IActionHandler GetActionHandler(this Actor actor, Type actionType)
        {
            return actor.ActionHandlers.FirstOrDefault(x => x.GetType() == actionType);
        }
    }
}