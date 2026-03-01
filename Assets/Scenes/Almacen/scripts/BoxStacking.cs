using UnityEngine;

public class BoxStacking:MonoBehaviour
{
   
    [SerializeField] public int numberOfBoxes = 0;
    [SerializeField] public int maxNumberOfBoxes = 5;
    [SerializeField] public float positionOffset = 1f;
    [SerializeField] public GameObject boxPrefab;
    int spawnedBoxes = 0;
    private float initialVerticalPosition = 0.71f;
    
    void AddBox()
    {
        numberOfBoxes++;
        
    }
    void Start()
    {
       
    }
    void Update()
    {
        while (spawnedBoxes < numberOfBoxes && spawnedBoxes < maxNumberOfBoxes)
        {
            Vector3 position = new Vector3(
                transform.position.x,
                (transform.position.y +initialVerticalPosition ) + positionOffset * spawnedBoxes,
                transform.position.z
            );

            Instantiate(boxPrefab, position, Quaternion.identity);
            spawnedBoxes++;
        }
        
    }
}
