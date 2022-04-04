using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace Engine.Services;

public class AsciiVisualizerService
{
    private char[,] Map;
    private int MapLength;
   
    public AsciiVisualizerService(int mapLength)
    {
        Map = new char[mapLength, mapLength];
        MapLength = mapLength;
    }

    public string GenerateMap(GameState state)
    {
        var output = "";

        for (int i = 0; i < MapLength; i++)
        {
            for (int j = 0; j < MapLength; j++)
            {
                output += Map[i, j];
            }
            output += "\n";
        }

        return output;
    }

    public void PlaceResource(GameObject gameObject, char representation)
    {
        int x = gameObject.Position.X;
        int y = gameObject.Position.Y;
        Map[x, y] = representation;
    }

    char ResourceSymbol(ResourceNode resourceNode)
    {
        return resourceNode.Type.ToString()[0];
    }
    
    
    
}