using System;
using Dovahkiin.ActionHandler;
using Dovahkiin.Model.Core;

namespace Dovahkiin.Model.Action
{
    public class AgressiveLook : IAction
    {
        public string Title { get; set; }
        public Action<IActionHandler> EndCallback { get; set; }
        public ClanType AlliesClan { get; set; }
    }
}