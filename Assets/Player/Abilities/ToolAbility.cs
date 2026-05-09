using AbilityScheduler;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Abilities
{
    public class ToolAbility : AbstractAbility
    {
        [SerializeField] private LayerMask sheepMask;
        protected bool HasSheep { get; set; }
        protected Sheep CurrentSheep { get; set; }
        
        
        public override void UpdateAbility()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            var intersects = Physics2D.GetRayIntersection(ray, Mathf.Infinity, sheepMask.value);
            if (!HasSheep)
            {
            
                if (intersects.collider)
                {
                    if (intersects.collider.gameObject.TryGetComponent(out Sheep sheep))
                    {
                        CurrentSheep = sheep;
                        HasSheep = true;
                    }
                }
            }
            else
            {
                if (!intersects)
                {
                    HasSheep = false;
                }
            }
            
        }
    }
}