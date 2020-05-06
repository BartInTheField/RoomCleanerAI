using UnityAtoms;
using UnityEngine;

public class RobotDirtyFloorDetector : RobotTileDetector
{
    protected override RoomTile tile => RoomTile.DirtyFloor;
}
