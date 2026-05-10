using System;
using System.Collections;
using DG.Tweening;
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
    [SerializeField] private VirtualState infectState;
    [SerializeField] private VirtualState insideFenceState;

    [SerializeField, PropertyInterface(typeof(IState))]
    private UnityEngine.Object canBreedState;
    [SerializeField, PropertyInterface(typeof(IState))] private UnityEngine.Object canBeInfectedState;

    [SerializeField, PropertyInterface(typeof(IState))]
    private UnityEngine.Object canBeShavedState;

    [SerializeField, PropertyInterface(typeof(IState))]
    private UnityEngine.Object isAdultState;
    
    [SerializeField] private GameObject babySheepPrefab;

    [SerializeField] private float spawnDistance = 1.0f;

    [SerializeField] public UnityEvent<GameObject> onSpawnNewSheep;
    [SerializeField] public UnityEvent onShave;
    [SerializeField] public UnityEvent onInfect;
    [SerializeField] public UnityEvent onDestroyed;

    private Rigidbody2D _rb;
    
    public Rigidbody2D  RigidBody => _rb;

    private IState CanBreedState => canBreedState as IState;
    
    private IState CanBeInfectedState => canBeInfectedState as IState;
    private IState CanBeShavedState => canBeShavedState as IState;
    
    private IState IsAdultState => isAdultState as IState;

    public bool IsShaved => shearState.Activated;
    public bool IsFed => breedState.Activated;
    
    
    public bool IsAdult => IsAdultState.Activated;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        GameManager.Instance.AddSheep(this);
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
        if (CanBeShavedState.Activated)
        {
            
            shearState.activated = true;
            onShave.Invoke();
        }
    }

    public void Breed()
    {
        if (!CanBreedState.Activated)
        {
            GetComponent<Animator>().SetTrigger("NoBreeding");
            return;
        }
        
        
        breedState.activated = true;
        var babySheep = Instantiate(babySheepPrefab);
        var spawnAngle = Random.Range(0.0f, 360.0f);

        Quaternion spawnDirectionQuaternion = Quaternion.AngleAxis(spawnAngle, Vector3.forward);
        Vector3 spawnRotation =spawnDirectionQuaternion * Vector3.up;
        spawnRotation.Normalize();

        babySheep.transform.position = transform.position + spawnRotation * spawnDistance;
        onSpawnNewSheep.Invoke(babySheep);


    }

    public void KillSheep()
    {
        GetComponent<Animator>().SetTrigger("Kill");
    }

    public void DestroySheep()
    {
        Destroy(gameObject);
    }

    public void InsideFence()
    {
        insideFenceState.Activated = true;
    }

    private void OnDestroy()
    {
        if(GetComponent<Animator>().GetInteger("State") == 2)
            GameManager.Instance.UpdateInfected(-1);
        onDestroyed.Invoke();
        DOTween.Kill(this);
    }

    public bool Infect()
    {
        if (CanBeInfectedState.Activated && !infectState.activated)
        {
            infectState.activated = true;
            onInfect.Invoke();
            return true;
        }

        return false;
    }

}
