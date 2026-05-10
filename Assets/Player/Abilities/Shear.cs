using AbilityScheduler;
using Player.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shear : ToolAbility
{
    protected override bool SheepFilter(Sheep sheep)
    {
        return !sheep.IsShaved && sheep.IsAdult;

    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (HasSheep && IsAbilityRunning)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                CurrentSheep.Shear();
                HasSheep = false;
            }
        }
        
    }
    
    
}
