using System.Collections.Generic;

public interface IEnemy : IDiscriptable, IWeaponed
{
    public IEnumerable<Dictionary<string, string>> Abilities { get; set; }
}