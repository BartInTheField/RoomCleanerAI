using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public enum Move
{
    Up,
    Down,
    Left,
    Right
}

[RequireComponent(typeof(RobotObstacleDetector))]
public class RobotMovement : MonoBehaviour
{
    [SerializeField] private VoidEvent onMoved;

    private RobotObstacleDetector obstacleDetector;

    private void Awake()
    {
        obstacleDetector = GetComponent<RobotObstacleDetector>();
    }

    public void Move(Move move)
    {
        DebugGUI.LogPersistent("RobotMove", $"Robot moved: {move}");

        switch(move)
        {
            case global::Move.Up:
                Up();
                break;
            case global::Move.Down:
                Down();
                break;
            case global::Move.Left:
                Left();
                break;
            case global::Move.Right:
                Right();
                break;
        }

        onMoved.Raise();
    }

    private void Up()
    {
        if (obstacleDetector.IsUp)
            return;

        transform.position = new Vector2(transform.position.x, transform.position.y + 1);
    }

    private void Down()
    {
        if (obstacleDetector.IsDown)
            return;

        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
    }

    private void Left()
    {
        if (obstacleDetector.Isleft)
            return;

        transform.position = new Vector2(transform.position.x - 1, transform.position.y);
    }

    private void Right()
    {
        if (obstacleDetector.IsRight)
            return;

        transform.position = new Vector2(transform.position.x + 1, transform.position.y);
    }
}
