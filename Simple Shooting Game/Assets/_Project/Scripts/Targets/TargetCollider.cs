using UnityEngine;

public class TargetCollider : MonoBehaviour, ITarget
{
    public Target parentTarget;

    public void HitParent()
    {
        if(parentTarget != null)
        {
            parentTarget.HitTarget();
        }
        else
        {
            Debug.LogError("Missing Target");
        }
    }

    public void HitTarget()
    {
        HitParent();
    }
}
