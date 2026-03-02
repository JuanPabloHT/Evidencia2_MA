using UnityEngine;

public class addBox : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private GameObject carriedBox;

    public bool HasBox => carriedBox != null;

    void Awake()
    {
        if (spawnPoint == null)
            spawnPoint = transform;
    }

    public bool Pickup(GameObject box)
    {
        if (carriedBox != null) return false;
        if (box == null) return false;

        carriedBox = box;

        Rigidbody rb = carriedBox.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col = carriedBox.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        carriedBox.transform.SetParent(spawnPoint, true);
        carriedBox.transform.localPosition = Vector3.zero;
        carriedBox.transform.localRotation = Quaternion.identity;

        return true;
    }

    public void Drop(Vector3 worldPosition)
    {
        if (carriedBox == null) return;

        carriedBox.transform.SetParent(null, true);
        carriedBox.transform.position = worldPosition;

        Rigidbody rb = carriedBox.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col = carriedBox.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        carriedBox = null;
    }
}

