using System.Collections.Generic;

namespace FinalRide.Gameplay.Interfaces
{
    public interface IVehicle : IArmored, IMovable, IDamageable
    {
        public float MovementSpeed { get; }

        public float Energy { get; }

        public bool IsFlying { get; }
    }
}
