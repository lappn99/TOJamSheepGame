using AbilityScheduler;
using ISL.StateSystem.Runtime;
using UnityEngine;

public class ActiveAbilityState : MonoBehaviour, IState
{
    [SerializeField] private AbstractAbility ability;

    public bool Activated
    {
        get => ability.IsAbilityRunning;
    }
}
