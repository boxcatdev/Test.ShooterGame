using UnityEngine;

public class GunMesh : MonoBehaviour
{
    private GunTest gun;

    private void Awake()
    {
        gun = GetComponentInParent<GunTest>();
    }

    public void EndShootAnimation()
    {
        gun.EndShootAnimation();
    }
}
