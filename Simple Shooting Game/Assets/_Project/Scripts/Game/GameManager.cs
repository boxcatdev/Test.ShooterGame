using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Camera _playerCam;

    public static Camera playerCamera;

    private void Awake()
    {
        playerCamera = _playerCam;
    }
}
