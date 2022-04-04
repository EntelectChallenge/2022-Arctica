using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Domain.Models
{
    public class World
    {
        public int Size { get; set; }
        public int CurrentTick { get; set; }
        public Map Map { get; set; }
    }
}