using Assets.Scripts.Enums;
using FinalRide.Gameplay.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace FinalRide.Gameplay.Weapons.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Projectile : MonoBehaviour, IMovable
    {
        private const float MIN_MAGNETISM_DISTANCE = 1.5f;

        private const float MAGNETISM_ANGLE_OFFSET = 60f;
        private const float ROTATION_SMOOTHING_SPEED = 5f;

        private Rigidbody2D _rigidBody;
        private int _pitLayer;
        private int _roadLayer;
        private float _currentLifeTime;

        protected AttackType _attackType;
        protected float _baseDamage = 0f;
        protected float _magnetismRadius = 0f;
        protected bool _hasMicroStun = false;
        protected HashSet<ArmorType> _piercedArmorTypes = new HashSet<ArmorType>();

        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _maxLifeTime = 2f;

        public float MovementSpeed => _moveSpeed;

        /// <summary>
        /// Safely injects compiled runtime combat parameters from the firing weapon slot framework.
        /// </summary>
        public void InitializePayload(float damage, float magnetismRadius, bool hasMicroStun, HashSet<ArmorType> piercedTypes)
        {
            _baseDamage = damage;
            _magnetismRadius = magnetismRadius;
            _hasMicroStun = hasMicroStun;

            if (piercedTypes != null)
            {
                _piercedArmorTypes = new HashSet<ArmorType>(piercedTypes);
            }
        }

        protected virtual void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();

            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
            _rigidBody.simulated = true;

            _currentLifeTime = _maxLifeTime;
            _pitLayer = LayerMask.NameToLayer("Pit");
            _roadLayer = LayerMask.NameToLayer("Road");

        }

        protected virtual void Update()
        {
            _currentLifeTime -= Time.deltaTime;
            if (_currentLifeTime <= 0f)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void FixedUpdate()
        {
            Move();
            HandleStaticMagnetism();
        }

        public virtual void Move()
        {
            if (_rigidBody == null) return;

            _rigidBody.velocity = transform.up * _moveSpeed;
        }

        private void HandleStaticMagnetism()
        {
            if (_magnetismRadius <= 0f || _rigidBody == null) return;

            // Generate an instant 360-degree round spatial layer query scan matching obstacle masks
            Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, _magnetismRadius, LayerMask.GetMask("Enemies"));

            if (targetCollider != null)
            {
                // Calculate the accurate directional shift vector utilizing your custom delta strategy
                Vector2 targetDelta = (Vector2)targetCollider.transform.position - (Vector2)transform.position;
                float minDistance = targetDelta.magnitude;

                if (minDistance <= 0.4f)
                {
                    Destroy(gameObject);
                    return;
                }

                if (minDistance <= MIN_MAGNETISM_DISTANCE)
                {
                    Vector2 hardTargetDirection = targetDelta.normalized;
                    _rigidBody.velocity = hardTargetDirection * _moveSpeed;

                    float trajectoryAngle = Mathf.Atan2(hardTargetDirection.y, hardTargetDirection.x) * Mathf.Rad2Deg - 90f;
                    _rigidBody.transform.rotation = Quaternion.AngleAxis(trajectoryAngle, Vector3.forward);
                    return;
                }
            }

            _rigidBody.velocity = transform.up * _moveSpeed;
        }


        private RaycastHit2D EvaluateValidHit(RaycastHit2D center, RaycastHit2D left, RaycastHit2D right)
        {
            if (center.collider != null && center.collider.gameObject.layer != _pitLayer && center.collider.gameObject.layer != _roadLayer) return center;
            if (left.collider != null && left.collider.gameObject.layer != _pitLayer && left.collider.gameObject.layer != _roadLayer) return left;
            if (right.collider != null && right.collider.gameObject.layer != _pitLayer && right.collider.gameObject.layer != _roadLayer) return right;

            return default;
        }

        /// Evaluates structural trigger entries to process asset damage routing or handle immediate physics destruction.
        /// </summary>
        /// <param name="collision">The active external Collider2D component boundary node.</param>
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null) return;

            int collisionLayer = collision.gameObject.layer;

            // Safely ignore physics calculations if the projectile triggers with the empty Pit or standard Road tracks
            if (collisionLayer == _pitLayer || collisionLayer == _roadLayer)
            {
                return;
            }

            // Note: Put your explicit damage payload calculations and particle execution methods right here

            Destroy(gameObject);
        }


        private void OnDestroy()
        {
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}