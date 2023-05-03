using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiaLog",menuName = "Dialog/Dialog Data")]
public class DialogData_SO : ScriptableObject
{
    public List<DialogPiece> dialog = new List<DialogPiece>();

    public Dictionary<string,DialogPiece> dialogIndex = new Dictionary<string,DialogPiece>();

    public void InitializeIndex()
    {
        dialogIndex.Clear();

        foreach (DialogPiece piece in dialog)
        {
            if(!dialogIndex.ContainsKey(piece.Id))
            {
                dialogIndex.Add(piece.Id, piece);
            }
        }
    }

    public QuestData_SO GetTask()
    {
        foreach (var item in dialog)
        {
            if(item.questData != null)
            {
                return QuestManager.Instance.GetTask(item.questData)?.questData;
            }
        }
        return null;
    }
}
