// File: Interactor.cs

using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] private GameObject tutLaptop;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject report;

    void Update()
    {
        if (FPSController.Instance.LockMovement) return;

        HandleLaptopInteraction();

        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject != null)
            {
                UseHeldObject();
            }
            else
            {
                TryPickUpObject();
                TryOpenCloseObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && heldObject != null)
        {
            TryPlaceObject();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            tutorial.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            report.SetActive(true);
        }
        GetTextTagTutorial();
    }

    void HandleLaptopInteraction()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, interactionDistance, interactableLayer))
        {
            LaptopController laptop = hitInfo.collider.GetComponent<LaptopController>();
            if (laptop != null)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    laptop.ReloadCurrentScene();
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    laptop.LoadPreviousScene();
                }
            }
        }
    }


    void GetTextTagTutorial()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 5, interactableLayer))
        {
            LaptopController laptop = hitInfo.collider.GetComponent<LaptopController>();
            if (laptop != null)
            {
                tutLaptop.SetActive(true);
                return;
            }
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable == null)
            {
                tut.SetActive(false);
                return;
            }

            tut.SetActive(true);
            tutorialText.text = interactable.GetTextTutorial();
        }
        else
        {
            tut.SetActive(false);
            tutLaptop.SetActive(false);
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
            if (hitInfo.collider.GetComponent<LaptopController>() != null) return;
            
            PickUpObject(hitInfo.collider.gameObject);
            interactable.OnPickup();
        }
    }

    void TryOpenCloseObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, interactionDistance, interactableLayer))
        {
            IOpenable openable = hitInfo.collider.GetComponent<IOpenable>();
            if (openable == null) return;
            if (openable.IsOpen)
            {
                openable.Close();
            }
            else
            {
                openable.Open();
            }
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
    }
    
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