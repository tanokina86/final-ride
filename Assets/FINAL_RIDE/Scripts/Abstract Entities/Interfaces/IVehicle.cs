using System.Collections.Generic;

public interface IVehicle : IArmored, IMovable
{
    public float MovementSpeed { get; }

    public float Energy { get; }

    public int Health { get; }
    public bool IsFlying { get; }

}