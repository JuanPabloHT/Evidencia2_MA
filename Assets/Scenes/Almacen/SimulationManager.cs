using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationManager : MonoBehaviour
{
    [Header("Fin de simulación")]
    [SerializeField] private float maxExecutionTime = 120f; // segundos
    [SerializeField] private string remainingBoxTag = "Box";     // cajas sin ordenar
    [SerializeField] private string stackedBoxTag = "Stacked";   // cajas ya apiladas (solo por claridad)

    [Header("Métricas")]
    [SerializeField] private bool stopPlayModeWhenFinished = true;

    private float startTime;
    private bool finished;

    void Start()
    {
        startTime = Time.time;

        // Debug 
        int totalAtStart = GameObject.FindGameObjectsWithTag(remainingBoxTag).Length
                         + GameObject.FindGameObjectsWithTag(stackedBoxTag).Length;

        Debug.Log($"[SimulationManager] Inicio. Total cajas detectadas aprox: {totalAtStart}");
    }

    void Update()
    {
        if (finished) return;

        float elapsed = Time.time - startTime;

        // Condición 1: tiempo máximo
        if (elapsed >= maxExecutionTime)
        {
            FinishSimulation("Tiempo máximo alcanzado");
            return;
        }

        int remaining = GameObject.FindGameObjectsWithTag(remainingBoxTag).Length;

        if (remaining == 0)
        {
            FinishSimulation("Todas las cajas fueron organizadas");
        }
    }

    private void FinishSimulation(string reason)
    {
        finished = true;

        float totalTime = Time.time - startTime;

        // Suma de movimientos (desde RobotMovementTracker)
        RobotMovementTracker[] trackers = FindObjectsOfType<RobotMovementTracker>();
        float totalDistance = 0f;
        int totalSteps = 0;

        for (int i = 0; i < trackers.Length; i++)
        {
            totalDistance += trackers[i].Distance;
            totalSteps += trackers[i].Moves;
        }

        Debug.Log("========================================");
        Debug.Log($"[SimulationManager] FIN: {reason}");
        Debug.Log($"[SimulationManager] Tiempo total (s): {totalTime:F2}");
        Debug.Log($"[SimulationManager] Robots: {trackers.Length}");
        Debug.Log($"[SimulationManager] Distancia total recorrida: {totalDistance:F2}");
        Debug.Log($"[SimulationManager] Pasos/movimientos totales: {totalSteps}");
        Debug.Log("========================================");

        if (stopPlayModeWhenFinished)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            
            // Application.Quit();
#endif
        }
    }
}