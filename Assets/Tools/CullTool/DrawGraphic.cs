using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DrawGraphic : Graphic
{

    private struct GraphicPoint
    {
        public Vector2 MousePosition { get; set; }
        public Vector2 RectPosition { get; set; }
    }
    
    private const int MAX_POINTS = 4;
    
    private List<GraphicPoint> _points = new List<GraphicPoint>();
    [SerializeField] private int sampleFrequency = 10;
    [SerializeField] private float snapDistance = 10f;
    [SerializeField] private float brushSize = 10.0f;
    [SerializeField] private float fadeTime = 3.0f;
    [SerializeField] private EdgeCollider2D fenceCollider;
    [SerializeField] private LayerMask mask;
    

    private Vector2 _localRectPosition;
    private int _lastSample;
    private CanvasGroup _canvasGroup;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lastSample = Time.frameCount;
        _points.Clear();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlacePoint()
    {
        RectTransform canvasRect = GetComponent<RectTransform>();
        var mousePoint = Mouse.current.position.ReadValue();
        
        bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePoint, null,
            out _localRectPosition);
        GraphicPoint point = new GraphicPoint
        {
            MousePosition = mousePoint,
            RectPosition = _localRectPosition
            
        };
        if (isInside)
        {
            if (_points.Count >= MAX_POINTS)
            {
                if (Vector2.Distance(_localRectPosition, _points[0].RectPosition) <= snapDistance)
                {
                    _points.Add(_points[0]);
                    
                    
                    UpdateFencePoints();
                    ContactFilter2D contactFilter2D = new ContactFilter2D();
                    contactFilter2D.layerMask = mask;
                    contactFilter2D.useLayerMask = true;

                    List<Collider2D> overlaps = new List<Collider2D>();
                    Physics2D.OverlapArea(fenceCollider.bounds.min, fenceCollider.bounds.max, contactFilter2D,
                        overlaps);
                    _canvasGroup.DOFade(0.0f, fadeTime).onComplete += () =>
                    {
                        _points.Clear();
                        SetVerticesDirty();
                        _canvasGroup.alpha = 1.0f;
                        UpdateFencePoints();
                        foreach (var overlap in overlaps)
                        {
                            Destroy(overlap.gameObject);
                        }
                        
                        
                    };
                }
                else
                {
                    _points.Clear();
                    UpdateFencePoints();
                }
            }
            else
            {
                _points.Add(point);
                UpdateFencePoints();
               
            }
            
            
            SetVerticesDirty();
        }
    }

    private void UpdateFencePoints()
    {   
        fenceCollider.points = _points.Select((graphicPoint =>
        {
                    
            var worldPosition =  fenceCollider.transform.InverseTransformPoint(
                Camera.main.ScreenToWorldPoint(graphicPoint.MousePosition));
            return new Vector2(worldPosition.x, worldPosition.y);
        })).ToArray();
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
            var points = QuadVerticesAroundPoint(currentPoint.RectPosition);
            AddQuad(ref vh, color, points.Item1, points.Item2, points.Item3, points.Item4);
            if (i > 0 )
            {
                var previousPoint = _points[i - 1];
                var line = currentPoint.RectPosition - previousPoint.RectPosition;
                float angle = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                
                var numQuads = Mathf.RoundToInt( line.magnitude / brushSize);
                
                for (int j = 0; j < numQuads; j++)
                {

                    var spawnPoint = previousPoint .RectPosition+ line.normalized * (j * brushSize);

                    (Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4) = QuadVerticesAroundPoint(spawnPoint);
                    AddQuad(ref vh, color, v1, v2, v3, v4, rotation, spawnPoint);
                }

            }
           
        }



        //vh.FillMesh();

    }
}
