using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "AttackData/New Data")]
public class AttackData_SO : ScriptableObject
{
    [Header("Attack Info")]
    public float attackRange;
    public float skillRange;
    public float cd;
    public float minDamage;
    public float maxDamage;
    public float criticalMultiply;
    public float criticalChance;

    public void UpdateData(AttackData_SO item)
    {
        attackRange = item.attackRange;
        skillRange = item.skillRange;
        cd = item.cd;
        minDamage = item.minDamage;
        maxDamage = item.maxDamage;
        criticalChance = item.criticalChance;
        criticalMultiply = item.criticalMultiply;
    }
}
