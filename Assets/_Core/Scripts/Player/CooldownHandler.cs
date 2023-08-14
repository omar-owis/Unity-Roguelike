using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownHandler : MonoBehaviour
{
    List<AbilityCooldown> _abilitiesOnCooldown = new List<AbilityCooldown>();
    public static CooldownHandler instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // handle cooldown count down
    private void Update()
    {
        for (int i = _abilitiesOnCooldown.Count - 1; i >= 0; i--)
        {
            if (_abilitiesOnCooldown[i].TimeRemaining <= 0)
            {
                _abilitiesOnCooldown.RemoveAt(i);
            }
            else
            {
                _abilitiesOnCooldown[i].Tick(Time.deltaTime);
            }
        }
    }

    public void SetAbilityOnCooldown(AbstractAbilityObject ability)
    {
        if (OnCooldown(ability)) return; // if the ability is already on cooldown return

        _abilitiesOnCooldown.Add(new AbilityCooldown(ability));
    }

    public float CooldownAmountLeft(AbstractAbilityObject ability)
    {
        for (int i = 0; i < _abilitiesOnCooldown.Count; i++)
        {
            if (_abilitiesOnCooldown[i].AbilityObject == ability) return _abilitiesOnCooldown[i].TimeRemaining;
        }
        return 0;
    }

    public bool OnCooldown(AbstractAbilityObject ability)
    {
        for (int i = 0; i < _abilitiesOnCooldown.Count; i++)
        {
            if (_abilitiesOnCooldown[i].AbilityObject == ability) return true;
        }
        return false;
    }
}

class AbilityCooldown
{
    private float _timeRemaining;
    private AbstractAbilityObject _abilityObject;

    public float TimeRemaining { get { return _timeRemaining; } }
    public AbstractAbilityObject AbilityObject { get { return _abilityObject; } }

    public AbilityCooldown(AbstractAbilityObject ability)
    {
        _abilityObject = ability;
        _timeRemaining = ability.cooldown;
    }

    public void Tick(float DeltaTime)
    {
        _timeRemaining -= DeltaTime;
    }

}