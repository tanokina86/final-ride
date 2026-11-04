using Assets.Scripts;
using Assets.Scripts.Enums;
using FinalRide.Gameplay.Weapons;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinalRide.Gameplay.Weapons
{
    public class WeaponHandler
    {
        private GameObject _weaponObject = null;
        private WeaponSlot _weaponSlot = null;

        private int currentStatusIndex = 0;
        private readonly string[] _upgradeStatus = new string[WeaponData.MAX_UPGRADE_NUMBER];

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

            if (parent != null)
            {
                SetGunTo(parent);
            }
        }

        public void UpgradeWeapon(AttackType attackType)
        {
            string shortName = GetShortName(attackType);

            // Реализуем честный сдвиг очереди (FIFO) при достижении максимума
            if (currentStatusIndex >= WeaponData.MAX_UPGRADE_NUMBER)
            {
                for (int i = 0; i < _upgradeStatus.Length - 1; i++)
                {
                    _upgradeStatus[i] = _upgradeStatus[i + 1];
                }
                _upgradeStatus[_upgradeStatus.Length - 1] = shortName;

                Debug.Log($"[Inventory] Сдвиг! Текущий сет: {string.Join(";", _upgradeStatus)}");
                Debug.Log("Car status changed");
            }
            else
            {
                _upgradeStatus[currentStatusIndex] = shortName;
                currentStatusIndex++;
            }

            // Накатываем новые данные из ScriptableObject на наш физический слот ствола
            ApplyWeaponFromResources(shortName);

            OnWeaponUpgraded?.Invoke(attackType);
        }

        public void CreateGun(AttackType attackType, string gunName = "")
        {
            string shortName = GetShortName(attackType);

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
    }
}
