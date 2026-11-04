using Assets.Scripts;
using Assets.Scripts.Enums;
using FinalRide.Gameplay.Interfaces;
using FinalRide.Gameplay.Vehicles;
using FinalRide.Gameplay.Weapons.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalRide.Gameplay.Vehicles
{
    public class EnemyCar : Vehicle, IEnemy
    {
        private int _id = 0;
        public int Id => _id;

        private string _name = null;
        private string _description = null;
        private Rigidbody2D _rigidbody = null;
        private GameObject _player = null;

        public string Name { get => string.IsNullOrEmpty(_name) ? name : _name; set => _name = value; }
        public string Discription => _description;

        public IEnumerable<Dictionary<string, Action>> Abilities { get; set; }

        private WeaponHandler _weaponHandler;

        public WeaponSlot ActiveWeaponSlot => this.GetComponentInChildren<WeaponSlot>();

        // Возвращаем свойство для выполнения контракта интерфейса IWeaponed
        // Оно будет просто считывать текущие данные (SO) из нашего активного оружейного слота
        public Weapon Weapon
        {
            get => ActiveWeaponSlot != null ? ActiveWeaponSlot.WeaponData : null;
            set
            {
                if (ActiveWeaponSlot != null) ActiveWeaponSlot.ApplyWeaponData(value);
            }
        }
        public bool HasAdditionalWeapon => false;

        [Header("Weapon Configurations")]
        [SerializeField]
        private AttackType initialAttackType = AttackType.Normal;

        public EnemyCar()
        {
            _id += 1;
        }

        public override void Move()
        {
            if (_player == null || _rigidbody == null) return;

            Vector2 playerPosition = _player.transform.position;

            // Плавное движение врага в сторону игрока
            _rigidbody.position += (playerPosition - _rigidbody.position) * MovementSpeed * Time.fixedDeltaTime;
        }

        public void Describe(string text)
        {
            _description = text;
        }

        public void Shoot(float baseDamage)
        {
            // Метод из интерфейса, можно использовать для спец-атак или оставить пустым
            Shoot();
        }

        public void Shoot()
        {
            // Вызываем честный физический выстрел из нашего нового оружейного слота
            if (ActiveWeaponSlot != null)
            {
                ActiveWeaponSlot.Shoot();
            }
        }

        private IEnumerator ShootRoutine(float speed = .5f)
        {
            while (Health > 0)
            {
                yield return new WaitForSeconds(speed);
                Shoot();
            }
        }

        private void OnEnable()
        {
            Debug.Log($"Enemy - {Name} is now Enable. Has capability to fly: {IsFlying}");

            // Инициализируем обработчик оружия, передавая сокет (в данном случае саму трансформ-точку врага)
            _weaponHandler = new WeaponHandler("Enemy", transform);
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            var playerComponent = FindObjectOfType<Player>();
            if (playerComponent != null)
            {
                _player = playerComponent.gameObject;
            }

            // ТЕПЕРЬ ЗДЕСЬ ИСПОЛЬЗУЕТСЯ НАСТРОЙКА ИЗ ИНСПЕКТОРА:
            _weaponHandler.UpgradeWeapon(initialAttackType);

            StartCoroutine(ShootRoutine(1f));
        }


        private void FixedUpdate()
        {
            Move();
        }

        private void Update()
        {
            if (_player == null || ActiveWeaponSlot == null) return;

            // Расчет угла для слежения оружия за игроком
            Vector2 targetDirection = (_player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;

            // Вращаем ствол в сторону игрока
            ActiveWeaponSlot.Rotate(angle);
        }
    }
}
