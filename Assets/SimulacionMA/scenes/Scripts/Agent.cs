using UnityEngine;
using System.Collections.Generic;

public abstract class Agent : MonoBehaviour
{
public int id;
public Vector3 posicion;

protected virtual void Update()
{
posicion = transform.position;
}

public abstract void Percibir();
public abstract void Actuar();
public abstract void EnviarMensaje(string mensaje, Agent receptor);
public abstract void RecibirMensaje(string mensaje, Agent emisor);
}