using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PopulationTier
    {
        public int Level { get; set; }// 1 - 5
        public string Name { get; set; }// Tier 1 - 5
        public int MaxPopulation { get; set; }// max population
        public IList<double> PopulationChangeFactorRange { get; set; }// -0.05, 0.05
        public TierResourceConstraints TierResourceConstraints { get; set; }// Food, Wood, Stone
    }
}
