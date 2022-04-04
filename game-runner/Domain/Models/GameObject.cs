using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Models
{
    public class GameObject
    {
        public Guid Id { get; set; }
        public GameObjectType GameObjectType { get; set; }
        public Position Position { get; set; }
    }
}