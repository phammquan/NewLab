// File: Interactable.cs
using UnityEngine;

public interface Interactable
{
    bool canInteract();
    void Use(RaycastHit hitInfo);
    void OnPickup();
    void OnDrop();

    bool IsValidPlacement(RaycastHit hitInfo);
    string GetTextTutorial();
}