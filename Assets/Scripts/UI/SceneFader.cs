using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    CanvasGroup cg;
    float alpha = 0;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(this);
    }

    public IEnumerator FadeOut(float time)
    {
        while(alpha < 1)
        {
            alpha += Time.deltaTime / time;
            cg.alpha = alpha;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / time;
            cg.alpha = alpha;
            yield return null;
        }
        Destroy(gameObject);
    }
}
