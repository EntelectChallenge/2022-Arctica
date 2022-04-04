using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
        public class CommandAction
        {
            public ActionType Type { get; set; }
            public int Units { get; set; }
            public Position TargetNode { get; set; }
        }
}
