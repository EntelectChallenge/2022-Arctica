using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PopulationTier
    {
        public int Level { get; set; }// 1 - 5 //add to readme
        public string Name { get; set; }// Tier 1 - 5 //add to readme
        public int MaxPopulation { get; set; }// max population //add to readme
        public IList<double> PopulationChangeFactorRange { get; set; }// -0.05, 0.05 //add to readme
        public TierResourceConstraints TierResourceConstraints { get; set; }// Food, Wood, Stone //add to readme
    }
}
