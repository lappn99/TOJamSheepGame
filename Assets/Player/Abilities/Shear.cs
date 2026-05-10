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

        if (HasSheep)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                CurrentSheep.Shear();
                HasSheep = false;
            }
        }
        
    }
}
