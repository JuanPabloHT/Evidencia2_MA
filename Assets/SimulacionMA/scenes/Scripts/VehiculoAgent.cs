using UnityEngine;

public class VehiculoAgent : Agent
{
[Header("Configuración de Movimiento")]
public float velocidadMaxima = 10f;
public float velocidadActual;
public float distanciaFrenado = 5f;

[Header("Navegación")]
public NodoRuta nodoDestino;

public enum EstadoVehiculo { Avanzando, Frenando, DetenidoSemaforo }
public EstadoVehiculo estado = EstadoVehiculo.Avanzando;

protected override void Update()
{
base.Update();
Percibir();
Deliberar();
Actuar();
}

public override void Percibir()
{
estado = EstadoVehiculo.Avanzando;
DetectarObstaculo();
}

public void DetectarObstaculo()
{
Vector3 origen = transform.position + Vector3.up * 0.5f;

Debug.DrawRay(origen, transform.forward * distanciaFrenado, Color.red);
RaycastHit[] impactos = Physics.RaycastAll(origen, transform.forward, distanciaFrenado);

foreach (RaycastHit hit in impactos)
{
if (hit.collider.gameObject == this.gameObject) continue;

if (hit.collider.CompareTag("Vehiculo") || hit.collider.CompareTag("Peaton"))
{
estado = EstadoVehiculo.Frenando;
return;
}
else if (hit.collider.CompareTag("ZonaEsperaVehiculo"))
{
SensorSemaforo sensor = hit.collider.GetComponent<SensorSemaforo>();
if (sensor != null && sensor.semaforo != null)
{
if (sensor.semaforo.estadoActual != SemaforoAgent.EstadoSemaforo.Verde)
{
estado = EstadoVehiculo.DetenidoSemaforo;
return;
}
}
}
}
}

public void Deliberar()
{
if (nodoDestino == null) return;

float distanciaAlNodo = Vector3.Distance(transform.position, nodoDestino.transform.position);

if (distanciaAlNodo < 2.0f)
{
nodoDestino = nodoDestino.ObtenerSiguienteAleatorio();
}
}

public override void Actuar()
{
if (nodoDestino == null)
{
FrenarInmediato();
return;
}

if (estado == EstadoVehiculo.Avanzando)
{
velocidadActual = Mathf.Lerp(velocidadActual, velocidadMaxima, Time.deltaTime * 1.5f);
}
else
{
FrenarInmediato();
}

AplicarMovimiento();
}

public void FrenarInmediato()
{
velocidadActual = Mathf.Lerp(velocidadActual, 0, Time.deltaTime * 8f);
}

private void AplicarMovimiento()
{
if (velocidadActual > 0.1f && nodoDestino != null)
{
Vector3 direccion = (nodoDestino.transform.position - transform.position).normalized;
if (direccion != Vector3.zero)
{
Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * 4f);
}

transform.Translate(Vector3.forward * velocidadActual * Time.deltaTime, Space.Self);
}
}

public override void EnviarMensaje(string mensaje, Agent receptor) { }
public override void RecibirMensaje(string mensaje, Agent emisor) { }
}