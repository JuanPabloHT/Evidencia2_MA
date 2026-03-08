using UnityEngine;
using System.Collections.Generic;

public class NodoRuta : MonoBehaviour
{
    [Header("¿A dónde puede ir el coche desde aquí?")]
    public List<NodoRuta> siguientesNodos;

    public NodoRuta ObtenerSiguienteAleatorio()
    {
        if (siguientesNodos == null || siguientesNodos.Count == 0) return null;
        
        int indiceAleatorio = Random.Range(0, siguientesNodos.Count);
        return siguientesNodos[indiceAleatorio];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        
        if (siguientesNodos != null)
        {
            foreach (NodoRuta nodo in siguientesNodos)
            {
                if (nodo != null)
                {
                    Gizmos.DrawLine(transform.position, nodo.transform.position);
                }
            }
        }
    }
}