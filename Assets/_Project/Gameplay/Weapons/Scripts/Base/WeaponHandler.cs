using Assets.Scripts;
using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinalRide.Gameplay.Weapons.Base
{
    public class WeaponHandler
    {
        private GameObject _weaponObject = null;
        private WeaponSlot _weaponSlot = null;

        private int currentStatusIndex = 0;
        private readonly string[] _upgradeStatus = new string[WeaponData.MAX_UPGRADE_NUMBER];

        private bool _isBuildLocked = false;
        private readonly Dictionary<AttackType, string> _tokenRegistry = new Dictionary<AttackType, string>
        {
            { AttackType.Normal, "nr" },
            { AttackType.Canon, "cn" },
            { AttackType.Splitted, "sp" },
            { AttackType.Electro, "el" },
            { AttackType.Ignito, "ig" }
        };


        public string[] UpgradeStatus => _upgradeStatus;

        private delegate void WeaponStateHandler(AttackType attackType);
        private event WeaponStateHandler OnWeaponUpgraded;

        // Конструктор инициализирует объект пушки и сразу привязывает к машине, если передан родитель
        public WeaponHandler(string owner, Transform parent = null)
        {
            string weaponName = owner == "Player" ? "PlayerWeapon" : "Weapon";
            _weaponObject = new GameObject(weaponName);

            // Добавляем наш универсальный слот, который теперь рулит спрайтами и физикой
            _weaponSlot = _weaponObject.AddComponent<WeaponSlot>();

            // CONNECTING THE DOTS: Передаем ссылку на себя для работы WeaponStatsBuilder внутри слота
            _weaponSlot.InitializeSlot(this);

            if (parent != null)
            {
                SetGunTo(parent);
            }
        }


        /// <summary>
        /// Registers weapon upgrades into the sequential configuration stack, handling custom terminal slot rotation algorithms.
        /// </summary>
        /// <param name="attackType">The explicit target attack type profile token retrieved from the map track pickup.</param>
        public void UpgradeWeapon(AttackType attackType)
        {
            if (_isBuildLocked || !_tokenRegistry.TryGetValue(attackType, out string shortName))
            {
                return;
            }

            int lastSlotIndex = WeaponData.MAX_UPGRADE_NUMBER - 1;

            if (currentStatusIndex >= WeaponData.MAX_UPGRADE_NUMBER)
            {
                _upgradeStatus[lastSlotIndex] = shortName;
                Debug.Log($"[Inventory] Overwriting last slot. Current stack ID: [{string.Join("-", _upgradeStatus)}]");
            }
            else
            {
                _upgradeStatus[currentStatusIndex] = shortName;
                currentStatusIndex++;
                Debug.Log($"[Inventory] Appending weapon. Current stack ID: [{string.Join("-", _upgradeStatus)}]");
            }

            ApplyWeaponFromResources(shortName);
            OnWeaponUpgraded?.Invoke(attackType);
        }

        public void CreateGun(AttackType attackType, string gunName = "")
        {
            if (!_tokenRegistry.TryGetValue(attackType, out string shortName))
            {
                shortName = "nr";
            }

            // Безопасно подтягиваем или добавляем WeaponSlot
            if (_weaponSlot == null)
            {
                _weaponSlot = _weaponObject.GetComponent<WeaponSlot>();
                if (_weaponSlot == null)
                {
                    _weaponSlot = _weaponObject.AddComponent<WeaponSlot>();
                }
            }

            ApplyWeaponFromResources(shortName);
        }

        private void ApplyWeaponFromResources(string shortName)
        {
            // Находим сгенерированный ассет в папке Assets/_Project/Resources/SO/Weapons/
            Weapon weaponData = Resources.Load<Weapon>($"SO/Weapons/{shortName}");

            if (weaponData != null)
            {
                _weaponSlot.ApplyWeaponData(weaponData);

                // Настраиваем сортировку спрайта, чтобы пушка ложилась поверх кузова машины (+1 к слою машины)
                SpriteRenderer vehicleRenderer = _weaponObject.GetComponentInParent<SpriteRenderer>();
                SpriteRenderer gunRenderer = _weaponObject.GetComponent<SpriteRenderer>();

                if (vehicleRenderer != null && gunRenderer != null)
                {
                    gunRenderer.sortingOrder = vehicleRenderer.sortingOrder + 1;
                }

                Debug.Log($"[WeaponHandler] Пушка успешно инициализирована из паспорта: SO/Weapons/{shortName}");
            }
            else
            {
                Debug.LogWarning($"[WeaponHandler] Не найден ассет Weapon ScriptableObject по пути: Resources/SO/Weapons/{shortName}");
            }
        }

        public void SetGunTo(Transform parent)
        {
            if (parent == null || _weaponObject == null) return;

            _weaponObject.transform.SetParent(parent);
            _weaponObject.transform.SetPositionAndRotation(parent.position, parent.rotation);
        }

        private static string GetShortName(AttackType attackType)
        {
            return WeaponData.GetShortName(Enum.GetName(typeof(AttackType), attackType));
        }

        public void CreateWeaponData()
        {
            WeaponData.CreateTotalShortNames();
        }

        /// <summary>
        /// Seals the current weapon modification array to protect the active combo build from unintended road pickups.
        /// </summary>
        public void ApplyWeaponSet()
        {
            _isBuildLocked = true;
            Debug.Log($"[Inventory] Weapon build locked successfully! Current configuration is sealed: {string.Join(";", _upgradeStatus)}");
        }

    }
}
