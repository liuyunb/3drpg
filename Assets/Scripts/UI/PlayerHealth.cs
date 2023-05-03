using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    Image healthBar;
    Image ExpBar;
    Text level;

    private void Awake()
    {
        healthBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        ExpBar = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        level = transform.GetChild(2).GetComponent<Text>();
    }

    private void Update()
    {
        UpdateExp();
        UpdateHealth();
        level.text = "Level " + GameManager.Instance.playerData.currentLevel.ToString("00");
    }

    public void UpdateHealth()
    {
        float percent = GameManager.Instance.playerData.currentHealth * 1.0f / GameManager.Instance.playerData.maxHealth;
        healthBar.fillAmount = percent;
    }

    public void UpdateExp()
    {
        float percent = GameManager.Instance.playerData.currentExp * 1.0f / GameManager.Instance.playerData.baseExp;
        ExpBar.fillAmount = percent;
    }
}
