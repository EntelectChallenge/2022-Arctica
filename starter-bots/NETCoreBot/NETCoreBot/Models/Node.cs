using NETCoreBot.Enums;
using NETCoreBot.Models;

namespace Domain.Models
{
    public class Node
    {
        public Position Position { get; set; }
        
        public ResourceType Resource { get; set; }
        
        public int ResourceCount { get; set; }
    }
}