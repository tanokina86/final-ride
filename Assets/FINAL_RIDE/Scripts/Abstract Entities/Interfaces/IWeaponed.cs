using Assets.Scripts.Enums;
using System.Collections.Generic;

public interface IWeaponed
{
    public AttackType AttackType { get; }

    public Dictionary<AttackType, KeyValuePair<float, float>> Weapon { get; }

    public bool HasAdditionalWeapon { get; }

    public abstract void Shoot(float baseDamage);
}