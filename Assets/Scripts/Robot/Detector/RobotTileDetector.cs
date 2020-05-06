using UnityAtoms;
using UnityEngine;

[RequireComponent(typeof(RobotEdgeDetector))]
public abstract class RobotTileDetector : MonoBehaviour
{
    protected abstract RoomTile tile { get; }

    [SerializeField] private AtomEventBase doCheckOn;
    [SerializeField] private Room room;

    private RobotEdgeDetector edgeDetector;

    public bool IsUp { get; private set; }
    public bool IsDown { get; private set; }
    public bool Isleft { get; private set; }
    public bool IsRight { get; private set; }

    private void Awake()
    {
        edgeDetector = GetComponent<RobotEdgeDetector>();

        doCheckOn.Register(CheckTiles);
    }

    private void OnDestroy()
    {
        doCheckOn.Unregister(CheckTiles);
    }

    private void CheckTiles()
    {

        IsUp = DetectTileUp();
        IsDown = DetectTileDown();
        Isleft = DetectTileLeft();
        IsRight = DetectTileRight();
    }

    private bool DetectTileUp()
    {
        return CheckEdge(edgeDetector.UpEdge);
    }

    private bool DetectTileDown()
    {
        return CheckEdge(edgeDetector.DownEdge);
    }

    private bool DetectTileLeft()
    {
        return CheckEdge(edgeDetector.LeftEdge);
    }

    private bool DetectTileRight()
    {
        return CheckEdge(edgeDetector.RightEdge);
    }

    private bool CheckEdge((RoomTile, RoomTile, RoomTile) edges)
    {
        var (firstTile, secondTile, thirdTile) = edges;

        return (
            firstTile == tile ||
            secondTile == tile ||
            thirdTile == tile
        );
    }
}
