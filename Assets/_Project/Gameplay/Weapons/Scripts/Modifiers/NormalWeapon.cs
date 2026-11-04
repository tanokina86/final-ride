using FinalRide.Gameplay.Interfaces;
using FinalRide.Gameplay.Weapons.Base;

namespace FinalRide.Gameplay.Weapons.Modifiers
{
    public class NormalWeapon : IWeaponModifier
    {
        private const float NormalFireRateMultiplier = 0.75f;

        /// <summary>
        /// Modifies runtime stats by boosting firing frequency and registering light-armor damage capabilities.
        /// </summary>
        /// <param name="stats">The active pass-by-reference configuration container from the core layer.</param>
        public void ModifyStats(WeaponStats stats)
        {
            if (stats == null) return;

            // Accelerate firing cycle using cumulative mathematical multiplication
            stats.FireRate *= NormalFireRateMultiplier;

            // Apply machine-gun specific properties and authorize damage tracking against Lite armor profiles
            stats.ApplyNormalAttributes();
        }
    }
}