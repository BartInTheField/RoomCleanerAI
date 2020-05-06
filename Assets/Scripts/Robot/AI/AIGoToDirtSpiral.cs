using BartInTheField.Timer;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

[RequireComponent(typeof(RobotMovement))]
public class AIGoToDirtSpiral : AI
{
    private bool isStart = true;
    private Move previousMove;

    protected override Move MakeMoveDecision()
    {
        // Go to dirt
        if (dirtyFloorDetector.IsUp && !obstacleDetector.IsUp)
            return Move.Up;

        if (dirtyFloorDetector.IsDown && !obstacleDetector.IsDown)
            return Move.Down;

        if (dirtyFloorDetector.Isleft && !obstacleDetector.Isleft)
            return Move.Left;

        if (dirtyFloorDetector.IsRight && !obstacleDetector.IsRight)
            return Move.Right;

        // Go spiral
        if ((isStart || previousMove == Move.Left))
        {
            if(obstacleDetector.IsUp)
            {
                if (isStart)
                    isStart = false;

                previousMove = Move.Up;
            } else
                return Move.Up;
        }

        if(previousMove == Move.Up)
        {
            if(obstacleDetector.IsRight)
                previousMove = Move.Right;
            else
                return Move.Right;
        }

        if(previousMove == Move.Right)
        {
            if (obstacleDetector.IsDown)
                previousMove = Move.Down;
            else 
                return Move.Down;
        } 

        if(previousMove == Move.Down)
        {
            if(obstacleDetector.Isleft)
                previousMove = Move.Left;
            else
                return Move.Left;
        }

        // When stuck this code executes
        return default;
    }
}
