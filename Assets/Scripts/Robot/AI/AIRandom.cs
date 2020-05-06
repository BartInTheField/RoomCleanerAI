using BartInTheField.Timer;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

[RequireComponent(typeof(RobotMovement))]
public class AIRandom : AI
{
    protected override Move MakeMoveDecision()
    {
        return (Move) Random.Range(0, System.Enum.GetNames(typeof(Move)).Length);
    }
}
