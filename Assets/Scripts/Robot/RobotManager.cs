using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [SerializeField] private GameObject robot;
    [SerializeField] private Vector2 spawnLocation;
    [SerializeField] private VoidEvent doSpawnRobotOn;

    private void Awake()
    {
        doSpawnRobotOn.Register(SpawnRobot);
    }

    private void OnDestroy()
    {
        doSpawnRobotOn.Unregister(SpawnRobot);
    }

    private void SpawnRobot()
    {
        Instantiate(robot, spawnLocation, Quaternion.identity);
    }
}
