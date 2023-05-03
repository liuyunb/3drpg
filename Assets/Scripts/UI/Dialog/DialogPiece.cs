using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogPiece
{
    public string Id;

    [TextArea]
    public string Text;

    public Sprite image;

    public bool canFold;

    public List<DialogOption> options = new List<DialogOption>();

    public QuestData_SO questData;

}
