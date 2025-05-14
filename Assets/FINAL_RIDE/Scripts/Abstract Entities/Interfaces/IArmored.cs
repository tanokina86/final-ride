using System.Collections.Generic;

public interface IArmored
{
    public KeyValuePair<ArmorType, float> Armor { get; }
    public void SetArmor(ArmorType type, float durability);
}