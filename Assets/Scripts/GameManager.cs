using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [FormerlySerializedAs("firstInfectedChance")] [SerializeField] private float latentInfectionChance = 0.05f;
    [FormerlySerializedAs("firstInfectedTryRate")] [SerializeField] private float latentInfectionTryRate = 1.0f;
    [SerializeField] private UnityEvent<Sheep> firstSheepInfected;
    
    private List<Sheep> _sheep;

    private bool _firstSheepInfected;
    
    void Start()
    {
        _sheep = FindObjectsByType<Sheep>().ToList();
        foreach (var sheep in _sheep)
        {
            sheep.onDestroyed.AddListener((() =>
            {
                _sheep.Remove(sheep);
            }));
            sheep.onSpawnNewSheep.AddListener((arg0 =>
            {
                var sheep = arg0.GetComponent<Sheep>();
                _sheep.Add(sheep);
                sheep.onDestroyed.AddListener((() =>
                {
                    _sheep.Remove(sheep);
                }));
            }));
        }

        StartCoroutine(LatentInfection());
    }

    // Update is called once per frame
    void Update()
    {
        
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
