using ISL.StateSystem.Runtime;
using UnityEngine;

public class ToolState : MonoBehaviour, IState
{
    [SerializeField] private ToolBarManager toolBar;

    [SerializeField] private Tool tool;
    
    

    public bool Activated
    {
        get => toolBar.CurrentTool == tool;
    }
}
