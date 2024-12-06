using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private Camera _playerCam;

    public static Camera playerCamera;

    [Header("Zone 1 Targets")]
    [SerializeField] private List<Target> zone1Targets = new List<Target>();
    [SerializeField] private int zone1Count = 0;

    private void Awake()
    {
        playerCamera = _playerCam;

        zone1Count = zone1Targets.Count;
    }

    public void StartZone1()
    {

    }
    public void HitZone1()
    {

    }
}
