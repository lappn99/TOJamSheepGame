using System;
using ISL.StateSystem.Runtime;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Sheep : MonoBehaviour
{

    [SerializeField] private UnityEvent mouseEntered;
    [SerializeField] private UnityEvent mouseExited;

    [SerializeField] private VirtualState shearState;
    [SerializeField] private VirtualState breedState;

    [SerializeField, PropertyInterface(typeof(IState))]
    private UnityEngine.Object canBreedState;
    
    [SerializeField] private GameObject babySheepPrefab;

    [SerializeField] private float spawnDistance = 1.0f;

    private Rigidbody2D _rb;
    
    public Rigidbody2D  RigidBody => _rb;

    private IState CanBreedState => canBreedState as IState;
    
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

    public void Shear()
    {
        shearState.activated = true;
    }

    public void Breed()
    {
        if (!CanBreedState.Activated)
        {
            return;
        }

        breedState.activated = true;
        var babySheep = Instantiate(babySheepPrefab);
        var spawnAngle = Random.Range(0.0f, 360.0f);

        Quaternion spawnDirectionQuaternion = Quaternion.AngleAxis(spawnAngle, Vector3.forward);
        Vector3 spawnRotation =spawnDirectionQuaternion * Vector3.up;
        spawnRotation.Normalize();

        babySheep.transform.position = transform.position + spawnRotation * spawnDistance;



    }
}
