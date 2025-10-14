// File: Interactor.cs

using TMPro;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Interaction Settings")] public float interactionDistance = 2f;
    public LayerMask interactableLayer;

    [Header("Placement Settings")] public LayerMask placeableLayer;
    public float maxPlaceDistance = 3f;

    [Header("Hold Settings")] public Transform holdParent;
    private GameObject heldObject;

    [SerializeField] private GameObject tut;
    [SerializeField] TMP_Text tutorialText;


    void Update()
    {
        if (FPSController.Instance.LockMovement) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject != null)
            {
                UseHeldObject();
            }
            else
            {
                TryPickUpObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && heldObject != null)
        {
            TryPlaceObject();
        }

        GetTextTagTutorial();
    }

    void GetTextTagTutorial()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 5, interactableLayer))
        {
            Debug.Log(hitInfo.collider.name);
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable == null)
            {
                Debug.LogError("Deactive");
                tut.SetActive(false);
                return;
            }
            tut.SetActive(true);
            tutorialText.text = interactable.GetTextTutorial();
        }
        else
        {
            tut.SetActive(false);
        }
    }

    void TryPickUpObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, interactionDistance, interactableLayer))
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable == null || !interactable.canInteract()) return;
            PickUpObject(hitInfo.collider.gameObject);
            interactable.OnPickup();
        }
    }

    void UseHeldObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, maxPlaceDistance);

        heldObject.GetComponent<Interactable>().Use(hitInfo);
    }

    void PickUpObject(GameObject objectToPickUp)
    {
        objectToPickUp.transform.SetParent(holdParent);
        objectToPickUp.transform.localPosition = Vector3.zero;
        objectToPickUp.transform.localRotation = Quaternion.identity;
        heldObject = objectToPickUp;
        objectToPickUp.GetComponent<Collider>().enabled = false;
    }

    void TryPlaceObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, maxPlaceDistance, placeableLayer))
        {
            if (heldObject.GetComponent<Interactable>().IsValidPlacement(hitInfo))
            {
                heldObject.GetComponent<Interactable>().OnDrop();
                heldObject.GetComponent<Collider>().enabled = true;
                PlaceObjectOnSurface(hitInfo.point, hitInfo.normal);
            }
            else
            {
                Debug.Log("Không thể đặt " + heldObject.name + " tại đây!");
            }
        }
        // Nếu không trỏ vào bề mặt nào, cũng không làm gì cả.
    }

    // File: Interactor.cs
    void PlaceObjectOnSurface(Vector3 position, Vector3 surfaceNormal)
    {
        heldObject.transform.SetParent(null);
        Collider objectCollider = heldObject.GetComponent<Collider>();


        heldObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
        heldObject.transform.position = position;


        Vector3 bottomPoint = objectCollider.ClosestPoint(position - surfaceNormal * 10f);


        float distanceToMoveUp = Vector3.Distance(position, bottomPoint);


        heldObject.transform.position = position + surfaceNormal * distanceToMoveUp;

        heldObject = null;
    }
}