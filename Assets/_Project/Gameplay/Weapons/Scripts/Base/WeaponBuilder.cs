using System.Collections.Generic;
using FinalRide.Gameplay.Interfaces;
using FinalRide.Gameplay.Weapons.Modifiers;

namespace FinalRide.Gameplay.Weapons.Base
{
    public class WeaponStatsBuilder
    {
        private readonly WeaponStats _compiledStats;
        private readonly Dictionary<string, System.Func<IWeaponModifier>> _modifierFactory;

        public WeaponStatsBuilder()
        {
            _compiledStats = new WeaponStats();

            // Pure Poly-morphic registry maps short codes directly to concrete runtime classes without switches
            _modifierFactory = new Dictionary<string, System.Func<IWeaponModifier>>
            {
                { "nr", () => new NormalWeapon() },
                { "cn", () => new CannonWeapon() },
                { "sp", () => new SplitWeapon() },
                { "el", () => new ElectroWeapon() },
                { "ig", () => new IgnitoWeapon() }
            };
        }

        /// <summary>
        /// Translates the clean upgrade string status queue into concrete modifiers and compiles final stats.
        /// </summary>
        /// <param name="upgradeStatus">The active string array sequence containing pickup histories from WeaponHandler.</param>
        /// <returns>A fully compiled snapshot of combat statistics including thermal thresholds.</returns>
        public WeaponStats BuildFinalStats(string[] upgradeStatus)
        {
            // Reset numerical bounds to default baseline configurations
            _compiledStats.Damage = 10f;
            _compiledStats.FireRate = 0.5f;
            _compiledStats.ProjectileCount = 1;
            _compiledStats.SpreadAngle = 0f;
            _compiledStats.ProjectileScale = 1f;
            _compiledStats.RotationSpeedMultiplier = 1f;

            _compiledStats.HeatPerShot = 5f;
            _compiledStats.CoolingRate = 20f;
            _compiledStats.OverheatCooldownDuration = 2.5f;

            if (upgradeStatus == null || upgradeStatus.Length == 0)
            {
                return _compiledStats;
            }

            // Iterate linearly through the established text token stream to process modification strategies
            for (int i = 0; i < upgradeStatus.Length; i++)
            {
                string token = upgradeStatus[i];
                if (!string.IsNullOrEmpty(token) && _modifierFactory.TryGetValue(token, out var factoryMethod))
                {
                    IWeaponModifier modifier = factoryMethod.Invoke();
                    if (modifier != null)
                    {
                        modifier.ModifyStats(_compiledStats);
                    }
                }
            }

            return _compiledStats;
        }
    }
}