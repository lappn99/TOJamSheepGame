using System.Linq;
using AbilityScheduler;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Abilities
{
    public abstract class ToolAbility : AbstractAbility
    {
        [SerializeField] private LayerMask sheepMask;
        protected bool HasSheep { get; set; }
        protected Sheep CurrentSheep { get; set; }

        protected virtual bool SheepFilter(Sheep sheep) => true;
        
        
        public override void UpdateAbility()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            var intersects = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity, sheepMask.value);
            
            if (!HasSheep)
            {
                
                if (intersects.Length > 0)
                {
                    var intersectingSheep = intersects.Where((hit2D => hit2D.collider.gameObject.GetComponent<Sheep>()))
                        .Select(hit2D => hit2D.collider.gameObject.GetComponent<Sheep>());
                    intersectingSheep = intersectingSheep.Where(SheepFilter);
                    var availableSheep = intersectingSheep as Sheep[] ?? intersectingSheep.ToArray();
                    if (availableSheep.Any())
                    {
                        CurrentSheep = availableSheep.First();
                        HasSheep = true;
                    }
                }
            }
            else
            {
                if (intersects.Length <= 0)
                {
                    HasSheep = false;
                }
            }
            
        }
    }
}