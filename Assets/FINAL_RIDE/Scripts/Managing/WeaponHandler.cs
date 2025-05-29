using Assets.FINAL_RIDE.Scripts.Abstract_Entities;
using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

namespace Assets.Scripts
{
    public class WeaponHandler
    {
        private Weapon _weaponEntity = null;

        GameObject _weapon = null;

        private const string GUN = "Gun";

        string test = string.Empty;
        private int currentStatusIndex = 0;

        private readonly string[] _upgradeStatus = new string[WeaponData.MAX_UPGRADE_NUMBER];

        public string[] UpgradeStatus => _upgradeStatus;

        delegate void WeaponDestroyHandler(Weapon weapon);
        event WeaponStateHandler OnWeaponDestroyed;

        delegate void WeaponStateHandler(AttackType attackType);
        event WeaponStateHandler OnWeaponUpgraded;

        public WeaponHandler(string owner)
        {
            //_weaponEntity = Weapon.CreateInstance(typeof(Weapon)) as Weapon;

            string weaponName = owner == "Player" ? "PlayerWeapon" : "Weapon";

            _weapon = new GameObject(weaponName);

        }
        public WeaponHandler(Weapon weapon)
        {
            //_weaponEntity = weapon;
        }

        public static void SetAttributes<T>(List<Attribute> attributes) where T : Attribute
        {
        }

        public void UpgradeWeapon(AttackType attackType)
        {
            string shortName = GetShortName(attackType);

            if (currentStatusIndex + 1 == WeaponData.MAX_UPGRADE_NUMBER)
            {
                //tODO: Create method, which generates an object with bind combination and player car upgrade
                _upgradeStatus[_upgradeStatus.Length - 1] = shortName;

                Debug.Log(string.Join(";", _upgradeStatus));
                Debug.Log("Car status changed");
            }

            //TODO: Add Listnerers for change sprite, animation and other things
            //OnWeaponUpgraded?.Invoke(attackType);

            else
            {
                _upgradeStatus[currentStatusIndex] = shortName;

                test += new string(shortName + ';');

                //Debug.Log(string.Join(";", _upgradeStatus));

                currentStatusIndex++;
            }

            string fullPath = $"Sprites/{shortName}";

            //Sprite loadedSprite = TrySetSpriteOnLoad(fullPath);
            Sprite loadedSprite = LoadSprite(fullPath);

            SetGunSprite(loadedSprite);
        }

        private static string GetShortName(AttackType attackType)
        {
            return WeaponData.GetShortName(Enum.GetName(typeof(AttackType), attackType));
        }

        public bool TrySetTo(Transform parent)
        {
            try
            {
                bool setted = _weaponEntity.Body ? _weaponEntity.Body.transform.parent = parent : null;
                return setted;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return false;
        }

        public void SetGunTo(Transform parent)
        {
            if (parent is null) return;

            _weapon.transform.parent = parent;

            _weapon.transform.SetPositionAndRotation(parent.position, parent.rotation);

            _weapon.transform.SetParent(parent);
        }

        public void CreateGun(AttackType attackType, string gunName)
        {
            _weapon.gameObject.AddComponent<Gun>();

            Gun gun = _weapon.GetComponent<Gun>();

            gun.Name = string.IsNullOrEmpty(gunName) ? string.Concat(attackType.ToString(), GUN) : gunName;

            Sprite loadedSprite = LoadSprite($"Sprites/{GetShortName(attackType)}");

            gun.Sprite = loadedSprite;

            gun.RendererOrder = _weapon.GetComponentInParent<SpriteRenderer>().sortingOrder + 1;

            Preset preset = null;

            if (_weapon.name == "PlayerWeapon" && gunName.Contains("Machine"))
            {
                preset = Resources.Load<Preset>("Presets/Weapon/default_machinegun");

            }
            if (Enum.GetName(typeof(AttackType), attackType).Contains("Split"))
            {
                preset = Resources.Load<Preset>("Presets/Weapon/splitter");
            }
            
            preset.ApplyTo(gun);

        }

        private void SetGunSprite(Sprite sprite)
        {
            _weapon.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        private Sprite TrySetSpriteOnLoad(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            Texture2D spriteTexture = new Texture2D(2, 2);

            try
            {
                spriteTexture = LoadTexture(FilePath);
            }
            catch (Exception exc)
            {
                Debug.Log("Texture was NOT LOADED " + exc.Message);
            }

            Sprite loadedSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

            return loadedSprite;
        }

        private Texture2D LoadTexture(string FilePath)
        {
            Texture2D texture = Resources.Load<Texture2D>(FilePath);

            return texture ?? texture;
        }


        private Sprite LoadSprite(string FilePath)
        {
            Sprite sprite = Resources.Load<Sprite>(FilePath);

            return sprite;
        }

        //public void Test()
        //{
        //    WeaponData.Create();
        //}

        public void CreateWeaponData()
        {
            WeaponData.CreateTotalShortNames();
        }
    }
}
