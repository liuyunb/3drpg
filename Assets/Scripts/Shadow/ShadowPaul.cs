using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPaul : SingleTon<ShadowPaul>
{
    public GameObject shadowPfb;

    private Queue<GameObject> paul = new Queue<GameObject>();

    public float paulCount = 5;

    public void FillPaul()
    {
        for (int i = 0; i < paulCount; i++)
        {
            var shadow = Instantiate(shadowPfb, transform);
            ReturnPaul(shadow);
        }
    }

    public void ReturnPaul(GameObject shadow)
    {
        shadow.SetActive(false);
        paul.Enqueue(shadow);
    }

    public GameObject PopPaul()
    {
        Debug.Log(paul.Count);
        if(paul.Count <= 0)
            FillPaul();
        var shadow = paul.Dequeue();
        
        shadow.SetActive(true);

        return shadow;
    }

}
