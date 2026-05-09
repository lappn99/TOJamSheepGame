using System;
using System.Collections;
using AbilityScheduler;
using DG.Tweening;
using ISL.StateSystem.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sheeps.Abilities
{
    public class Move : AbstractAbility, IState
    {
        [SerializeField] private bool allowMovement;

        [SerializeField, Range(0.1f, 5.0f)] private float minMoveTime = 1.0f;
        [SerializeField] private float baseMoveChance = 0.05f;
        [SerializeField] private float minMoveDistance = 1.0f;
        [SerializeField] private float maxMoveDistance = 5.0f;
        [SerializeField] private float moveSpeed = 1.0f;
        [SerializeField] private Ease movementEase = Ease.Linear;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private Transform castOrigin;

        private Tween _moveTween;

        private bool _move;

        private Vector3 _moveDirection;

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rb;
        private BoxCollider2D _playerBox;

        public bool AllowMovement
        {
            get => allowMovement;
            set => allowMovement = value;
        }

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

        private void Update()
        {
            if (!IsAbilityRunning && (Time.time - StopTime) > minMoveTime)
            {
                if (Random.value <= baseMoveChance)
                {
                    _move = true;
                }
            }
        }

        public override void UpdateAbility()
        {
            
        }

        private IEnumerator MoveSheep()
        {
//            print("Move");
            float moveAngle = Random.Range(0.0f, 360.0f);


            var moveDistance = Random.Range(minMoveDistance, maxMoveDistance);
                
            var moveDirection =  Quaternion.AngleAxis(moveAngle, Vector3.forward) * Vector3.right;
                
            while (Physics2D.BoxCast(castOrigin.position, _playerBox.size , 0.0f, moveDirection, distance: moveDistance,
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

            
            _moveTween = _rb.DOMove(targetPosition, moveDistance / moveSpeed).SetEase(movementEase);
            
            _moveTween.onComplete += () =>
            {
                _move = false;
            };

        }

        public override void StopAbility()
        {
            
            base.StopAbility();
            _move = false;
            print("Stop");
            if (_moveTween is { active: true })
            {
                print("Stop movement");
                _moveTween.Kill();
            }
        }

        public void SetCanMove(bool move)
        {
            allowMovement = move;
        }

        public bool Activated
        {
            get => allowMovement && _move;
        }
    }
}