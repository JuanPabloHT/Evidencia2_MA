using UnityEngine;
using System.Collections.Generic;

public class SemaforoAgent : Agent
{
public enum EstadoSemaforo { Verde, Amarillo, Rojo }
public EstadoSemaforo estadoActual = EstadoSemaforo.Rojo;

[Header("Visuales y Barreras")]
public GameObject luzVerde;
public GameObject luzAmarilla;
public GameObject luzRoja;
public Collider barreraCoches;
public List<Collider> barrerasPeatones = new List<Collider>();

public int cantidadVehiculos = 0;
public int cantidadPeatones = 0;

void Start()
{
ActualizarFisicaYVisuales();
}

public void CambiarEstado(EstadoSemaforo nuevoEstado)
{
if (estadoActual != nuevoEstado)
{
estadoActual = nuevoEstado;
ActualizarFisicaYVisuales();
}
}

private void ActualizarFisicaYVisuales()
{
if(luzVerde != null) luzVerde.SetActive(estadoActual == EstadoSemaforo.Verde);
if(luzAmarilla != null) luzAmarilla.SetActive(estadoActual == EstadoSemaforo.Amarillo);
if(luzRoja != null) luzRoja.SetActive(estadoActual == EstadoSemaforo.Rojo);

if (barreraCoches != null)
{
barreraCoches.enabled = (estadoActual == EstadoSemaforo.Rojo || estadoActual == EstadoSemaforo.Amarillo);
}

foreach (Collider barrera in barrerasPeatones)
{
if (barrera != null)
{
barrera.enabled = (estadoActual == EstadoSemaforo.Verde || estadoActual == EstadoSemaforo.Amarillo);
}
}
}

public void RegistrarVehiculo() { cantidadVehiculos++; }
public void LiberarVehiculo() { cantidadVehiculos--; if(cantidadVehiculos < 0) cantidadVehiculos = 0; }
public void RegistrarPeaton() { cantidadPeatones++; }
public void LiberarPeaton() { cantidadPeatones--; if(cantidadPeatones < 0) cantidadPeatones = 0; }

public override void Percibir() { }
public override void Actuar() { }
public override void EnviarMensaje(string mensaje, Agent receptor) { }
public override void RecibirMensaje(string mensaje, Agent emisor) { }
}