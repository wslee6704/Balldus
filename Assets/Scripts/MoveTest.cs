using UnityEngine;

public class MoveTest : MonoBehaviour
{
    float timer=0f;
    
    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 1f){
            timer = 0f;
            this.gameObject.transform.position += Vector3.right;
            Debug.Log(this.gameObject.transform.position);
        }
  
    }
}
