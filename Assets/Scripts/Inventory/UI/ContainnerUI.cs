using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainnerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;

    public void RefreshUI()
    {
        for(int i = 0;i <slotHolders.Length;i++)
        {
            slotHolders[i].itemUI.index = i;
            slotHolders[i].UpdateUI();
        }
    }
}
