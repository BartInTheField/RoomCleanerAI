using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "RoomCleaner/Room", order = 1)]
public class Room : ScriptableObject
{
    public List<List<(RoomTile, GameObject)>> content = new List<List<(RoomTile, GameObject)>> { };

    public int GetAmountOfTilesOfType(RoomTile tileType)
    {
        int amount = 0;

        foreach (List<(RoomTile, GameObject)> tiles in content)
        {
            foreach ((RoomTile, GameObject) tile in tiles)
            {
                if (tile.Item1 == tileType)
                    amount++;
            }
        }

        return amount;
    }
}
