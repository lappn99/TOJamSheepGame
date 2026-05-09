using ISL.StateSystem.Runtime;
using UnityEngine;

public class VirtualState : MonoBehaviour, IState
{

    [SerializeField] public bool activated;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Activated
    {
        get => activated;
        set => activated = value;
    }
}
