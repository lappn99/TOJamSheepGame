using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [FormerlySerializedAs("firstInfectedChance")] [SerializeField] private float latentInfectionChance = 0.05f;
    [FormerlySerializedAs("firstInfectedTryRate")] [SerializeField] private float latentInfectionTryRate = 1.0f;
    [SerializeField] private UnityEvent<Sheep> firstSheepInfected;
    
    private List<Sheep> _sheep = new List<Sheep>();

    private bool _firstSheepInfected;

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        

        StartCoroutine(LatentInfection());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSheep(Sheep sheep)
    {
        _sheep.Add(sheep);
        sheep.onDestroyed.AddListener((() =>
        {
            _sheep.Remove(sheep);
        }));
    }

    private IEnumerator LatentInfection()
    {
        while (true)
        {
            if (Random.value <= latentInfectionChance)
            {
                var infected = _sheep[Random.Range(0, _sheep.Count - 1)];

                if (infected.Infect())
                {
                    if (!_firstSheepInfected)
                    {
                        _firstSheepInfected = true;
                        firstSheepInfected.Invoke(infected);
                    }
                }
                
            }
            
            yield return new WaitForSeconds(latentInfectionTryRate);
        }
    }
}
