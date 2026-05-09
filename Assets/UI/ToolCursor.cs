using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;




public class ToolCursor : MonoBehaviour
{
    [SerializeField] private GameObject GrabTool, ShearTool, BreedTool, FenceTool;
    [SerializeField] private Tool tool;
    [SerializeField] private Image Grab_Img;
    [SerializeField] private Sprite Grab_Spr, GrabReleased_Spr;

    public Tool currentTool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTool = Tool.Grab;
        ChangeTool(currentTool);
        Cursor.visible = false;
    }

    void OnEnable()
    {
        ToolBarManager.OnToolChange += ChangeTool;

    }

    void OnDisable()
    {
        ToolBarManager.OnToolChange -= ChangeTool;
    }

    public void ChangeTool(Tool tool)
    {
        GrabTool.SetActive(false);
        ShearTool.SetActive(false);
        BreedTool.SetActive(false);
        FenceTool.SetActive(false);

        switch (tool)
        {
            case Tool.Grab:
                GrabTool.SetActive(true); break;
            case Tool.Shear:
                ShearTool.SetActive(true); break;
            case Tool.Breed:
                BreedTool.SetActive(true); break;
            case Tool.Fence:
                FenceTool.SetActive(true); break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        transform.position = mousePos;

        if (currentTool == Tool.Grab)
        {
            Grab_Img.sprite = Mouse.current.leftButton.isPressed ? Grab_Spr : GrabReleased_Spr;
        }

    }
}
