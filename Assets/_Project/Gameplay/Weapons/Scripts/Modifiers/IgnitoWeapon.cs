using FinalRide.Gameplay.Interfaces;
using FinalRide.Gameplay.Weapons.Base;

namespace FinalRide.Gameplay.Weapons.Modifiers
{
    public class IgnitoWeapon : IWeaponModifier
    {
        private const float IgnitoRangeMultiplier = 0.4f; // Drastically limits bullet lifetime/range for close combat
        private const float IgnitoFireRateMultiplier = 0.5f; // Makes the weapon fire much faster like a continuous stream

        /// <summary>
        /// Modifies runtime stats by reducing operational range, boosting firing frequency, and setting up flame characteristics.
        /// </summary>
        /// <param name="stats">The active pass-by-reference configuration container.</param>
        public void ModifyStats(WeaponStats stats)
        {
            if (stats == null) return;

            // Compress fire rate interval to simulate continuous flame stream execution
            stats.FireRate *= IgnitoFireRateMultiplier;

            // Note: Projectile scale or specific lifetime modifiers can be adjusted here 
            // to restrict the attack vector strictly to short distances
            stats.ProjectileScale *= 1.3f; // Visual representation of expanding fire particles
        }
    }
}