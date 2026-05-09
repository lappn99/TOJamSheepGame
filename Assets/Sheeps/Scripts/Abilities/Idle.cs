using System.Collections;
using AbilityScheduler;
using DG.Tweening;
using ISL.StateSystem.Runtime;
using UnityEngine;

namespace Sheeps.Abilities
{
    public class Idle : AbstractAbility, IState
    {

        [SerializeField, Range(0.1f, 5.0f)] private float minMoveTime = 1.0f;
        [SerializeField, Range(1.0f, 10.0f)] private float maxMoveTime = 1.0f;
        [SerializeField] private float minMoveDistance = 1.0f;
        [SerializeField] private float maxMoveDistance = 5.0f;
        [SerializeField] private float moveSpeed = 1.0f;
        [SerializeField] private Ease movementEase = Ease.Linear;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private Transform castOrigin;

        private Vector3 _moveDirection;

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rb;
        private BoxCollider2D _playerBox;

        public override void InitAbility()
        {
            base.InitAbility();
            _spriteRenderer = Actor.GetComponent<SpriteRenderer>();
            _rb = Actor.GetComponent<Rigidbody2D>();
            _playerBox = Actor.GetComponent<BoxCollider2D>();
        }

        public override void StartAbility()
        {
            base.StartAbility();
            StartCoroutine(nameof(MoveSheep));
        }

        public override void UpdateAbility()
        {
            
        }

        private IEnumerator MoveSheep()
        {

            while (IsAbilityRunning)
            {
                float waitTime = UnityEngine.Random.Range(minMoveTime, maxMoveTime);

                yield return new WaitForSeconds(waitTime);
                float moveAngle = Random.Range(0.0f, 360.0f);


                var moveDistance = Random.Range(minMoveDistance, maxMoveDistance);
                
                var moveDirection =  Quaternion.AngleAxis(moveAngle, Vector3.forward) * Vector3.right;
                
                while (Physics2D.BoxCast(castOrigin.position, _playerBox.size * Actor.transform.localScale, 0.0f, moveDirection, distance: moveDistance,
                           layerMask: obstacleMask.value))
                {
                    yield return null;
                    moveAngle = Random.Range(0.0f, 360.0f);
                    moveDirection = Quaternion.AngleAxis(moveAngle, Vector3.forward) * Vector3.right;

                } 
                
                _moveDirection = moveDirection.normalized;
                var targetPosition = transform.position + _moveDirection * moveDistance;
                if (Vector3.Dot(transform.right, _moveDirection) < 0)
                {
                    Actor.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    Actor.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                
                
                yield return _rb.DOMove(targetPosition, moveDistance / moveSpeed).SetEase(movementEase).WaitForCompletion();


            }

        }

        public bool Activated
        {
            get => true;
        }
    }
}