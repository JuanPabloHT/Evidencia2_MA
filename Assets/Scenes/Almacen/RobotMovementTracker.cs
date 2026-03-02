using UnityEngine;

public class RobotMovementTracker : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;

    private Vector3 lastPos;
    private float distAccum = 0f;
    private int moves = 0;

    public int Moves => moves;
    public float Distance => distAccum;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        Vector3 p = transform.position;
        float d = Vector3.Distance(p, lastPos);

        if (d > 0.0001f)
        {
            distAccum += d;

            int newMoves = Mathf.FloorToInt(distAccum / Mathf.Max(0.0001f, cellSize));
            if (newMoves > moves) moves = newMoves;
        }

        lastPos = p;
    }

    public void SetCellSize(float s)
    {
        cellSize = Mathf.Max(0.0001f, s);
    }
}