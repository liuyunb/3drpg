using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject healthPrefab;
    public Transform barPos;
    public Canvas canvas;
    public bool alwaysVisable;
    public float visableTime = 3f;
    float remainVisableTime;

    Camera cam;
    Transform healthBar;
    Image healthImg;
    CharacterData characterData;

    private void Awake()
    {
        characterData = GetComponent<CharacterData>();
        characterData.UpdateHealthUI += UpdateHealth;
        remainVisableTime = visableTime;

    }

    private void OnEnable()
    {
        cam = Camera.main;
        if(healthBar == null)
        {
            healthBar = Instantiate(healthPrefab, canvas.transform).transform;
            healthBar.position = barPos.position;
            healthImg = healthBar.GetChild(0).GetComponent<Image>();
            healthBar.gameObject.SetActive(alwaysVisable);
        }

    }


    public void UpdateHealth(int currentHealth,int maxHealth)
    {
        if (currentHealth <= 0)
            Destroy(healthBar.gameObject);
        if(healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
            float precent = currentHealth * 1.0f / maxHealth;
            healthImg.fillAmount = precent;
            remainVisableTime = visableTime;
        }
    }


    private void LateUpdate()
    {
        if(healthBar != null)
        {
            healthBar.position = barPos.position;
            healthBar.forward = -cam.transform.forward;
            if (remainVisableTime > 0 && !alwaysVisable)
            {
                remainVisableTime -= Time.deltaTime;
            }
            else
                healthBar.gameObject.SetActive(alwaysVisable);
        }
    }
}
