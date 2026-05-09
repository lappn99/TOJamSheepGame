using AbilityScheduler;
using ISL.StateSystem.Runtime;
using UnityEngine;

namespace Sheeps.Abilities
{
    public class Idle : AbstractAbility, IState
    {
        public override void UpdateAbility()
        {
        }


        public bool Activated
        {
            get => true;
        }
    }
}