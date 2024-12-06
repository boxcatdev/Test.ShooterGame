using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;

public class PlayerInteract : MonoBehaviour
{
    #region Variables & References
    //events
    public event Action<bool> OnGetInteractable;
    public event Action<bool> OnPressInteractionKey;

    //references
    private InputHandler _input;

    [Header("Settings")]
    [SerializeField] private Transform _interactionPosition;
    [SerializeField] float _interactionRange = 2f;

    //object player is currently interacting with
    public IInteractable currentInteractable { get; private set; }

    //player interaction state
    public bool IsInteracting { get; private set; }

    //private variables
    private bool _isButtonDown;

    #endregion

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }
    private void OnEnable()
    {
        _input.OnUseAction += UseStart;
    }
    private void OnDisable()
    {
        _input.OnUseAction -= UseStart;
    }

    private void Update()
    {
        #region Interaction Physics Check
        if (_interactionPosition == null) return;

        IInteractable interactable = GetInteractableObject();
        if (interactable != null && currentInteractable != interactable)
        {
            currentInteractable = interactable;
            OnGetInteractable?.Invoke(true);
            //Debug.Log("OnGetInteractable True");

        }
        else if (interactable == null && currentInteractable != null)
        {
            currentInteractable.LeaveInteraction();
            currentInteractable = null;

            OnGetInteractable?.Invoke(false);
            //Debug.Log("OnGetInteractable False");

            //update _isInteracting if player leaves interaction distance
            if (IsInteracting == true) IsInteracting = false;
        }
        #endregion

        // undo use bool
        if (IsInteracting)
        {
            UseStop();

            /*if (_input.use == false)
            {
                UseStop();
            }*/
        }

        #region Button Press Events
        //keyboard press
        /*if (Keyboard.current.fKey.wasPressedThisFrame && currentInteractable != null)
        {
            IsInteracting = true;
            if (currentInteractable.CanInteract())
            {
                OnPressInteractionKey?.Invoke(true);
                currentInteractable.TriggerInteraction(transform, IsInteracting);
            }
            else
            {
                OnPressInteractionKey?.Invoke(false);
            }
        }
        if (Keyboard.current.fKey.wasReleasedThisFrame && currentInteractable != null)
        {
            IsInteracting = false;

            OnPressInteractionKey?.Invoke(false);
            if (currentInteractable.CanInteract())
            {
                currentInteractable.TriggerInteraction(transform, IsInteracting);
            }
        }*/

        #endregion
    }
    private void UseStart()
    {
        if (currentInteractable == null) return;

        IsInteracting = true;
        if (currentInteractable.CanInteract())
        {
            OnPressInteractionKey?.Invoke(true);
            currentInteractable.TriggerInteraction(transform, IsInteracting);
        }
        else
        {
            OnPressInteractionKey?.Invoke(false);
        }
    }
    private void UseStop()
    {
        if (currentInteractable == null) return;

        IsInteracting = false;

        if (currentInteractable.CanInteract())
        {
            currentInteractable.TriggerInteraction(transform, IsInteracting);
        }

        OnPressInteractionKey?.Invoke(false);
    }

    public IInteractable GetInteractableObject()
    {

        List<IInteractable> interactableList = new List<IInteractable>();

        //put all interactables in range in a list
        Collider[] colliders = Physics.OverlapSphere(_interactionPosition.position, _interactionRange);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                interactableList.Add(interactable);
            }
        }

        //find the closest from the list
        IInteractable closestInteractable = null;
        foreach (var interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {

                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }

        //return the closest
        return closestInteractable;
    }

    private void OnDrawGizmosSelected()
    {
        if (_interactionPosition == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_interactionPosition.position, _interactionRange);
    }
}
