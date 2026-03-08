using UnityEngine;

public class SensorSemaforo : MonoBehaviour
{
public SemaforoAgent semaforo;
public bool esSensorPeatonal;

void OnTriggerEnter(Collider other)
{
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