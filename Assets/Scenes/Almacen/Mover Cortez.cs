using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class MoverCortez : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Mover()
    {
        var kb = Keyboard.current;
        if (kb == null) return;
        if (kb.wKey.isPressed)
        {
            // (1,0,0) 
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
           
        }

        if (kb.sKey.isPressed)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
           
        }

        if (kb.dKey.isPressed)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
         
        }

        if (kb.aKey.isPressed)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
          
        }



}
    // Update is called once per frame
    void Update()
    {
        Mover();
    }
}
