using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections.Generic;

public interface IWeaponed
{
    const Weapon weapon = null;

    public Weapon Weapon { get; set; }

    public bool HasAdditionalWeapon { get; }
}