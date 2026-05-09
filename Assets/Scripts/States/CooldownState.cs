using System.Collections;
using ISL.StateSystem.Runtime;
using UnityEngine;

namespace DefaultNamespace.States
{
    public class CooldownState: MonoBehaviour, IState
    {


        [SerializeField] private float cooldownTime = 1.0f;


        private void Start()
        {
            Activated = true;
        }

        public void StartCooldown()
        {
            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {
            Activated = false;
            yield return new WaitForSeconds(cooldownTime);
            Activated = true;
        }
        
        
        public bool Activated { get; private set; }
    }
}