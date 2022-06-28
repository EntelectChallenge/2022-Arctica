using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class StatusMultiplier { 
    
        public int WoodReward { get; set; }
        public int FoodReward { get; set; }
        public int StoneReward { get; set; }
        public int GoldReward { get; set; }
        public int HeatReward { get; set; }

        public StatusMultiplier() { 
            WoodReward = 0;
            FoodReward = 0;
            StoneReward = 0;
            GoldReward = 0;
            HeatReward = 0;
        }

        //TODO: dimishing returns on duplicat buildings
    }
}
