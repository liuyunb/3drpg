using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;
[CustomEditor(typeof(DialogData_SO))]
public class DialogCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open In Editor"))
        {
            DialogEditor.InitWindow((DialogData_SO)target);
        }
        base.OnInspectorGUI();
    }
}

public class DialogEditor : EditorWindow
{
    private DialogData_SO dialogData;

    private ReorderableList piecesList = null;
    
    private Vector2 scrollPos = Vector2.zero;

    private Dictionary<string, ReorderableList> optionListDict = new Dictionary<string, ReorderableList>();

    [MenuItem("LyFighting/DialogEditor")]
    public static void Init()
    {
        DialogEditor dialogEditor = GetWindow<DialogEditor>("Dialog Editor");
        dialogEditor.autoRepaintOnSceneChange = true;
    }

    public static void InitWindow(DialogData_SO data)
    {
        DialogEditor dialogEditor = GetWindow<DialogEditor>("Dialog Editor");
        dialogEditor.dialogData = data;
    }
    
    [OnOpenAsset]
    public static bool OpenAsset(int instanceId, int line)
    {
        DialogData_SO data = EditorUtility.InstanceIDToObject(instanceId) as DialogData_SO;
        if (data != null)
        {
            InitWindow(data);
            return true;
        }

        return false;
    }

    private void OnSelectionChange()
    {
        var curData = Selection.activeObject as DialogData_SO;
        if (curData != null)
        {
            dialogData = curData;
            SetupReorderableList();
        }
        else
        {
            dialogData = null;
            piecesList = null;
        }
        Repaint();
    }

    private void OnGUI()
    {
        if (dialogData != null)
        {
            EditorGUILayout.LabelField(dialogData.name, EditorStyles.boldLabel);
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if(piecesList == null)
                SetupReorderableList();
            
            GUILayout.Space(10);
            
            piecesList.DoLayoutList();

            GUILayout.EndScrollView();
        }
        else
        {
            if (GUILayout.Button("Create New Asset"))
            {
                string path = "Assets/Game Data/DialogData/Dialog/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                DialogData_SO data = ScriptableObject.CreateInstance<DialogData_SO>();
                AssetDatabase.CreateAsset(data, path + "NewDialogData.asset");
                dialogData = data;
            }
            GUILayout.Label("No Data Selected", EditorStyles.boldLabel);
        }
    }

    public void SetupReorderableList()
    {
        piecesList = new ReorderableList(dialogData.dialog, typeof(DialogPiece), true, true, true, true);
        piecesList.drawHeaderCallback += OnDrawHeader;
        piecesList.drawElementCallback += OnDrawElement;
        piecesList.elementHeightCallback += OnHeightChange;
    }

    private float OnHeightChange(int index)
    {
        return GetPieceHeight(dialogData.dialog[index]);
    }

    private float GetPieceHeight(DialogPiece dialogPiece)
    {
        var height = EditorGUIUtility.singleLineHeight;
        if (dialogPiece.canFold)
        {
            height += EditorGUIUtility.singleLineHeight * 15;

            foreach (var option in dialogPiece.options)
            {
                height += EditorGUIUtility.singleLineHeight;
            }
        }
        
        return height;
    }

    private void OnDrawElement(Rect rect, int index, bool isactive, bool isfocused)
    {
        EditorUtility.SetDirty(dialogData);
        
        GUIStyle textStyle = new GUIStyle("TextField");
        
        if (index < dialogData.dialog.Count)
        {
            var curPiece = dialogData.dialog[index];

            var tempRect = rect;



            tempRect.height = EditorGUIUtility.singleLineHeight;

            curPiece.canFold = EditorGUI.Foldout(tempRect, curPiece.canFold, curPiece.Id);

            tempRect.y += tempRect.height + 5;

            if (curPiece.canFold)
            {
                tempRect.width = 30;
                EditorGUI.LabelField(tempRect, "Id");

                tempRect.x += tempRect.width;

                tempRect.width = 100;
                curPiece.Id = EditorGUI.TextField(tempRect, curPiece.Id);

                tempRect.x += tempRect.width + 10;

                tempRect.width = 45;
                EditorGUI.LabelField(tempRect, "Quest");

                tempRect.x += tempRect.width;

                tempRect.width = 100;
                curPiece.questData = EditorGUI.ObjectField(tempRect, curPiece.questData, typeof(QuestData_SO), false) as QuestData_SO;

                tempRect.y += EditorGUIUtility.singleLineHeight + 10;
                tempRect.x = rect.x;

                tempRect.width = 50;
                tempRect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.LabelField(tempRect, "Image");

                tempRect.x += tempRect.width + 10;

                tempRect.width = 100;
                tempRect.height = tempRect.width;
            
                curPiece.image = EditorGUI.ObjectField(tempRect, curPiece.image, typeof(Sprite), false) as Sprite;

                tempRect.y += tempRect.height + 10;
                tempRect.x = rect.x;

                tempRect.width = 50;
                tempRect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.LabelField(tempRect, "Text");

                tempRect.x += tempRect.width + 10;

                tempRect.width = 200;
                tempRect.height = tempRect.width / 4;
                textStyle.wordWrap = true;
                curPiece.Text = EditorGUI.TextField(tempRect, curPiece.Text, textStyle);

                tempRect.x = rect.x;
                tempRect.y += tempRect.height + 5;
                tempRect.width = rect.width;

                string optionKey = curPiece.Id + curPiece.Text;
                if (optionKey != string.Empty)
                {
                    if (!optionListDict.ContainsKey(optionKey))
                    {
                        var optionList = new ReorderableList(curPiece.options, typeof(DialogOption), true, true, true, true);
                        
                        optionList.drawHeaderCallback += OnDrawOptionHeader;
                        optionList.drawElementCallback += (optionRect, optionIndex, optionActive, optionFocused) =>
                        {
                            OnDrawOptionElement(curPiece, optionRect, optionIndex , optionActive, optionFocused);
                        };

                        optionListDict[optionKey] = optionList;
                    }
                    
                    optionListDict[optionKey].DoList(tempRect);
                }
            }


        }
    }

    private void OnDrawOptionElement(DialogPiece curPiece, Rect optionRect, int optionIndex, bool optionActive, bool optionFocused)
    {
        var tempRect = optionRect;
        var option = curPiece.options[optionIndex];
        tempRect.width = optionRect.width * 0.5f;
        option.text = EditorGUI.TextField(tempRect, option.text);

        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.3f;
        option.targetId = EditorGUI.TextField(tempRect, option.targetId);

        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.2f;
        option.takeQuest = EditorGUI.Toggle(tempRect, option.takeQuest);
    }


    private void OnDrawOptionHeader(Rect rect)
    {
        var tempRect = rect;
        tempRect.width = rect.width * 0.5f;
        GUI.Label(tempRect, "Option Text");

        tempRect.x += tempRect.width + 5;
        tempRect.width = rect.width * 0.3f;
        GUI.Label(tempRect, "Target Id");

        tempRect.x += tempRect.width + 5;
        tempRect.width = rect.width * 0.2f;
        GUI.Label(tempRect, "Apply");
    }

    private void OnDrawHeader(Rect rect)
    {
        GUI.Label(rect, "Dialog Pieces");
    }
}
