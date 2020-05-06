using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

[RequireComponent(typeof(RobotEdgeDetector))]
public class RobotEdgeLogger : MonoBehaviour
{
    [SerializeField] private AtomEventBase doLogOn;

    private RobotEdgeDetector edgeDetector;

    private void Awake()
    {
        edgeDetector = GetComponent<RobotEdgeDetector>();

        doLogOn.Register(LogEdges);
    }

    private void OnDestroy()
    {
        doLogOn.Unregister(LogEdges);
    }

    private void LogEdges()
    {
        var (TopLeft, TopMiddle, TopRight) = edgeDetector.UpEdge;
        DebugGUI.LogPersistent("RobotUpEdge", $"Up edge: " +
            $"Left({Enum.GetName(typeof(RoomTile), TopLeft)}) " +
            $"Middle({Enum.GetName(typeof(RoomTile), TopMiddle)}) " +
            $"Right({Enum.GetName(typeof(RoomTile), TopRight)})");

        var (DownLeft, DownMiddle, DownRight) = edgeDetector.DownEdge;
        DebugGUI.LogPersistent("RobotDownEdge", $"Down edge: " +
            $"Left({Enum.GetName(typeof(RoomTile), DownLeft)}) " +
            $"Middle({Enum.GetName(typeof(RoomTile), DownMiddle)}) " +
            $"Right({Enum.GetName(typeof(RoomTile), DownRight)})");

        var (LeftTop, LeftMiddle, LeftBottom) = edgeDetector.LeftEdge;
        DebugGUI.LogPersistent("RobotLeftEdge", $"Left edge: " +
            $"Top({Enum.GetName(typeof(RoomTile), LeftTop)}) " +
            $"Middle({Enum.GetName(typeof(RoomTile), LeftMiddle)}) " +
            $"Bottom({Enum.GetName(typeof(RoomTile), LeftBottom)})");

        var (RightTop, RightMiddle, RightBottom) = edgeDetector.RightEdge;
        DebugGUI.LogPersistent("RobotRightEdge", $"Right edge: " +
            $"Top({Enum.GetName(typeof(RoomTile), RightTop)}) " +
            $"Middle({Enum.GetName(typeof(RoomTile), RightMiddle)}) " +
            $"Bottom({Enum.GetName(typeof(RoomTile), RightBottom)})");
    }
}
