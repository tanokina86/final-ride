using System.Collections.Generic;
using UnityEngine;

using FinalRide.Gameplay.Interfaces;

namespace FinalRide.Gameplay.Vehicles
{
    public abstract class Vehicle : MonoBehaviour, IVehicle
    {
        [SerializeField]
        private float _speed = 0f;

        [SerializeField]
        private ArmorType _armorType;
        
        private const float MAX_HEALTH = 100f;

        private KeyValuePair<ArmorType, float> _armor = new KeyValuePair<ArmorType, float>(ArmorType.Lite, 100f);

        protected float _currentHealth;

        public float Health { get => _currentHealth; protected set => _currentHealth = value; }

        public float Energy { get; } = 0f;

        public bool IsInvincible { get; set; } = false;

        public bool IsFlying { get; protected set; } = false;

        public float MovementSpeed => _speed;

        public KeyValuePair<ArmorType, float> Armor => _armor;

        protected virtual void Awake()
        {
            _currentHealth = MAX_HEALTH;
        }

        public abstract void Move();

        public void SetArmor(ArmorType type, float durability)
        {
            _armor = new KeyValuePair<ArmorType, float>(type, durability);
        }

        /// <summary>
        /// Универсальный метод получения урона для ЛЮБОЙ машины в игре (Игрок, Враг, Босс)
        /// </summary>
        public virtual void TakeDamage(float damage)
        {
            // Если это игрок и у него включен Щит, полностью игнорируем урон!
            // (Мы безопасно проверяем компонент через паттерн-матчинг C#)
            if (IsInvincible) // Change to invinsable flag instead of class type. EnemyCar could be invinsible too
            {
                Debug.Log("[Combat] Урон полностью заблокирован энергетическим щитом игрока!");
                return;
            }

            _currentHealth -= damage;
            Debug.Log($"[{gameObject.name}] Получен урон: {damage}. Осталось ХП: {Mathf.Max(0, _currentHealth)}");

            // Если здоровье кончилось — запускаем уничтожение
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Базовая логика уничтожения машины
        /// </summary>
        protected virtual void Die()
        {
            Debug.Log($"[Boom] Транспорт {gameObject.name} полностью уничтожен!");

            // Объект удаляется со сцены (для врагов это смерть, для игрока — вызов Game Over)
            Destroy(gameObject);
        }
    }
}

