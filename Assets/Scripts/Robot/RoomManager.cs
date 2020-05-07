using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using System.Linq;

public enum RoomTile
{
    DirtyFloor,
    Obstacle,
    CleanFloor
}

public class RoomManager : MonoBehaviour
{

    [SerializeField] private VoidEvent onInitialDrawnRoom;
    [SerializeField] private VoidEvent doDrawOn;
    [SerializeField] private FloatEvent onFinishedCleaningRoom;

    [SerializeField] private GameObject dirtyFloor;
    [SerializeField] private GameObject cleanFloor;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private Room room;

    private float time = 0f;
    private int totalFloors = 0;
    private int cleanedFloors = 0;
    private bool stopTime = false;

    private void Awake()
    {
        List<List<int>> roomInts = new List<List<int>>
        {
            new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };

        foreach (List<int> tiles in roomInts)
        {
            List<(RoomTile, GameObject)> roomRow = new List<(RoomTile, GameObject)>();
            foreach (int tile in tiles)
            {
                roomRow.Add(((RoomTile)tile, null));
            }
            room.content.Add(roomRow);
        }

        doDrawOn.Register(Redraw);
    }

    private void OnDestroy()
    {
        doDrawOn.Unregister(Redraw);
    }

    private void Start()
    {
        Draw();

        totalFloors = GetAmountOfTilesOfType(RoomTile.CleanFloor) + GetAmountOfTilesOfType(RoomTile.DirtyFloor);

        onInitialDrawnRoom.Raise();
    }

    private void Update()
    {
        cleanedFloors = GetAmountOfTilesOfType(RoomTile.CleanFloor);

        if ((cleanedFloors != totalFloors) || stopTime)
            time += Time.deltaTime;

        DisplayData();

        if (cleanedFloors == totalFloors)
            onFinishedCleaningRoom.Raise();
    }

    public void StopTime()
    {
        stopTime = true;
    }

    private void Draw()
    {
        for (int tileListIndex = 0; tileListIndex < room.content.Count; tileListIndex++)
        {
            var tiles = room.content[tileListIndex];

            for (int tileIndex = 0; tileIndex < tiles.Count; tileIndex++)
            {
                GameObject objectToSpawn = null;
                switch (tiles[tileIndex].Item1)
                {
                    case RoomTile.DirtyFloor:
                        objectToSpawn = dirtyFloor;
                        break;
                    case RoomTile.Obstacle:
                        objectToSpawn = obstacle;
                        break;
                    case RoomTile.CleanFloor:
                        objectToSpawn = cleanFloor;
                        break;
                    default:
                        break;
                }

                tiles[tileIndex] = (tiles[tileIndex].Item1, SpawnObject(objectToSpawn, tileIndex, tileListIndex));
            }
        }

    }

    private void Redraw()
    {
        for (int tileListIndex = 0; tileListIndex < room.content.Count; tileListIndex++)
        {
            var tiles = room.content[tileListIndex];

            for (int tileIndex = 0; tileIndex < tiles.Count; tileIndex++)
            {
                var (currentTile, currentGameObject) = tiles[tileIndex];
                if (currentTile == RoomTile.CleanFloor && currentGameObject.name.Contains("Dirty"))
                {
                    Destroy(currentGameObject);
                    tiles[tileIndex] = (currentTile, SpawnObject(cleanFloor, tileIndex, tileListIndex));
                }
            }
        }
    }


    private GameObject SpawnObject(GameObject objectToSpawn, int x, int y)
    {
        return Instantiate(objectToSpawn, new Vector2(x, y), Quaternion.identity);
    }

    private int GetAmountOfTilesOfType(RoomTile tileType)
    {
        int amount = 0;

        foreach (List<(RoomTile, GameObject)> tiles in room.content)
        {
            foreach ((RoomTile, GameObject) tile in tiles)
            {
                if (tile.Item1 == tileType)
                    amount++;
            }
        }

        return amount;
    }

    private void DisplayData()
    {
        DebugGUI.LogPersistent("Time", $"Time: {time}");
        DebugGUI.LogPersistent("Floors", $"Cleaned {cleanedFloors} / {totalFloors}");
    }
}
