using FinalRide.Gameplay.Interfaces;
using FinalRide.Gameplay.Weapons.Base;

namespace FinalRide.Gameplay.Weapons
{
    public class SplitWeapon : IWeaponModifier
    {
        private const float SplitDamageMultiplier = 0.7f;
        private const float SplitScaleMultiplier = 0.75f;
        private const int AdditionalProjectiles = 2; // Converts single shot to a 3-way fan scatter
        private const float DefaultSpreadAngle = 15f;

        /// <summary>
        /// Modifies runtime stats by applying a damage penalty, decreasing bullet scale, and setting up fan-shaped multi-projectile routing.
        /// </summary>
        /// <param name="stats">The active pass-by-reference configuration container.</param>
        public void ModifyStats(WeaponStats stats)
        {
            if (stats == null) return;

            // Reduce individual bullet weight and physical dimensions
            stats.Damage *= SplitDamageMultiplier;
            stats.ProjectileScale *= SplitScaleMultiplier;

            // Inject multi-projectile parameters for spread calculation logic
            stats.ProjectileCount += AdditionalProjectiles;
            stats.SpreadAngle = DefaultSpreadAngle;
        }
    }
}