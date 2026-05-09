using System;
using UnityEngine;
using UnityEngine.Events;

public class Sheep : MonoBehaviour
{

    [SerializeField] private UnityEvent mouseEntered;
    [SerializeField] private UnityEvent mouseExited;

    private Rigidbody2D _rb;
    
    public Rigidbody2D  RigidBody => _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        mouseEntered.Invoke();
    }

    private void OnMouseExit()
    {
        mouseExited.Invoke();
    }
}
