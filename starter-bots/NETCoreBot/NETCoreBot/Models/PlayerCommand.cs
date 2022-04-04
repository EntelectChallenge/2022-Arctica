using NETCoreBot.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class PlayerCommand
    {
        public Guid? PlayerId { get; set; }
        public List<CommandAction> Actions { get; set; }

        public PlayerCommand()
        {
            this.Actions = new List<CommandAction>();
        }
    }
}
