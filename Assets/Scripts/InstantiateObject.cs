using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    public GameObject GO_ToInstantiate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Instantiate()
    {
        Instantiate(GO_ToInstantiate, transform.position,Quaternion.identity); 
    }
}
