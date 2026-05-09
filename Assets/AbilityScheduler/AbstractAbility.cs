using System;
using ISL.StateSystem.Runtime;
using UnityEngine;

namespace AbilityScheduler
{
    public abstract class AbstractAbility : MonoBehaviour
    {

        [SerializeField, PropertyInterface(typeof(IState))]
        private UnityEngine.Object state;
        
        protected float StartTime { get; private set; }
        
        protected float StopTime { get; private set; }
        public bool IsAbilityRunning { get; private set; }

        public GameObject Actor { get; set; }
        
        public IState State => state as IState;

        public event EventHandler AbilityStopped;
        public event EventHandler AbilityStarted;
    
        public virtual void InitAbility() {}

        public virtual void StartAbility()
        {
            StartTime = Time.time;
            IsAbilityRunning = true;
            OnAbilityStarted();
        }

        public virtual void StopAbility()
        {
            StopTime = Time.time;
            IsAbilityRunning = false;
            OnAbilityStopped();
        }

        public abstract void UpdateAbility();


        protected virtual void OnAbilityStopped()
        {
            AbilityStopped?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAbilityStarted()
        {
            AbilityStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}
