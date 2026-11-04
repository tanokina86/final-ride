using Assets.Scripts.Enums;
using FinalRide.Gameplay.Interfaces;
using UnityEngine;

namespace FinalRide.Gameplay.Weapons
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Projectile : MonoBehaviour, IMovable
    {
        private Rigidbody2D _rigidBody;

        protected AttackType _attackType;
        protected float _baseDamage = 0f;

        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _maxLifeTime = 2f; // Время жизни пули в секундах, чтобы не засорять память
        private float _currentLifeTime;

        public float MovementSpeed => _moveSpeed;

        protected virtual void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();

            // Настраиваем физику для стабильных 2D столкновений
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
            _rigidBody.simulated = true;

            _currentLifeTime = _maxLifeTime;
        }

        protected virtual void Update()
        {
            // Честный таймер жизни снаряда на основе дельты времени
            _currentLifeTime -= Time.deltaTime;
            if (_currentLifeTime <= 0f)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void FixedUpdate()
        {
            // Физическое движение должно выполняться строго в FixedUpdate
            Move();
        }

        public virtual void Move()
        {
            if (_rigidBody == null) return;

            // Двигаем пулю вперед по вектору её локального разворота (ось Y/Up для 2D)
            _rigidBody.velocity = transform.up * _moveSpeed;
        }

        private void OnDestroy()
        {
            // Здесь будет анимация взрыва/попадания
        }

        private void OnBecameInvisible()
        {
            // Уничтожаем пулю, если она полностью улетела за границы экрана камеры
            Destroy(gameObject);
        }
    }
}
