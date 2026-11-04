using FinalRide.Gameplay.Interfaces;
using FinalRide.Gameplay.Weapons.Base;

namespace FinalRide.Gameplay.Weapons.Modifiers
{
    public class ElectroWeapon : IWeaponModifier
    {
        /// <summary>
        /// Modifies runtime stats by injecting electric attributes, enabling armor bypass, and triggering micro-stun effects.
        /// </summary>
        /// <param name="stats">The active pass-by-reference configuration container.</param>
        public void ModifyStats(WeaponStats stats)
        {
            if (stats == null) return;

            // Activate electrical bitmask properties via the unified secure method
            stats.ApplyShockerAttributes();
        }
    }
}