using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class RobotEdgeDetector : MonoBehaviour
{
    [SerializeField] private AtomEventBase doCheckOn;
    [SerializeField] private VoidEvent onEdgeChanged;
    [SerializeField] private Room room;

    public (RoomTile, RoomTile, RoomTile) UpEdge { get; private set; }
    public (RoomTile, RoomTile, RoomTile) DownEdge { get; private set; }
    public (RoomTile, RoomTile, RoomTile) LeftEdge { get; private set; }
    public (RoomTile, RoomTile, RoomTile) RightEdge { get; private set; }

    private int positionX;
    private int positionY;

    private void Start()
    {
        CheckEdges();

        doCheckOn.Register(CheckEdges);
    }

    private void OnDestroy()
    {
        doCheckOn.Unregister(CheckEdges);
    }

    private void CheckEdges()
    {
        InitPositionInInt();

        UpEdge = DetectUpEdge();
        DownEdge = DetectDownEdge();
        LeftEdge = DetectLeftEdge();
        RightEdge = DetectRightEdge();

        onEdgeChanged.Raise();
    }


    private void InitPositionInInt()
    {
        positionX = Mathf.RoundToInt(transform.position.x);
        positionY = Mathf.RoundToInt(transform.position.y);
    }

    private (RoomTile, RoomTile, RoomTile) DetectUpEdge()
    {
        int topPosition = positionY + 2;

        return (
            // Top left tile
            room.content[topPosition][positionX - 1].Item1,
            // Top middle tile
            room.content[topPosition][positionX].Item1,
            // Top right tile
            room.content[topPosition][positionX + 1].Item1
        );
    }

    private (RoomTile, RoomTile, RoomTile) DetectDownEdge()
    {
        int bottomPosition = positionY - 2;
        return (
            // Down left tile
            room.content[bottomPosition][positionX - 1].Item1,
            // Down middle tile
            room.content[bottomPosition][positionX].Item1,
            // Down right tile
            room.content[bottomPosition][positionX + 1].Item1
        );

    }

    private (RoomTile, RoomTile, RoomTile) DetectLeftEdge()
    {
        int leftPosition = positionX - 2;
        return (
            // Left top tile
            room.content[positionY + 1][leftPosition].Item1,
            // Left middle tile
            room.content[positionY][leftPosition].Item1,
            // Left bottom tile
            room.content[positionY - 1][leftPosition].Item1
        );
    }

    private (RoomTile, RoomTile, RoomTile) DetectRightEdge()
    {
        int rightPosition = positionX + 2;
        return (
            // Right top tile
            room.content[positionY + 1][rightPosition].Item1,
            // Right middle tile
            room.content[positionY][rightPosition].Item1,
            // Right bottom tile
            room.content[positionY - 1][rightPosition].Item1
        );
    }
}
