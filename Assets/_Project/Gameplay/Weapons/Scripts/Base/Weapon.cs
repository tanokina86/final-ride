using UnityEngine;
using Assets.Scripts.Enums; // Убедитесь, что путь к вашим энумам верный

namespace FinalRide.Gameplay.Weapons
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Final Ride/Weapon Data")]
    public class Weapon : ScriptableObject
    {
        [Header("Base Settings")]
        [SerializeField] private AttackType attackType = AttackType.Normal;
        [SerializeField] private float _rotationSpeed = 90f;
        [SerializeField] private Sprite mainSprite;

        [Header("Projectile Settings")]
        [SerializeField] private Projectile projectilePrefab;

        // Чистые геттеры для рантайм-логики стволов
        public AttackType AttackType => attackType;
        public float RotationSpeed => _rotationSpeed;
        public Sprite Sprite => mainSprite;
        public Projectile ProjectilePrefab => projectilePrefab;
    }
}
