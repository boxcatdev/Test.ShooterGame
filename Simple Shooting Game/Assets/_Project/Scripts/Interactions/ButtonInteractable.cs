using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractable : BaseInteractable
{
    [Header("Events")]
    public UnityEvent OnButtonPress;

    public override void LeaveInteraction()
    {
        base.LeaveInteraction();
    }

    protected override void DoInteractableAction(bool value)
    {
        base.DoInteractableAction(value);

        if (value)
        {
            OnButtonPress?.Invoke();
        }
    }
}
