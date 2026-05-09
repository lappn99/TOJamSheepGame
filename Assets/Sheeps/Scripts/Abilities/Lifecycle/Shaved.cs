using AbilityScheduler;
using UnityEngine;

namespace Sheeps.Abilities.Lifecycle
{
    public class Shaved : AbstractAbility
    {
        public override void StartAbility()
        {
            base.StartAbility();
            Animator.SetBool("Wool", false);
        }

        public override void UpdateAbility()
        {
            
        }
    }
}