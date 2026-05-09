using AbilityScheduler;
using UnityEngine;

public class Baby : AbstractAbility
{
    public override void StartAbility()
    {
        base.StartAbility();
        Animator.SetInteger("State", 0);
    }

    public override void UpdateAbility()
    {
    }
}
