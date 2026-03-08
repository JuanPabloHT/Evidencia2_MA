using UnityEngine;

public class SpawnerAgentes : MonoBehaviour
{
    public GameObject[] prefabsAgentes;
    public Transform[] puntosDeSpawn;
    public float tiempoEntreSpawns = 3f;
    public int limiteMaximo = 10;

    private int agentesGenerados = 0;
    private float temporizador = 0f;

    [Header("Configuración Automática (Opcional)")]
    public NodoRuta nodoInicialCoches; 
    public SemaforoAgent semaforoAsignado;

    void Update()
    {
        if (agentesGenerados >= limiteMaximo) return;

        temporizador += Time.deltaTime;

        if (temporizador >= tiempoEntreSpawns)
        {
            GenerarAgente();
            temporizador = 0f;
        }
    }

    void GenerarAgente()
    {
        int indicePunto = Random.Range(0, puntosDeSpawn.Length);
        Collider[] estorbo = Physics.OverlapSphere(puntosDeSpawn[indicePunto].position, 2f);
        foreach (Collider c in estorbo)
        {
            if (c.CompareTag("Vehiculo")) return; 
        }

        int indicePrefab = Random.Range(0, prefabsAgentes.Length);
        GameObject nuevoAgente = Instantiate(prefabsAgentes[indicePrefab], puntosDeSpawn[indicePunto].position, puntosDeSpawn[indicePunto].rotation);
        agentesGenerados++;

        VehiculoAgent scriptCoche = nuevoAgente.GetComponent<VehiculoAgent>();
        if (scriptCoche != null && nodoInicialCoches != null)
        {
            scriptCoche.nodoDestino = nodoInicialCoches;
        }

        PeatonAgent scriptPeaton = nuevoAgente.GetComponent<PeatonAgent>();
        if (scriptPeaton != null && semaforoAsignado != null)
        {
            scriptPeaton.semaforoAsociado = semaforoAsignado;
        }
    }
}