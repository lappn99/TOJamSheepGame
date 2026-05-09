using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DrawGraphic : Graphic
{
    private List<Vector2> _points = new List<Vector2>();
    [SerializeField] private int sampleFrequency = 10;
    private int _lastSample;
    [SerializeField] private float brushSize = 10.0f;


    private Vector2 _localMousePoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lastSample = Time.frameCount;
        _points.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlacePoint()
    {
        RectTransform canvasRect = GetComponent<RectTransform>();
        bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Mouse.current.position.ReadValue(), null,
            out _localMousePoint);
        if (isInside)
        {
            SetVerticesDirty();
            _points.Add(_localMousePoint);
        }
    }

    private (Vector2, Vector2, Vector2, Vector2) QuadVerticesAroundPoint(Vector2 point)
    {
        
        float halfSize = brushSize / 2.0f;
        
        Vector2 v1 = new Vector2(point.x - halfSize, point.y - halfSize); 
        Vector2 v2 = new Vector2(point.x - halfSize, point.y + halfSize); 
        Vector2 v3 = new Vector2(point.x + halfSize, point.y + halfSize); 
        Vector2 v4 = new Vector2(point.x + halfSize, point.y - halfSize);

        return (v1, v2, v3, v4);
    }

    private void AddQuad(ref VertexHelper vh, Color32 color,Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4) => AddQuad(ref vh, color, v1, v2, v3, v4, Quaternion.identity, Vector2.zero);
    private void AddQuad(ref VertexHelper vh, Color32 color, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Quaternion rotation, Vector2 pivot)
    {
        var triangleBase = vh.currentVertCount;

        Vector2 ApplyRotation(Vector2 vertex)
        {
            return (Vector2)(rotation * (vertex - pivot)) + pivot;
        }
        
        vh.AddVert(ApplyRotation(v1), color, new Vector4(0f, 0f));
        vh.AddVert(ApplyRotation(v2), color, new Vector4(0f, 1f));
        vh.AddVert(ApplyRotation(v3), color, new Vector4(1f, 1f));
        vh.AddVert(ApplyRotation(v4), color, new Vector4(1f, 0f));
        
        vh.AddTriangle(triangleBase, triangleBase + 1, triangleBase + 2);
        vh.AddTriangle(triangleBase + 2, triangleBase + 3, triangleBase);
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {

        vh.Clear();
        if (!Application.isPlaying)
        {
            return;
        }

        Color32 color = Color.white;
        var halfWidth = brushSize / 2.0f;
        for (var i = 0; i < _points.Count; i++)
        {
            var currentPoint = _points[i];
            var points = QuadVerticesAroundPoint(currentPoint);
            AddQuad(ref vh, color, points.Item1, points.Item2, points.Item3, points.Item4);
            if (i > 0 )
            {
                var previousPoint = _points[i - 1];
                var line = currentPoint - previousPoint;
                float angle = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                
                var numQuads = Mathf.RoundToInt( line.magnitude / brushSize);
                
                for (int j = 0; j < numQuads; j++)
                {

                    var spawnPoint = previousPoint + line.normalized * (j * brushSize);

                    (Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4) = QuadVerticesAroundPoint(spawnPoint);
                    AddQuad(ref vh, color, v1, v2, v3, v4, rotation, spawnPoint);
                }

            }
           
        }



        //vh.FillMesh();

    }
}
