using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public Text itemName;
    public Text itemDetail;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        SetPosition();
    }

    private void Update()
    {
        SetPosition();
    }
    public void SetUpToolTip(ItemData_SO itemData)
    {
        itemName.text = itemData.itemName;
        itemDetail.text = itemData.itemDetials;
    }

    public void SetPosition()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float height = corners[1].y - corners[0].y;
        float width = corners[3].x - corners[0].x;

        if (mousePos.y < height)
            transform.position = mousePos + Vector3.up * height * 0.6f;
        else if (Screen.width - mousePos.x < width)
            transform.position = mousePos + Vector3.left * width * 0.6f;
        else
            transform.position = mousePos + Vector3.right * width * 0.6f;

    }
}
