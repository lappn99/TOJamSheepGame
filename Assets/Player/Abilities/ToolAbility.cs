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
        [SerializeField] private float checkRadius = 0.5f;
        protected bool HasSheep { get; set; }
        protected Sheep CurrentSheep { get; set; }

        protected virtual bool SheepFilter(Sheep sheep) => true;
        
        
        public override void UpdateAbility()
        {
            var mousePosition = Mouse.current.position.ReadValue();

            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            
            var intersects = Physics2D.OverlapCircleAll(worldPosition, checkRadius, sheepMask);
            
            if (!HasSheep)
            {
                if (intersects.Length > 0)
                {
                    var intersectingSheep = intersects.Where((collider => collider.gameObject.GetComponent<Sheep>()))
                        .Select(collider => collider.gameObject.GetComponent<Sheep>());
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