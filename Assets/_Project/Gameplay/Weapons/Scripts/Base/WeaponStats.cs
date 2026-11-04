using System.Collections.Generic;

namespace FinalRide.Gameplay.Weapons.Base
{
    public class WeaponStats
    {
        public float Damage { get; set; } = 10f;
        public float FireRate { get; set; } = 0.5f;
        public int ProjectileCount { get; set; } = 1;
        public float SpreadAngle { get; set; } = 0f;
        public float ProjectileScale { get; set; } = 1f;
        public float RotationSpeedMultiplier { get; set; } = 1f;

        public float HeatPerShot { get; set; } = 5f;
        public float CoolingRate { get; set; } = 20f;
        public float OverheatCooldownDuration { get; set; } = 2.5f;

        public bool IsBypassingArmor { get; private set; } = false;
        public bool HasMicroStun { get; private set; } = false;

        public HashSet<ArmorType> PiercedArmorTypes { get; private set; } = new HashSet<ArmorType>();

        /// <summary>
        /// Safely applies shocker attributes using logical OR and updates status flags.
        /// </summary>
        public void ApplyShockerAttributes()
        {
            IsBypassingArmor |= true;
            HasMicroStun |= true;
        }

        /// <summary>
        /// Safely applies heavy cannon attributes to the current weapon stack calculation node.
        /// </summary>
        public void ApplyCannonAttributes()
        {
            PiercedArmorTypes.Add(ArmorType.Hard);
        }

        /// <summary>
        /// Safely applies normal machine-gun attributes to the active weapon stack calculation block.
        /// </summary>
        public void ApplyNormalAttributes()
        {
            PiercedArmorTypes.Add(ArmorType.Lite);
        }

        /// <summary>
        /// Safely applies split multi-projectile scatter layout configurations to the current weapon stack.
        /// </summary>
        /// <param name="additionalProjectiles">The number of additional bullets spawned inside the fan pattern.</param>
        /// <param name="angle">The target maximum angle limit for the weapon fire spread vector.</param>
        public void ApplySplitAttributes(int additionalProjectiles, float angle)
        {
            ProjectileCount += additionalProjectiles;
            SpreadAngle = angle;
        }

        /// <summary>
        /// Safely applies continuous flame stream particle characteristics to the active weapon stack.
        /// </summary>
        public void ApplyIgnitoAttributes()
        {
            // Intentionally left open for unique dots, environmental fire flags or continuous tick tracking attributes
        }
    }
}