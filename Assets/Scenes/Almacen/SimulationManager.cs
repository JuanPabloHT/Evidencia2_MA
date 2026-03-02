using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [Header("Condiciones de término")]
    [SerializeField] private DropZone dropZone;
    [SerializeField] private float maxTimeSeconds = 180f;

    [Header("Métricas")]
    [SerializeField] private float cellSize = 1f;

    [Header("Debug/Estado")]
    [SerializeField] private string boxTag = "Box";

    private float startTime;
    private bool ended;

    private int totalBoxes; 

    void Start()
    {
        startTime = Time.time;

        GameObject[] boxes = GameObject.FindGameObjectsWithTag(boxTag);
        totalBoxes = (boxes != null) ? boxes.Length : 0;

        Debug.Log($"[SimulationManager] Cajas detectadas al inicio: {totalBoxes}");

        var trackers = FindObjectsByType<RobotMovementTracker>(FindObjectsSortMode.None);
        for (int i = 0; i < trackers.Length; i++)
        {
            trackers[i].SetCellSize(cellSize);
        }
    }

    void Update()
    {
        if (ended) return;

        float elapsed = Time.time - startTime;

        bool allStacked = (dropZone != null && dropZone.TotalPlaced >= totalBoxes && totalBoxes > 0);
        bool timeUp = (elapsed >= maxTimeSeconds);

        if (allStacked || timeUp)
        {
            EndSimulation(allStacked, elapsed);
        }
    }

    private void EndSimulation(bool allStacked, float elapsed)
    {
        ended = true;

        int totalMoves = 0;
        float totalDistance = 0f;

        var trackers = FindObjectsByType<RobotMovementTracker>(FindObjectsSortMode.None);
        for (int i = 0; i < trackers.Length; i++)
        {
            totalMoves += trackers[i].Moves;
            totalDistance += trackers[i].Distance;
        }

        Debug.Log("=== SIMULATION END ===");
        Debug.Log(allStacked
            ? $"FIN: Todas las cajas apiladas (<=5). Tiempo: {elapsed:F2}s (Placed: {dropZone.TotalPlaced}/{totalBoxes})"
            : $"FIN: Tiempo máximo alcanzado. Tiempo: {elapsed:F2}s (Placed: {(dropZone != null ? dropZone.TotalPlaced : 0)}/{totalBoxes})");
        Debug.Log($"Movimientos totales: {totalMoves}");
        Debug.Log($"Distancia total: {totalDistance:F2} unidades");

    }
}