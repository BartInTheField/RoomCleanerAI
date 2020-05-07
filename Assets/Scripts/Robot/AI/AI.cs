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
    [SerializeField] private bool useTimer = true;

    protected RobotMovement movement;
    protected RobotObstacleDetector obstacleDetector;
    protected RobotCleanFloorDetector cleanFloorDetector;
    protected RobotDirtyFloorDetector dirtyFloorDetector;

    protected bool canStart = true;
    protected bool hasStopped = false;

    protected virtual void Awake()
    {
        movement = GetComponent<RobotMovement>();

        obstacleDetector = GetComponent<RobotObstacleDetector>();
        cleanFloorDetector = GetComponent<RobotCleanFloorDetector>();
        dirtyFloorDetector = GetComponent<RobotDirtyFloorDetector>();
    }

    protected virtual void Start()
    {
        if(useTimer)
            delayTimer.OnTimeout += MoveRobot;

        doStopOn.Register(StopMoving);

        MoveRobot();
    }

    protected virtual void Update()
    {
        if (!useTimer)
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
        if (hasStopped || !canStart)
            return;

        movement.Move(MakeMoveDecision());
        AfterMove();
    }

    protected abstract Move MakeMoveDecision();

    protected virtual void AfterMove()
    {

    }

}
