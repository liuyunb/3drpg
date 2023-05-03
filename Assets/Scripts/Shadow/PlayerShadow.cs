using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    public Material playerMa;

    private Material selfMa;

    private SkinnedMeshRenderer smr;

    public float alphaSet = 0.5f;
    public float alphaSpeed = 0.8f;
    private float alpha;

    public float liveTime = 0.5f;

    private void OnEnable()
    {
        alpha = alphaSet;
        selfMa = new Material(playerMa);
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        smr.material = selfMa;
    }

    private void Update()
    {
        alpha -= 1 / liveTime * alphaSpeed * Time.deltaTime;
        if (alpha <= 0)
        {
            ShadowPaul.Instance.ReturnPaul(this.gameObject);
        }
        else
        {
            Color color = new Color(0f, 0f, 0f, alpha);
            selfMa.color = color;
        }


    }
}
