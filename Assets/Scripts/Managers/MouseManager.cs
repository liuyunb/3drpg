using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;




//using UnityEngine.Events;

//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { }
public class MouseManager : MonoBehaviour
{
    private static MouseManager instance;
    public static MouseManager Instance { get { return instance; } }

    RaycastHit hitInfo;

    public Texture2D point, doorway, attack, target, arrow;

    public event Action<Vector3> OnMouseClick;
    public event Action<GameObject> OnEnemyAttack;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        SetCursorTexture();
        if(!InteractWithUI())
        {
            MouseControll();
        }
    }

    public void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,out hitInfo))
        {
            //…Ë÷√ Û±ÍÃ˘Õº
            if(InteractWithUI())
            {
                Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                return;
            }
            switch(hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16),CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Attackable":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "DoorWay":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Item":
                    Cursor.SetCursor(point, new Vector2(16, 16), CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    public void MouseControll()
    {
        if(Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if(hitInfo.collider.CompareTag("Ground"))
            {
                OnMouseClick?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.CompareTag("DoorWay"))
            {
                OnMouseClick?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.CompareTag("Item"))
            {
                OnMouseClick?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                OnEnemyAttack?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.CompareTag("Attackable"))
            {
                OnEnemyAttack?.Invoke(hitInfo.collider.gameObject);
            }
        }
    }

    public bool InteractWithUI()
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
            return false;
    }
}
