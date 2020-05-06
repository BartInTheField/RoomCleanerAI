using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    private RobotMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<RobotMovement>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            _movement.Move(Move.Up);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _movement.Move(Move.Down);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _movement.Move(Move.Right);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _movement.Move(Move.Left);
        }
    }
}
