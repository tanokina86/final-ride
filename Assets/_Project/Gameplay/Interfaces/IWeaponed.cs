using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections.Generic;

using FinalRide.Gameplay.Weapons;

namespace FinalRide.Gameplay.Interfaces
{
    public interface IWeaponed
    {
        const Weapon weapon = null;

        public Weapon Weapon { get; set; }

        public bool HasAdditionalWeapon { get; }
    }
}
