using UnityEngine;
using FinalRide.Gameplay.Interfaces;
using FinalRide.Gameplay.Weapons.Modifiers;

namespace FinalRide.Gameplay.Weapons.Blueprints
{
    [CreateAssetMenu(fileName = "NewCannonBlueprint", menuName = "FinalRide/Weapons/Blueprints/Cannon")]
    public class CannonBlueprint : WeaponBlueprint
    {
        /// <summary>
        /// Instantiates a fresh concrete instance of the Cannon modification logic strategy block.
        /// </summary>
        public override IWeaponModifier CreateModifier() => new CannonWeapon();
    }
}