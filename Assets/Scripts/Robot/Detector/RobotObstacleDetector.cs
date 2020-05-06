using UnityAtoms;
using UnityEngine;

public class RobotObstacleDetector : RobotTileDetector
{
    protected override RoomTile tile => RoomTile.Obstacle;
}
