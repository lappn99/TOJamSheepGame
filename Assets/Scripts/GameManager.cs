using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [FormerlySerializedAs("firstInfectedChance")] [SerializeField] private float latentInfectionChance = 0.05f;
    [FormerlySerializedAs("firstInfectedTryRate")] [SerializeField] private float latentInfectionTryRate = 1.0f;
    [SerializeField] private UnityEvent<Sheep> firstSheepInfected;
    [SerializeField] private Volume PPVolume;
    
    private List<Sheep> _sheep = new List<Sheep>();

    private bool _firstSheepInfected;

    private static GameManager _instance;

    public int infectedCount;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        PPVolume.weight = 0f;

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnEnable()
    {
        Goat.FirstInfected += StartLatentInfection;

    }

    private void OnDisable()
    {
        Goat.FirstInfected -= StartLatentInfection;

    }

    void StartLatentInfection()
    {
        Sheep first = _sheep[Random.Range(0,_sheep.Count)];
        first.Infect();
        firstSheepInfected.Invoke(first);
        _firstSheepInfected = true;

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

    public void UpdateInfected(int add)
    {
        infectedCount += add;
        if(infectedCount <= 0)
        {
            PPVolume.weight = 0f;
        }
        else
        {
            PPVolume.weight = 1f;
        }
    }

    private IEnumerator LatentInfection()
    {
        while (true)
        {
            
            foreach (var sheep in _sheep)
            {
                if (Random.value <= latentInfectionChance)
                {
                    if (sheep.Infect())
                    {
                        PPVolume.weight = 1.0f;

                        if (!_firstSheepInfected)
                        {
                            _firstSheepInfected = true;
                            firstSheepInfected.Invoke(sheep);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(latentInfectionTryRate);
        }
    }
}
