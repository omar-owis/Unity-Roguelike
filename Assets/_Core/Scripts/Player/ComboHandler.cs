using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ComboHandler : MonoBehaviour
{
    List<ComboAbilityTimer> _comboAbilityTimers = new List<ComboAbilityTimer>();
    public static ComboHandler instance;

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

    private void Update()
    {
        for (int i = _comboAbilityTimers.Count - 1; i >= 0; i--)
        {
            if (_comboAbilityTimers[i].TimeRemaining <= 0)
            {
                CooldownHandler.instance.SetAbilityOnCooldown(_comboAbilityTimers[i].ComboAbility);
                _comboAbilityTimers.RemoveAt(i);
            }
            else
            {
                _comboAbilityTimers[i].Tick(Time.deltaTime);
            }
        }
    }

    // resets the combo timer if ability is already in list. else adds combo timer to list
    // returns ability to be casted in sequance
    public AbstractAbilityObject AbilityToActivate(ComboAbility ability)
    {
        AbstractAbilityObject abilityToActivate = null;
        int index = ComboIndex(ability);

        if (index > -1)
        {
            _comboAbilityTimers[index].ResetTimer(ability.ResetComboTime);
            abilityToActivate = ability.ReturnAbility(_comboAbilityTimers[index].ComboIndex);
            _comboAbilityTimers[index].IncrementCombo();
        }
        else
        {
            ComboAbilityTimer newCombo = new ComboAbilityTimer(ability);
            abilityToActivate = ability.ReturnAbility(0);
            newCombo.IncrementCombo();
            _comboAbilityTimers.Add(newCombo);
        }

        return abilityToActivate;
    }

    public int ComboIndex(ComboAbility ability)
    {
        for (int i = 0; i < _comboAbilityTimers.Count; i++)
        {
            if (_comboAbilityTimers[i].ComboAbility == ability) return i;
        }

        return -1;
    }

}

class ComboAbilityTimer
{
    private float _timeRemaining;
    private ComboAbility _comboAbility;
    private int _comboIndex;

    public float TimeRemaining { get { return _timeRemaining; } }
    public ComboAbility ComboAbility { get { return _comboAbility; } }

    public int ComboIndex { get { return _comboIndex; } }

    public ComboAbilityTimer(ComboAbility combo)
    {
        _comboAbility = combo;
        _timeRemaining = combo.ResetComboTime;
        _comboIndex = 0;
    }

    public void Tick(float DeltaTime)
    {
        _timeRemaining -= DeltaTime;
    }

    public void ResetTimer(float time)
    {
        _timeRemaining = time;
    }

    public void IncrementCombo()
    {
        if (_comboIndex + 1 > _comboAbility.SequenceLength - 1)
        {
            _comboIndex = 0;
            CooldownHandler.instance.SetAbilityOnCooldown(_comboAbility);
        }

        else _comboIndex++;
    }
}
