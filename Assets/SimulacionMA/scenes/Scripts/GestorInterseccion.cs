using UnityEngine;
using System.Collections.Generic;

public class GestorInterseccion : MonoBehaviour
{
[Header("Semáforos de la Intersección (Asignar Manualmente)")]
public List<SemaforoAgent> ejeNorteSur = new List<SemaforoAgent>();
public List<SemaforoAgent> ejeEsteOeste = new List<SemaforoAgent>();

[Header("Tiempos (En Segundos)")]
public float tiempoVerde = 10f;
public float tiempoAmarillo = 3f;
[Tooltip("Tiempo de seguridad con todos en rojo")]
public float tiempoRojoLimpieza = 2f; 

private float temporizador = 0f;
private int faseActual = 0;

void Update()
{
GestionarTrafico();
}

private void GestionarTrafico()
{
temporizador += Time.deltaTime;

if (faseActual == 0) 
{
CambiarEstadoGrupo(ejeNorteSur, SemaforoAgent.EstadoSemaforo.Verde);
CambiarEstadoGrupo(ejeEsteOeste, SemaforoAgent.EstadoSemaforo.Rojo);
if (temporizador >= tiempoVerde) { faseActual = 1; temporizador = 0f; }
}
else if (faseActual == 1) 
{
CambiarEstadoGrupo(ejeNorteSur, SemaforoAgent.EstadoSemaforo.Amarillo);
if (temporizador >= tiempoAmarillo) { faseActual = 2; temporizador = 0f; }
}
else if (faseActual == 2) 
{
CambiarEstadoGrupo(ejeNorteSur, SemaforoAgent.EstadoSemaforo.Rojo);
CambiarEstadoGrupo(ejeEsteOeste, SemaforoAgent.EstadoSemaforo.Rojo);
if (temporizador >= tiempoRojoLimpieza) { faseActual = 3; temporizador = 0f; }
}
else if (faseActual == 3) 
{
CambiarEstadoGrupo(ejeNorteSur, SemaforoAgent.EstadoSemaforo.Rojo);
CambiarEstadoGrupo(ejeEsteOeste, SemaforoAgent.EstadoSemaforo.Verde);
if (temporizador >= tiempoVerde) { faseActual = 4; temporizador = 0f; }
}
else if (faseActual == 4) 
{
CambiarEstadoGrupo(ejeEsteOeste, SemaforoAgent.EstadoSemaforo.Amarillo);
if (temporizador >= tiempoAmarillo) { faseActual = 5; temporizador = 0f; }
}
else if (faseActual == 5) 
{
CambiarEstadoGrupo(ejeNorteSur, SemaforoAgent.EstadoSemaforo.Rojo);
CambiarEstadoGrupo(ejeEsteOeste, SemaforoAgent.EstadoSemaforo.Rojo);
if (temporizador >= tiempoRojoLimpieza) { faseActual = 0; temporizador = 0f; }
}
}

private void CambiarEstadoGrupo(List<SemaforoAgent> grupo, SemaforoAgent.EstadoSemaforo estado)
{
foreach (SemaforoAgent semaforo in grupo)
{
if (semaforo != null)
{
semaforo.CambiarEstado(estado);
}
}
}
}