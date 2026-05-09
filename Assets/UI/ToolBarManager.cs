using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public enum Tool
{
    Grab,
    Shear,
    Breed,
    Fence
}
public class ToolBarManager : MonoBehaviour
{
    public static UnityAction<Tool> OnToolChange;

    private List<Button> buttons = new();

    public Tool CurrentTool { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            buttons.Add(transform.GetChild(i).GetComponent<Button>());
        }
        ChangeTool(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTool(int ToolIndex)
    {
        CurrentTool = (Tool)ToolIndex;
        OnToolChange.Invoke((Tool)ToolIndex);
        foreach (Button button in buttons)
            button.GetComponent<Animator>().SetBool("Current",false);
        buttons[ToolIndex].GetComponent<Animator>().SetBool("Current", true);
    }

}
