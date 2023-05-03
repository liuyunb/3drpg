using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData",menuName = "CharacterData/New Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Data Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    [Header("KillPoint")]
    public int killPoint;

    [Header("Level")]
    public int baseExp;
    public int currentExp;
    public int currentLevel;
    public int maxLevel;
    public float levelBuff;
    public float levelMultiply { get { return (currentLevel - 1) * levelBuff + 1;  } }

}
