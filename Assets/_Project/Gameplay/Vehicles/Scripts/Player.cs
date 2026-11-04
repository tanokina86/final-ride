using Assets.Scripts.Enums;
using FinalRide.Gameplay.Abilities;
using FinalRide.Gameplay.Interfaces;
using FinalRide.Gameplay.Weapons.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalRide.Gameplay.Vehicles
{
    public class Player : Vehicle, IWeaponed
    {
        private Rigidbody2D _playerRigidbody = null;
        private WeaponHandler _weaponHandler;

        private const float MAX_ENERGY = 100f;
        private float _energyRegenRate = 15f;
        private float _currentEnergy;

        [Header("Modular Abilities (ScriptableObjects)")]
        [SerializeField] private List<VehicleAbility> containerAbilities;

        // Поиск нашего универсального оружейного слота среди дочерних объектов тачки игрока
        public WeaponSlot Gun => this.GetComponentInChildren<WeaponSlot>();

        public bool HasAdditionalWeapon { get; } = true;
        public bool IsDashing { get; set; } = false;
        public bool IsShieldActive { get; set; } = false;

        public Rigidbody2D PlayerRigidBody => _playerRigidbody;

        public float CurrentEnergy => _currentEnergy;
        public float MaxEnergy => MAX_ENERGY;

        // Реализация интерфейса (коллекция способностей машины)
        public IEnumerable<Dictionary<string, Action>> Abilities { get; set; }

        [SerializeField]
        public Weapon Weapon
        {
            get => Gun != null ? Gun.WeaponData : null;
            set { if (Gun != null) Gun.ApplyWeaponData(value); }
        }

        private void OnEnable()
        {
            _weaponHandler = new WeaponHandler("Player", transform);
        }

        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody2D>();
            _currentEnergy = MAX_ENERGY;

            SetWeapon();

            // Инициализация твоей расширяемой системы словарей на основе SO списка
            var list = new List<Dictionary<string, Action>>();

            foreach (var soAbility in containerAbilities)
            {
                if (soAbility == null) continue;

                var abilityData = new Dictionary<string, Action>
                {
                    {
                        soAbility.AbilityName,
                        () => TryExecuteAbility(soAbility)
                    }
                };

                list.Add(abilityData);
            }

            Abilities = list;

            StartCoroutine(ShootRoutine(.25f));
        }

        private void Update()
        {
            // Постоянная регенерация энергии в реальном времени
            RegenerateEnergy();

            // Считывание нажатий комбинаций клавиш
            HandleComboInputs();

            // Чит-код для смены пушек на кнопку U
            if (Input.GetKeyDown(KeyCode.U))
            {
                AttackType randomType = (AttackType)UnityEngine.Random.Range(1, 5);
                _weaponHandler.UpgradeWeapon(randomType);
                Debug.Log($"[Cheat] Оружие игрока улучшено до: {randomType}");
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _weaponHandler.ApplyWeaponSet();
            }
        }

        private void FixedUpdate()
        {
            // Если машина находится в состоянии бешеного тарана, обычное ручное управление блокируется
            if (!IsDashing)
            {
                Move();
            }

            RotateGun();
        }

        private void SetWeapon()
        {
            _weaponHandler.CreateWeaponData();
            _weaponHandler.CreateGun(AttackType.Normal, "MachineGun");
        }

        private void RegenerateEnergy()
        {
            if (_currentEnergy < MAX_ENERGY)
            {
                _currentEnergy += _energyRegenRate * Time.deltaTime;
                _currentEnergy = Mathf.Min(_currentEnergy, MAX_ENERGY);
            }
        }

        /// <summary>
        /// Считывание чистых комбинаций кнопок (Комбо), подходящих для ПК и геймпада PS
        /// </summary>
        private void HandleComboInputs()
        {
            // КОМБО 1: Защитное поле (например, кнопка L1 на геймпаде или LeftShift на ПК)
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ExecuteAbilityByName("Shield");
            }

            // КОМБО 2: Таран Battletoads (например, одновременный зажим кнопок Газ + Тормоз (W + S) на ПК)
            if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.E))
            {
                ExecuteAbilityByName("Taran");
            }
        }

        /// <summary>
        /// Ищет способность в словаре по её AbilityName и безопасно запускает при совпадении
        /// </summary>
        private void ExecuteAbilityByName(string targetName)
        {
            if (Abilities == null) return;

            foreach (var abilityDict in Abilities)
            {
                if (abilityDict.ContainsKey(targetName))
                {
                    abilityDict[targetName]?.Invoke();
                    return;
                }
            }
        }

        private void TryExecuteAbility(VehicleAbility ability)
        {
            if (ability == null) return;

            // Сначала строго проверяем и списываем энергию у кузова машины
            if (TryConsumeEnergy(ability.EnergyCost))
            {
                ability.Activate(this);
            }
        }

        public bool TryConsumeEnergy(float amount)
        {
            if (_currentEnergy >= amount)
            {
                _currentEnergy -= amount;
                Debug.Log($"[Energy] Потрачено {amount}. Осталось: {Mathf.RoundToInt(_currentEnergy)}");
                return true;
            }

            Debug.Log($"[Energy] Недостаточно энергии! Требуется: {amount}, есть: {Mathf.RoundToInt(_currentEnergy)}");
            return false;
        }

        public void RechargeEnergy(float amount)
        {
            _currentEnergy = Mathf.Min(_currentEnergy + amount, MAX_ENERGY);
            Debug.Log($"[Energy] Восстановлено {amount}. Текущая энергия: {Mathf.RoundToInt(_currentEnergy)}");
        }

        #region Base Actions

        public override void Move()
        {
            if (_playerRigidbody == null) return;

            Vector2 axisIncrement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * MovementSpeed * Time.fixedDeltaTime;
            _playerRigidbody.MovePosition(_playerRigidbody.position + axisIncrement);
        }

        public void RotateGun()
        {
            if (Gun == null) return;

            int direction = Input.GetKey(KeyCode.Mouse0) ? 1 : Input.GetKey(KeyCode.Mouse1) ? -1 : 0;
            Gun.Rotate(direction);
        }

        public void Shoot()
        {
            if (Input.GetKey(KeyCode.Space) && Gun != null)
            {
                Gun.Shoot();
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

        internal WeaponHandler GetWeaponHandler()
        {
            return _weaponHandler;
        }

        #endregion
    }
}
