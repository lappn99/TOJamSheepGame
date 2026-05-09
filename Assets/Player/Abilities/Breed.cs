using AbilityScheduler;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Abilities
{
    public class Breed : ToolAbility
    {
        public override void UpdateAbility()
        {
            base.UpdateAbility();
            if (HasSheep)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    CurrentSheep.Breed();
                }
            }
        }
    }
}