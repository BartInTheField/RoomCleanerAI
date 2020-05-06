using UnityAtoms;
using UnityEngine;

public class RobotCleanFloorDetector : RobotTileDetector
{
    protected override RoomTile tile => RoomTile.CleanFloor;
}
