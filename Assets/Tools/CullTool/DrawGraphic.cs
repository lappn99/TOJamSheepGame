using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.frameCount - _lastSample > sampleFrequency)
        {
            
            _lastSample = Time.frameCount;
            RectTransform canvasRect = GetComponent<RectTransform>();
            bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Mouse.current.position.ReadValue(), null,
                out _localMousePoint);
            if (isInside)
            {
                SetVerticesDirty();

            }
            //print(localPoint);

        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        
        vh.Clear();
        Color32 color = Color.red;


        float halfSize = brushSize / 2.0f;
        Vector2 v1 = new Vector2(_localMousePoint.x - halfSize, _localMousePoint.y - halfSize);
        Vector2 v2 = new Vector2(_localMousePoint.x - halfSize, _localMousePoint.y + halfSize);
        Vector2 v3 = new Vector2(_localMousePoint.x + halfSize, _localMousePoint.y + halfSize);
        Vector2 v4 = new Vector2(_localMousePoint.x + halfSize, _localMousePoint.y - halfSize);

        vh.AddVert(v1, color, new Vector4(0f, 0f));
        vh.AddVert(v2, color, new Vector4(0f, 1f));
        vh.AddVert(v3, color, new Vector4(1f, 1f));
        vh.AddVert(v4, color, new Vector4(1f, 0f));
        
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
        
        /*Rect r = GetPixelAdjustedRect();
        var brushHalfSize = brushSize / 2.0f;
        foreach (var point in _points)
        {

            vh.AddVert(new Vector3(point.x - brushHalfSize , point.y - brushHalfSize, 0.0f), color, new Vector4(0.0f, 0.0f));
            vh.AddVert(new Vector3(point.x + brushHalfSize, point.y - brushHalfSize, 0.0f), color, new Vector4(1.0f, 0.0f));
            vh.AddVert(new Vector3(point.x + brushHalfSize, point.y + brushHalfSize, 0.0f), color, new Vector4(1.0f, 1.0f));
            vh.AddVert(new Vector3(point.x - brushHalfSize, point.y + brushHalfSize,0.0f), color, new Vector4(0.0f, 1.0f));
            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }*/



        //vh.FillMesh();

    }
}
