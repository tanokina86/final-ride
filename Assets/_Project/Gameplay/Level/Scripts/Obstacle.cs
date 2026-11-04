using UnityEngine;
using FinalRide.Gameplay.Interfaces;

namespace FinalRide.Gameplay.Level
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Obstacle : MonoBehaviour, IDamageable
    {
        [Header("Obstacle Durability")]
        [SerializeField] private float maxHealth = 3f;

        private float _currentHealth;
        private Rigidbody2D _rb;

        public bool IsInvincible { get; set; } = false;

        public float Health => _currentHealth;

        private void Awake()
        {
            _currentHealth = maxHealth;
            _rb = GetComponent<Rigidbody2D>();

            if (_rb != null)
            {
                _rb.bodyType = RigidbodyType2D.Dynamic;
                _rb.mass = 5f;

                // Old Unity properties for damping/drag
                _rb.drag = 2f;
                _rb.angularDrag = 2f;
            }
        }

        /// <summary>
        /// Applies damage to the static obstacle from projectiles or collisions.
        /// </summary>
        /// <param name="damage">The amount of damage directed at the obstacle.</param>
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Explode();
            }
        }

        private void Explode()
        {
            Debug.Log($"[Destroyed] Obstacle {gameObject.name} blown to pieces!");
            Destroy(gameObject);
        }
    }
}