using UnityEngine;

public class DropZone : MonoBehaviour
{
    [SerializeField] private int maxStackHeight = 5;
    [SerializeField] private float boxHeight = 0.6f;
    [SerializeField] private Vector3 stackStep = new Vector3(2.5f, 0f, 0f);

    private int reservedCount = 0;
    private int totalPlaced = 0;

    public int TotalPlaced => totalPlaced;

    public Vector3 ReserveNextSpot()
    {
        int stackIndex = reservedCount / maxStackHeight;
        int level = reservedCount % maxStackHeight;

        reservedCount++;

        Vector3 basePos = transform.position + stackStep * stackIndex;
        return basePos + Vector3.up * (boxHeight * level);
    }

    public void RegisterPlaced()
    {
        totalPlaced++;
    }
}