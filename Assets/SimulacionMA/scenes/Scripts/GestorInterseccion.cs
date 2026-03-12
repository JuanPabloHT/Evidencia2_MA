using UnityEngine;
using System.Collections.Generic;

public class GestorInterseccion : MonoBehaviour
{
public float radioBusqueda = 15f;
public List<SemaforoAgent> semaforosCercanos = new List<SemaforoAgent>();

[ContextMenu("Vincular Semaforos")]
public void VincularSemaforos()
{
semaforosCercanos.Clear();
SemaforoAgent[] todosLosSemaforos = FindObjectsOfType<SemaforoAgent>();
foreach (SemaforoAgent semaforo in todosLosSemaforos)
{
float distancia = Vector3.Distance(transform.position, semaforo.transform.position);
if (distancia <= radioBusqueda)
{
semaforosCercanos.Add(semaforo);
}
}
Debug.Log("Semáforos vinculados: " + semaforosCercanos.Count);
}
}