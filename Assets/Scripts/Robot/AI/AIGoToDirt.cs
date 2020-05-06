using BartInTheField.Timer;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

[RequireComponent(typeof(RobotMovement))]
public class AIGoToDirt : AI
{
    protected override Move MakeMoveDecision()
    {
        if (dirtyFloorDetector.IsUp &&  !obstacleDetector.IsUp)
            return Move.Up;

        if (dirtyFloorDetector.IsDown && !obstacleDetector.IsDown)
            return Move.Down;

        if (dirtyFloorDetector.Isleft && !obstacleDetector.Isleft)
            return Move.Left;

        if (dirtyFloorDetector.IsRight && !obstacleDetector.IsRight)
            return Move.Right;

        return Move.Up;
    }
}
