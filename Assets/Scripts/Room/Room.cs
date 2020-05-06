using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "RoomCleaner/Room", order = 1)]
public class Room : ScriptableObject
{
    public List<List<(RoomTile, GameObject)>> content = new List<List<(RoomTile, GameObject)>> { };
}
