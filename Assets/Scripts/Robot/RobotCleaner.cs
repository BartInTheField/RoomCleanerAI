using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class RobotCleaner : MonoBehaviour
{
    [SerializeField] private VoidEvent doCleanOn;
    [SerializeField] private VoidEvent onCleaned;
    [SerializeField] private Room room;

    private void Awake()
    {
        Clean();

        doCleanOn.Register(Clean);
    }

    private void OnDestroy()
    {
        doCleanOn.Unregister(Clean);
    }

    public void Clean()
    {
        int positionY = Mathf.RoundToInt(transform.position.y);
        int positionX = Mathf.RoundToInt(transform.position.x);

        // Clean middle row of robot
        room.content[positionY][positionX] = (RoomTile.CleanFloor, room.content[positionY][positionX].Item2);
        room.content[positionY][positionX - 1] = (RoomTile.CleanFloor, room.content[positionY][positionX - 1].Item2);
        room.content[positionY][positionX + 1] = (RoomTile.CleanFloor, room.content[positionY][positionX + 1].Item2);

        // Clean top row of robot
        room.content[positionY + 1][positionX] = (RoomTile.CleanFloor, room.content[positionY + 1][positionX].Item2);
        room.content[positionY + 1][positionX - 1] = (RoomTile.CleanFloor, room.content[positionY + 1][positionX - 1].Item2);
        room.content[positionY + 1][positionX + 1] = (RoomTile.CleanFloor, room.content[positionY + 1][positionX + 1].Item2);

        // Clean bottom row of robot
        room.content[positionY - 1][positionX] = (RoomTile.CleanFloor, room.content[positionY - 1][positionX].Item2);
        room.content[positionY - 1][positionX - 1] = (RoomTile.CleanFloor, room.content[positionY - 1][positionX - 1].Item2);
        room.content[positionY - 1][positionX + 1] = (RoomTile.CleanFloor, room.content[positionY - 1][positionX + 1].Item2);

        onCleaned.Raise();
    }
}
