using FinalRide.Gameplay.Interfaces;
using FinalRide.Gameplay.Weapons.Base;

namespace FinalRide.Gameplay.Weapons.Modifiers
{
    public class ElectroWeapon : IWeaponModifier
    {
        private const float SHOKER_MAGNETISM_RANGE = 2.5f;

        /// <summary>
        /// Modifies runtime stats by injecting advanced electric configurations, activating micro-stun tags, and registering dynamic proximity magnetism.
        /// </summary>
        /// <param name="stats">The active pass-by-reference configuration container inside the base layer.</param>
        public void ModifyStats(WeaponStats stats)
        {
            if (stats == null) return;

            // Route execution onto the core layout block to lock stun properties and configure dynamic magnet vectors
            stats.ApplyShockerAttributes(SHOKER_MAGNETISM_RANGE);
        }
    }
}
