using AbilityScheduler;
using UnityEngine;

public class Infected : AbstractAbility
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void StartAbility()
    {
        base.StartAbility();
        Animator.SetInteger("State", 2);
    }

    public override void UpdateAbility()
    {
        
    }
}
