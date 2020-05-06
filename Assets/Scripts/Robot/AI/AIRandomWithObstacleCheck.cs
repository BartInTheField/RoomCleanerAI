using BartInTheField.Timer;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

[RequireComponent(typeof(RobotMovement))]
public class AIRandomWithObstacleCheck : AI
{
    protected override Move MakeMoveDecision()
    {
        Move randomMove = (Move)Random.Range(0, System.Enum.GetNames(typeof(Move)).Length);

        switch(randomMove)
        {
            case (Move.Up):
                if (obstacleDetector.IsUp)
                    randomMove = MakeMoveDecision();
                break;
            case (Move.Down):
                if (obstacleDetector.IsDown)
                    randomMove = MakeMoveDecision();
                break;
            case (Move.Left):
                if (obstacleDetector.Isleft)
                    randomMove = MakeMoveDecision();
                break;
            case (Move.Right):
                if (obstacleDetector.IsRight)
                    randomMove = MakeMoveDecision();
                break;
        }

        return randomMove;
    }
}
