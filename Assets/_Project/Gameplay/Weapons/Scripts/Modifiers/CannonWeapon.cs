using FinalRide.Gameplay.Interfaces;
using FinalRide.Gameplay.Weapons.Base;

namespace FinalRide.Gameplay.Weapons
{
    public class CannonWeapon : IWeaponModifier
    {
        private const float CannonDamageMultiplier = 1.5f;
        private const float CannonFireRateMultiplier = 1.8f; // Increases time delay between shots

        /// <summary>
        /// Modifies runtime stats by boosting damage, decreasing fire rate, and activating heavy-armor damage flags.
        /// </summary>
        /// <param name="stats">The active pass-by-reference configuration container.</param>
        public void ModifyStats(WeaponStats stats)
        {
            if (stats == null) return;

            // Amplify core characteristics using cumulative mathematical multipliers
            stats.Damage *= CannonDamageMultiplier;
            stats.FireRate *= CannonFireRateMultiplier;

            // Seal the persistent heavy damage bitmask flag using logical OR operation
            stats.ApplyCannonAttributes();
        }
    }
}