using Assets.Scripts;

using FinalRide.Gameplay.Weapons.Base;

namespace FinalRide.Gameplay.Interfaces
{
    public interface IWeaponed
    {
        const Weapon weapon = null;

        public Weapon Weapon { get; set; }

        public bool HasAdditionalWeapon { get; }
    }
}
