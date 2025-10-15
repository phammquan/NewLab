

using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Dropper : MonoBehaviour, Interactable
{
    [SerializeField] bool canInteractMySelf;

    [SerializeField] Vector3 PickUpRotation;
    [SerializeField] Vector3 DropRotation;


    [Header("Dropper Settings")]
    public Renderer liquidRenderer;
    [Tooltip("Khoảng cách tối đa để hút/nhỏ dung dịch.")]
    public float actionDistance = 5f;
    private string heldLiquidName = "";
    private bool isFull = false;
    private Color heldLiquidColor = Color.clear;

    [SerializeField] private List<Transform> posAnim;
    [SerializeField] private string tagTutorial;

    void Start()
    {
        UpdateDropperVisual();
    }


    public bool canInteract()
    {
        return canInteractMySelf;
    }

    public void Use(RaycastHit hitInfo)
    {
        if (hitInfo.collider == null) return;

        if (isFull)
        {
            TestTube testTube = hitInfo.collider.GetComponent<TestTube>();
            if (testTube != null)
            {
                Action doneMove = () =>
                {
                    Debug.Log("Nhỏ " + heldLiquidName + " vào ống nghiệm.");
                    testTube.AddLiquid(heldLiquidName, heldLiquidColor);
                    isFull = false;
                    heldLiquidName = ""; // <-- Reset tên
                    UpdateDropperVisual();
                };
                AnimMoveTestTube(doneMove, testTube.posMove);
               
            }
        }
        else
        {
            LiquidSource source = hitInfo.collider.GetComponent<LiquidSource>();
            if (source != null)
            {
                Action doneAction = () =>
                {
                    Debug.Log("Hút " + source.liquidName + ".");
                    isFull = true;
                    heldLiquidName = source.liquidName; // <-- LƯU LẠI TÊN
                    UpdateDropperVisual();
                    transform.localRotation = Quaternion.Euler(PickUpRotation);
                    FPSController.Instance.LockMovement = false;
                };
                if(!source.IsOpen) return;  
                AnimMoveLiquid(doneAction);

            }
        }
    }

    private void AnimMoveLiquid(Action done)
    {
        FPSController.Instance.LockMovement = true;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(posAnim[0].position, 0.5f).SetEase(Ease.InSine));
        seq.Append(transform.DOMove(posAnim[1].position, 0.5f).SetEase(Ease.InSine));
        seq.Join(transform.DORotate(new Vector3(0, -90, 90), 0.5f).SetEase(Ease.InSine));

        seq.OnComplete(() =>
        {
            Sequence returnSeq = DOTween.Sequence();

            returnSeq.Append(transform.DOMove(posAnim[0].position, 0.5f).SetEase(Ease.InSine));
            returnSeq.Append(transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InSine));
            returnSeq.Join(transform.DORotate(PickUpRotation, 0.5f).SetEase(Ease.InSine));

            returnSeq.OnComplete(() =>
            {
                FPSController.Instance.LockMovement = false;
                done.Invoke();
            });
        });
    }
    private void AnimMoveTestTube(Action done, List<Transform> posAnim)
    {
        FPSController.Instance.LockMovement = true;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(posAnim[0].position, 0.5f).SetEase(Ease.InSine));
        seq.Join(transform.DORotate(new Vector3(0, -90, 90), 0.5f).SetEase(Ease.InSine));
        seq.OnComplete(() =>
        {
            done.Invoke();
            Sequence returnSeq = DOTween.Sequence();
            returnSeq.Append(transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.InSine));
            returnSeq.Join(transform.DORotate(PickUpRotation, 0.5f).SetEase(Ease.InSine));
            returnSeq.OnComplete(() =>
            {
                FPSController.Instance.LockMovement = false;
            });
        });
    }
    public string GetTextTutorial()
    {
        return tagTutorial;
    }
    public bool IsValidPlacement(RaycastHit hitInfo)
    {
        if (hitInfo.collider.GetComponent<Tray>() != null)
        {
            return true;
        }
        
        return false;
    }
    public void OnPickup()
    {
        transform.localRotation = Quaternion.Euler(PickUpRotation);
    }
    
    public void OnDrop()
    {
       transform.DOLocalRotate(DropRotation, 0.1f).SetEase(Ease.OutBounce);
    }

    private void UpdateDropperVisual()
    {
        if (liquidRenderer != null)
        {
            bool canShow = isFull ? true : false;
            liquidRenderer.enabled = canShow;
        }
    }
}