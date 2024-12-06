using PatchworkGames;
using System;
using UnityEngine;

public class Target : MonoBehaviour, ITarget
{
    private TargetManager targetManager;

    [Header("Effects")]
    [SerializeField] private PointText _3dText;
    [SerializeField] private Transform _textSpawnPoint;

    [Header("Settings")]
    [SerializeField] private bool _respawn = false;
    [SerializeField] private float _respawnTime = 5f;
    [SerializeField] private int _pointValue = 10;
    public int pointValue => _pointValue;

    public Action OnHitTarget = delegate { };

    private void Awake()
    {
        targetManager = FindAnyObjectByType<TargetManager>();
    }
    public void HitTarget()
    {
        ScoreManager.Instance.AddPoints(pointValue);

        Debug.Log($"Hit {_pointValue} points");

        ResetAndDisable();
    }

    private void ResetAndDisable()
    {
        // reset settings

        // spawn 3D text
        Vector3 billboardDirection = GameManager.playerCamera.transform.position - _textSpawnPoint.position;
        PointText text = ObjectPoolManager.SpawnObject(_3dText.gameObject, _textSpawnPoint.position, _textSpawnPoint.rotation, ObjectPoolManager.PoolType.Gameobject).GetComponent<PointText>();
        text.transform.SetParent(null);
        text.transform.forward = billboardDirection;
        text.StartText(pointValue.ToString());

        // if respawn enabled use target manager
        if (_respawn)
        {
            targetManager.RespawnTarget(this, _respawnTime);
        }

        // disable gameobject
        gameObject.SetActive(false);
    }
}
