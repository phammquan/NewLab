// File: LiquidSource.cs
using UnityEngine;

public class LiquidSource : MonoBehaviour, Interactable
{
    public string liquidName = "HCl"; // <-- THÊM DÒNG NÀY
    public Color liquidColor = Color.blue;
    public string Text_Tut;
    [SerializeField] private bool canDrag;

    // Các hàm cũ giữ nguyên
    public Color GetLiquidColor()
    {
        return liquidColor;
    }

    public bool canInteract()
    {
        return canDrag;
    }

    public void Use(RaycastHit hitInfo)
    {
        throw new System.NotImplementedException();
    }

    public void OnPickup()
    {
        throw new System.NotImplementedException();
    }

    public void OnDrop()
    {
        throw new System.NotImplementedException();
    }

    public bool IsValidPlacement(RaycastHit hitInfo)
    {
        throw new System.NotImplementedException();
    }

    public string GetTextTutorial()
    {
        return Text_Tut;
    }
}