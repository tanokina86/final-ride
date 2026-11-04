using UnityEngine;
using FinalRide.Gameplay.Interfaces;
using Assets.Scripts.Enums;

namespace FinalRide.Gameplay.Weapons.Blueprints
{
    public abstract class WeaponBlueprint : ScriptableObject
    {
        [Header("Identity Settings")]
        [SerializeField] private AttackType attackType;
        [SerializeField] private string shortToken;

        public AttackType AttackType => attackType;
        public string ShortToken => shortToken;

        /// <summary>
        /// Factory method that instantiates the concrete pure C# weapon modifier class without switch statements.
        /// </summary>
        public abstract IWeaponModifier CreateModifier();
    }
}