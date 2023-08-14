using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float MaxHealth { get; }
    float CurrentHealth { get; set; }
    void Damage(float damage);
    void Heal(float amount);
}
