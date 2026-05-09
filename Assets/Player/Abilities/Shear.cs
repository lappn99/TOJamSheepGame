using AbilityScheduler;
using Player.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shear : ToolAbility
{
    
    
    

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (HasSheep)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                CurrentSheep.Shear();
            }
        }
        
    }
}
