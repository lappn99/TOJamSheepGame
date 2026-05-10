using AbilityScheduler;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Abilities
{
    public class Breed : ToolAbility
    {
        protected override bool SheepFilter(Sheep sheep)
        {
            return !sheep.IsFed && sheep.IsAdult;
        }

        public override void UpdateAbility()
        {
            base.UpdateAbility();
            if (HasSheep)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    CurrentSheep.Breed();
                    HasSheep = false;
                }
            }
        }
    }
}