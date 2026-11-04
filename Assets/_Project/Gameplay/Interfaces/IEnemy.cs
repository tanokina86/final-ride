using System;
using System.Collections.Generic;

namespace FinalRide.Gameplay.Interfaces
{
    public interface IEnemy : IDiscriptable, IWeaponed
    {
        public IEnumerable<Dictionary<string, Action>> Abilities { get; set; }
    }
}
