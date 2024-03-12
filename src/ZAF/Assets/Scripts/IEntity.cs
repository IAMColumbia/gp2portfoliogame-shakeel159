using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    string Name { get; set; }
    float health { get; set; }
    float attackDmg { get; set; }
}
