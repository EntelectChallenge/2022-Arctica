using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class BotMapState
    {
        public List<Guid> ScoutTowers { get; set; }
        public List<Guid> Nodes { get; set; }

        public static BotMapState GetVariableFields(BotMapState previousBMS, BotMapState currenetBMS)
        {
            return new BotMapState
            {
                ScoutTowers = previousBMS.Nodes.SequenceEqual(currenetBMS.ScoutTowers) ? null : currenetBMS.ScoutTowers,
                Nodes = previousBMS.Nodes.SequenceEqual(currenetBMS.Nodes) ? null : currenetBMS.Nodes
            };
        }
    }
}