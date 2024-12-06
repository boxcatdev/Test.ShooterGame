using UnityEngine;

public interface IInteractable
{
    void TriggerInteraction(Transform transform, bool value);
    void LeaveInteraction();
    string GetInteractText();
    Transform GetTransform();
    PGInput GetInteractionInput();
    bool CanInteract();
    void DisableInteractions();
    void EnableInteractions();
}
