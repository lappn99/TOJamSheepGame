using AbilityScheduler;
using UnityEngine;

public class Adult : AbstractAbility
{
    public override void StartAbility()
    {
        base.StartAbility();
        Animator.SetInteger("State", 1);
        Animator.SetBool("Wool", true);
    }

    public override void UpdateAbility()
    {
        
    }
}
