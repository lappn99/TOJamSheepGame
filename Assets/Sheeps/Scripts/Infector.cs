using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Infector : MonoBehaviour
{

    [SerializeField] private float infectChance = 0.25f;
    [SerializeField] private float tryInfectRate = 1.0f;

    private List<Sheep> sheepsInRange = new List<Sheep>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(TryInfect), 0.0f, tryInfectRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Sheep sheep))
        {
            if (!sheepsInRange.Contains(sheep))
            {
                sheepsInRange.Add(sheep);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Sheep sheep))
        {
            if (sheepsInRange.Contains(sheep))
            {
                sheepsInRange.Remove(sheep);
            }
        }
    }

    private void TryInfect()
    {
        if (gameObject.activeInHierarchy)
        {
            foreach (var sheep in sheepsInRange)
            {
                if (Random.value <= infectChance)
                {
                    sheep.Infect();
                }
            }
        }
    }
}
