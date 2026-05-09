using System;
using System.Linq;
using ISL.StateSystem.Runtime;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class StateDrawer : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_EDITOR
            
            EditorApplication.hierarchyWindowItemByEntityIdOnGUI += HierarchyWindowItemOnGUI;
#endif
        }

        private void Update()
        {
#if UNITY_EDITOR
            
            EditorApplication.RepaintHierarchyWindow();
#endif
        }

#if UNITY_EDITOR
        private void HierarchyWindowItemOnGUI(EntityId instanceid, Rect selectionRect)
        {
            UnityEngine.Object obj = EditorUtility.EntityIdToObject(instanceid);
            GameObject go = obj as GameObject;

            if (go != null)
            {
                var state = go.GetComponents<MonoBehaviour>().OfType<IState>().FirstOrDefault();
                if (state != null)
                {
                    Color color = Color.red;
                    if (state.Activated)
                    {
                        color = Color.green;
                    
                    }
                    color.a = 0.1f;
                    EditorGUI.DrawRect(selectionRect, color); // Light red background

                    // Set the text color
                    Color contentColor = color;
            
                    // Draw the object name in the new color
                    //EditorGUI.LabelField(selectionRect, go.name, new GUIStyle() { normal = new GUIStyleState() { textColor = Color.white } });
                }
            }
            
        }
#endif


    }
        
}