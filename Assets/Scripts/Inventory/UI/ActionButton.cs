using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode keyCode;

    SlotHolder currentSlot;

    bool isUse;

    private void Awake()
    {
        currentSlot = GetComponent<SlotHolder>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(keyCode))
        {
            isUse = true;
        }
    }
    private void FixedUpdate()
    {
        if(isUse)
        {
            isUse = false;
            currentSlot.UseItem();
        }
    }
}
