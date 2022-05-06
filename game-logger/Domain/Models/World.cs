namespace Domain.Models
{
    public class World
    {
        public int? Size { get; set; }
        public int CurrentTick { get; set; }
        public Map Map { get; set; }

        public static World GetStaticFields(World world)
        {
            return new World
            {
                Size = world.Size,
                //CurrentTick = world.CurrentTick,
                Map = Map.GetStaticFields(world.Map)
            };
        }

        public static World GetVariableFields(World previousWorld, World currentWorld)
        {
            return new World
            {
                CurrentTick = currentWorld.CurrentTick,
                Map = Map.GetVariableFields(previousWorld.Map,currentWorld.Map)
            };
        }

    }
}
