using UnityEngine;
using System.Collections.Generic;

public class AutoConectorRuta : MonoBehaviour
{
[ContextMenu("Conectar Nodos")]
public void ConectarNodos()
{
NodoRuta[] nodos = GetComponentsInChildren<NodoRuta>();
for (int i = 0; i < nodos.Length - 1; i++)
{
nodos[i].siguientesNodos = new List<NodoRuta>();
nodos[i].siguientesNodos.Add(nodos[i + 1]);
}
Debug.Log("Ruta conectada");
}
}