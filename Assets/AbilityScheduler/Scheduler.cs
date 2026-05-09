using System;
using System.Collections.Generic;
using System.Linq;
using ISL.StateSystem.Runtime;
using UnityEditor;
using UnityEngine;

namespace AbilityScheduler
{
    public class Scheduler : MonoBehaviour
    {

        [SerializeField] private List<AbstractAbility> rootAbilites;
        [SerializeField] private GameObject actor;
        [SerializeField] private int animatorLayer;
        [SerializeField] private Animator animator;

        private AbstractAbility _currentAbility = null;
        
        public AbstractAbility LastAbility { get; private set; }
        public AbstractAbility CurrentAbility => _currentAbility;
    
            // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            IterateAbilities((InitAbility));
        }

        private void InitAbility(AbstractAbility obj)
        {
            
            obj.Actor = actor;
            obj.Animator = animator;
            obj.AnimatorLayer = animatorLayer;
            obj.InitAbility();
        }

        // Update is called once per frame
        void Update()
        {
            CheckAbilitiesState();

            if (_currentAbility && _currentAbility.IsAbilityRunning)
            {
                _currentAbility.UpdateAbility();
            }
            
        }

        private void CheckAbilitiesState()
        {
            
            AbstractAbility nextAbility = _currentAbility;
            IterateAbilities((ProcessAbility));
            
            void ProcessAbility(AbstractAbility current)
            {
               

                if (!current.State.Activated)
                {
                    return;
                }

                nextAbility = current;
            }

            if (nextAbility != _currentAbility)
            {
                if (_currentAbility is not null)
                {
                    _currentAbility.StopAbility();
                }

                _currentAbility = nextAbility;
                _currentAbility.StartAbility();
                _currentAbility.AbilityStopped += CurrentAbilityOnAbilityStopped;
            }
        }

        private void CurrentAbilityOnAbilityStopped(object sender, EventArgs e)
        {
            if (sender is AbstractAbility abstractAbility)
            {
                LastAbility = abstractAbility;
                abstractAbility.AbilityStopped -= CurrentAbilityOnAbilityStopped;
                
            }
        }


        private void IterateAbilities(Action<AbstractAbility> processAbility)
        {
            foreach (var root in rootAbilites)
            {
                Stack<AbstractAbility> abilityStack = new Stack<AbstractAbility>();
                abilityStack.Push(root);
                while (abilityStack.Count > 0)
                {
                    AbstractAbility current = abilityStack.Pop();
                    processAbility.Invoke(current);
                    for (int i = 0; i < current.transform.childCount; i++)
                    {
                        var child = current.transform.GetChild(i);
                        if (child.TryGetComponent(out AbstractAbility childAbility))
                        {
                            abilityStack.Push(childAbility);
                        }
                    }
                }
            }
        }
        
    }
}
