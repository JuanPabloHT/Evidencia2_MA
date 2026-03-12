using UnityEngine;

public class SensorSemaforo : MonoBehaviour
{
public SemaforoAgent semaforo;
public bool esSensorPeatonal;

void Start()
{
if (semaforo == null)
{
VincularSemaforoCercano();
}
}

[ContextMenu("Vincular Semaforo Cercano")]
public void VincularSemaforoCercano()
{
SemaforoAgent[] todosLosSemaforos = FindObjectsByType<SemaforoAgent>(FindObjectsSortMode.None);
float distanciaMinima = Mathf.Infinity;
SemaforoAgent semaforoMasCercano = null;

foreach (SemaforoAgent s in todosLosSemaforos)
{
float distancia = Vector3.Distance(transform.position, s.transform.position);
if (distancia < distanciaMinima)
{
distanciaMinima = distancia;
semaforoMasCercano = s;
}
}

if (semaforoMasCercano != null)
{
semaforo = semaforoMasCercano;
Debug.Log("Semaforo más cercano vinculado a " + gameObject.name);
}
}

void OnTriggerEnter(Collider other)
{
if (semaforo == null) return; 

if (esSensorPeatonal && other.CompareTag("Peaton"))
{
semaforo.RegistrarPeaton();
}
else if (!esSensorPeatonal && other.CompareTag("Vehiculo"))
{
semaforo.RegistrarVehiculo();
}
}

void OnTriggerExit(Collider other)
{
if (semaforo == null) return; 

if (esSensorPeatonal && other.CompareTag("Peaton"))
{
semaforo.LiberarPeaton();
}
else if (!esSensorPeatonal && other.CompareTag("Vehiculo"))
{
semaforo.LiberarVehiculo();
}
}
}