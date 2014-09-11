using System;
using Dovahkiin.ActionHandler;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Action
{
    public class UseItem : IAction
    {
        public ICreature Target { get; set; }
        public Usable UsableItem { get; set; }
        public Action<IActionHandler> EndCallback { get; set; }
    }
}