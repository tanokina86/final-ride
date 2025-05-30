﻿using System;
using System.Collections.Generic;

public interface IEnemy : IDiscriptable, IWeaponed
{
    public IEnumerable<Dictionary<string, Action>> Abilities { get; set; }
}