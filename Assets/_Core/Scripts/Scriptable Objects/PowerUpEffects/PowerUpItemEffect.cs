using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpItemEffect : ScriptableObject
{
    public abstract void ExecuteEffect(PowerUpObject item, CharacterData c);

}
