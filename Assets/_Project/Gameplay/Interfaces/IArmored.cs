using System.Collections.Generic;

namespace FinalRide.Gameplay.Interfaces
{
    public interface IArmored
    {
        public KeyValuePair<ArmorType, float> Armor { get; }
        public void SetArmor(ArmorType type, float durability);
    }
}