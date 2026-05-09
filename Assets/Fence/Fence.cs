using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class Fence : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
   [SerializeField]private GameObject fenchPole;

   public EdgeCollider2D EdgeCollider => edgeCollider;

   private List<GameObject> _fences = new List<GameObject>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateFence(Vector3[] points)
    {

        if (points.Length == 0)
        {
            foreach (var fence in _fences)
            {
                fence.SetActive(false);
            }

            edgeCollider.enabled = false;
            return;
        }
        else if (points.Length < 2)
        {
            edgeCollider.enabled = false;
        }
        else
        {
            
            edgeCollider.enabled = true;
        }

        
        for (var i = 0; i < points.Length; i++)
        {
            if (_fences.Count > i)
            {
                GameObject fence = _fences[i];
                fence.transform.localPosition = points[i];
                fence.SetActive(true);
            }
            else
            {
                GameObject newFence = Instantiate(fenchPole, this.transform);
                newFence.transform.localPosition = points[i];
                _fences.Add(newFence);
            }
        }
    }
}
