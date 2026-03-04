using UnityEngine;

public class BoxStacking : MonoBehaviour
{
    [Header("Estado")]
    [SerializeField] public int numberOfBoxes = 0;

    [Header("Configuración")]
    [SerializeField] public int maxNumberOfBoxes = 5;
    [SerializeField] public float positionOffset = 0.6f;      // altura entre cajas
    [SerializeField] public float initialVerticalPosition = 0.3f; // primera caja sobre el piso

    public bool TryStackBox(GameObject box)
    {
        if (box == null) return false;
        if (numberOfBoxes >= maxNumberOfBoxes) return false;

        // Calcula la posición exacta del "slot" en la pila
        Vector3 pos = new Vector3(
            transform.position.x,
            transform.position.y + initialVerticalPosition + positionOffset * numberOfBoxes,
            transform.position.z
        );

        // Snapea la caja al slot
        box.transform.SetPositionAndRotation(pos, Quaternion.identity);

        box.transform.SetParent(transform, true);

        Rigidbody rb = box.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        box.tag = "Stacked";


        Collider col = box.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        numberOfBoxes++;
        return true;
    }

    public bool IsFull()
    {
        return numberOfBoxes >= maxNumberOfBoxes;
    }
}