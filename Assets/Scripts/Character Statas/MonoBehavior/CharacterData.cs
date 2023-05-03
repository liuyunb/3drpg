using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterData : MonoBehaviour
{

    public event Action<int, int> UpdateHealthUI;
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;

    public AttackData_SO templateAttackData;
    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;
    [HideInInspector]
    public bool isDead;

    [Header("Weapon")]
    public Transform weapon;

    Animator anim;
    RuntimeAnimatorController baseAnim;

    private void Awake()
    {
        characterData = Instantiate(templateData);
        attackData = Instantiate(templateAttackData);
        anim = GetComponent<Animator>();
        baseAnim = anim.runtimeAnimatorController;
    }

    //baseData
    #region
    public int maxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value; }
    }
    public int currentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }
    public int baseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }
    public int currentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }
    #endregion

    //attackData
    #region attack
    public float attackRange
    {
        get { if (characterData != null) return attackData.attackRange; else return 0; }
        set { attackData.attackRange = value; }
    }
    public float skillRange
    {
        get { if (characterData != null) return attackData.skillRange; else return 0; }
        set { attackData.skillRange = value; }
    }
    public float cd
    {
        get { if (characterData != null) return attackData.cd; else return 0; }
        set { attackData.cd = value; }
    }
    public float minDamage
    {
        get { if (characterData != null) return attackData.minDamage; else return 0; }
        set { attackData.minDamage = value; }
    }
    public float maxDamage
    {
        get { if (characterData != null) return attackData.maxDamage; else return 0; }
        set { attackData.maxDamage = value; }
    }
    public float criticalMultiply
    {
        get { if (characterData != null) return attackData.criticalMultiply; else return 0; }
        set { attackData.criticalMultiply = value; }
    }
    public float criticalChance
    {
        get { if (characterData != null) return attackData.criticalChance; else return 0; }
        set { attackData.criticalChance = value; }
    }
    #endregion

    //Level
    #region
    public int baseExp
    {
        get { if (characterData != null) return characterData.baseExp; else return 0; }
        set { characterData.baseExp = value; }
    }
    public int currentExp
    {
        get { if (characterData != null) return characterData.currentExp; else return 0; }
        set { characterData.currentExp = value; }
    }
    public int currentLevel
    {
        get { if (characterData != null) return characterData.currentLevel; else return 0; }
        set { characterData.currentLevel = value; }
    }
    public int maxLevel
    {
        get { if (characterData != null) return characterData.maxLevel; else return 0; }
        set { characterData.maxLevel = value; }
    }
    public float levelBuff
    {
        get { if (characterData != null) return characterData.levelBuff; else return 0; }
        set { characterData.levelBuff = value; }
    }
    public float levelMultiply
    {
        get { if (characterData != null) return characterData.levelMultiply; else return 0; }
    }
    public int killPoint
    {
        get { if (characterData != null) return characterData.killPoint; else return 0; }
        set { characterData.killPoint = value; }
    }
    #endregion

    public void TakeDamage(CharacterData attacker, CharacterData defender)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.currentDefence, 0);
        defender.currentHealth = Mathf.Max(defender.currentHealth - damage, 0);
        //如果暴击则触发受击动画
        if (isCritical)
            defender.GetComponent<Animator>().SetTrigger("Hit");

        //TODO:打死后更新UI或者经验
        defender.UpdateHealthUI?.Invoke(defender.currentHealth, defender.maxHealth);
        if (defender.currentHealth <= 0)
        {
            defender.isDead = true;
            attacker.UpdateExp(defender.killPoint);
        }
    }

    public void TakeDamage(int damage, CharacterData defender)
    {
        int currentDamage = Mathf.Max(damage - defender.currentDefence, 0);
        defender.currentHealth = Mathf.Max(defender.currentHealth - currentDamage, 0);

        UpdateHealthUI?.Invoke(defender.currentHealth, defender.maxHealth);
        if (defender.currentHealth <= 0)
        {
            defender.isDead = true;
            UpdateExp(defender.killPoint);
        }

    }

    public int CurrentDamage()
    {
        float damage = UnityEngine.Random.Range(minDamage, maxDamage);
        //暴击
        damage = isCritical ? damage * criticalMultiply : damage;
        return (int)damage;
    }

    private void UpdateExp(int killPoint)
    {
        currentExp += killPoint;
        if (currentExp >= baseExp)
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        currentLevel++;
        currentExp -= baseExp;
        baseExp = (int)(baseExp * levelMultiply);
        maxHealth = (int)(maxHealth * levelMultiply);
        currentHealth = maxHealth;

        Debug.Log("Level Up!");
    }

    public void EquipWeapon(ItemData_SO item)
    {
        if (item.itemPrefab != null)
            Instantiate(item.itemPrefab, weapon);

        //调整攻击数值
        attackData.UpdateData(item.attackData);
        anim.runtimeAnimatorController = item.animator;
        InventoryManager.Instance.UpdateStatasText(maxHealth, (int)minDamage, (int)maxDamage);
    }

    public void UnEquipWeapon()
    {
        for (int i = 0; i < weapon.childCount; i++)
        {
            Destroy(weapon.GetChild(i).gameObject);
        }
        attackData.UpdateData(templateAttackData);
        anim.runtimeAnimatorController = baseAnim;
        InventoryManager.Instance.UpdateStatasText(maxHealth, (int)minDamage, (int)maxDamage);
    }

    public void ChangeWeapon(ItemData_SO item)
    {
        UnEquipWeapon();
        EquipWeapon(item);
    }

    public void ApplyUsableItem(UsableItemData_SO usableData)
    {
        currentHealth = Mathf.Min(currentHealth + usableData.HealthPoint, maxHealth);
    }
}


