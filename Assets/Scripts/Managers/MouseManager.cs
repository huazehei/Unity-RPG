using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// using UnityEngine.Events;
// [System.Serializable]
// public class EventVector3 : UnityEvent<Vector3> { }

public class MouseManager : Singleton<MouseManager>
{
    RaycastHit hitInfo;
    public event Action<Vector3> OnMouseClick;
    public event Action<GameObject> OnEnemyClick;
    public Texture2D point, doorWay, attack, target, arrow;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        SetCursorTexture();
        if (InteractWithUI())
            return;
        MouseControl();
    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (InteractWithUI())
        {
            Cursor.SetCursor(arrow,new Vector2(0,0),CursorMode.Auto);
            return;
        }
        if (Physics.Raycast(ray, out hitInfo))
        {
            //切换鼠标贴图
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Attackable":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(doorWay,new Vector2(16,16),CursorMode.Auto);
                    break;
                case "Item":
                    Cursor.SetCursor(point,new Vector2(16,16),CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(arrow,new Vector2(0,0),CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
                OnMouseClick?.Invoke(hitInfo.point);
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            if(hitInfo.collider.gameObject.CompareTag("Attackable"))
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
                OnMouseClick?.Invoke(hitInfo.point);
            if (hitInfo.collider.gameObject.CompareTag("Item"))
                OnMouseClick?.Invoke(hitInfo.point);
        }
    }

    bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return true;
        return false;
    }
}
