// File: LiquidSource.cs

using DG.Tweening;
using UnityEngine;

public class LiquidSource : MonoBehaviour, Interactable, IOpenable
{
    public string liquidName = "HCl"; // <-- THÊM DÒNG NÀY
    public Color liquidColor = Color.blue;
    public string Text_Tut;
    [SerializeField] private bool canDrag;

    [SerializeField] private Transform nap;
    [SerializeField] private Transform[] positions;

    public bool isOpen = false;

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
    }

    public bool IsOpen
    {
        get { return isOpen; }
    }


    private void Open()
    {
        isOpen = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(nap.DOMove(positions[1].position, 0.5f).SetEase(Ease.InSine));
        seq.Append(nap.DOMove(positions[2].position, 0.5f).SetEase(Ease.InSine));

    }

    public void Close()
    {
        isOpen = false;
        Sequence seq = DOTween.Sequence();
        seq.Append(nap.DOMove(positions[1].position, 0.5f).SetEase(Ease.InSine));
        seq.Append(nap.DOMove(positions[0].position, 0.5f).SetEase(Ease.InSine));
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

    void IOpenable.Open()
    {
        Open();
    }
}