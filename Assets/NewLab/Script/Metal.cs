// File: IronNail.cs

using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public abstract class MetalBase : MonoBehaviour, Interactable
{
    [SerializeField] bool canInteractMySelf;
    [SerializeField] private string tagTutorial;
    
    
    [SerializeField] private Vector3 dragRotation;
    [SerializeField] private Vector3 dropRotation;


    public bool canInteract()
    {
        return canInteractMySelf;
    }

    public virtual void Use(RaycastHit hitInfo)
    {
        if (hitInfo.collider != null)
        {
            TestTube testTube = hitInfo.collider.GetComponent<TestTube>();
            if (testTube != null && !testTube.isItem )
            {
                Action doneMove = () =>
                {
                   ActionDone(testTube);
                };
                AnimMoveLiquid(doneMove, testTube.posMove);

            }
        }
    }

    protected abstract void ActionDone(TestTube testTube);
    private void AnimMoveLiquid(Action done, List<Transform> posMove)
    {
        FPSController.Instance.LockMovement = true;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(posMove[0].position, 0.5f).SetEase(Ease.InSine));
        seq.Append(transform.DOMove(posMove[1].position, 0.5f).SetEase(Ease.InSine));
        seq.Join(transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutBounce));

        seq.OnComplete(() =>
        {
            FPSController.Instance.LockMovement = false;
            done.Invoke();
        });
    }

    public bool IsValidPlacement(RaycastHit hitInfo)
    {
        if (hitInfo.collider.GetComponent<Tray>() != null)
        {
            return true;
        }

        return false;
    }
    

    public string GetTextTutorial()
    {
        return tagTutorial;
    }
    #region Interactable Interface (để trống nếu không cần logic riêng)
    

    public void OnPickup()
    {
        transform.DORotate(dragRotation, 0.5f).SetEase(Ease.InSine);

    }

    public virtual void OnDrop()
    {
        transform.DORotate(dropRotation, 0.5f).SetEase(Ease.OutBounce);
    }

    #endregion
}