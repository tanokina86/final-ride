using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour, IVehicle
{
    [SerializeField]
    private float _speed = 0f;

    [SerializeField]
    private ArmorType _armorType;

    public int Health { get; } = 100;

    public float Energy { get; } = 0f;


    private KeyValuePair<ArmorType, float> _armor = new KeyValuePair<ArmorType, float>(ArmorType.Lite, 100f);
    public bool IsFlying { get; } = false;

    public float MovementSpeed => _speed;

    public KeyValuePair<ArmorType, float> Armor => _armor ;

    public abstract void Move();

    public void SetArmor(ArmorType type, float durability)
    {
        _armor = new KeyValuePair<ArmorType, float>(type, durability);
    }
}
