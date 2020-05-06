using UnityEngine;
using System.Collections;
using UnityAtoms;
using BartInTheField.Timer;


[RequireComponent(typeof(RobotMovement))]
[RequireComponent(typeof(RobotObstacleDetector))]
[RequireComponent(typeof(RobotDirtyFloorDetector))]
[RequireComponent(typeof(RobotCleanFloorDetector))]
public abstract class AI : MonoBehaviour
{

    [SerializeField] private AtomEventBase doStopOn;
    [SerializeField] private Timer delayTimer;

    protected RobotMovement movement;
    protected RobotObstacleDetector obstacleDetector;
    protected RobotCleanFloorDetector cleanFloorDetector;
    protected RobotDirtyFloorDetector dirtyFloorDetector;

    private bool hasStopped = false;

    private void Awake()
    {
        movement = GetComponent<RobotMovement>();

        obstacleDetector = GetComponent<RobotObstacleDetector>();
        cleanFloorDetector = GetComponent<RobotCleanFloorDetector>();
        dirtyFloorDetector = GetComponent<RobotDirtyFloorDetector>();
    }

    protected void Start()
    {
        delayTimer.OnTimeout += MoveRobot;
        doStopOn.Register(StopMoving);

        MoveRobot();
    }

    private void OnDestroy()
    {
        doStopOn.Unregister(StopMoving);
    }

    private void StopMoving()
    {
        hasStopped = true;
    }

    private void MoveRobot()
    {
        if (hasStopped)
            return;

        movement.Move(MakeMoveDecision());
    }

    protected abstract Move MakeMoveDecision();
}
