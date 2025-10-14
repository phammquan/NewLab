// File: LiquidSource.cs
using UnityEngine;

public class LiquidSource : MonoBehaviour
{
    public string liquidName = "HCl"; // <-- THÊM DÒNG NÀY
    public Color liquidColor = Color.blue;

    // Các hàm cũ giữ nguyên
    public Color GetLiquidColor()
    {
        return liquidColor;
    }
}