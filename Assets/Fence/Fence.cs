using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class Fence : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
   [SerializeField]private GameObject fenchPole;

   [SerializeField] private PolygonCollider2D internalCollider;
   [SerializeField] private LayerMask sheepMask;

   public EdgeCollider2D EdgeCollider => edgeCollider;

   public PolygonCollider2D InternalCollider => internalCollider;

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

    public Collider2D[] CheckObjectsInside(ContactFilter2D contactFilter2D)
    {
        InternalCollider.enabled = true;
        InternalCollider.points = edgeCollider.points;
        List<Collider2D> results = new List<Collider2D>();
        var numCollisions = Physics2D.OverlapCollider(InternalCollider, contactFilter2D, results);
        print(numCollisions);
        return results.ToArray();

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
