using PatchworkGames;
using System;
using UnityEngine;
using UnityEngine.Events;

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

    private bool _isActive = true;

    //[Header("Animation")]
    //[SerializeField] private Transform _scaleParent;

    private Animator _animator;

    private int _animIDStart;
    private int _animIDEnd;

    // Actions
    public Action OnHitTarget = delegate { };

    [Header("Events")]
    public UnityEvent OnTargetHit;

    private void Awake()
    {
        targetManager = FindAnyObjectByType<TargetManager>();
        _animator = GetComponent<Animator>();

        SetupAnimations();
    }
    private void OnEnable()
    {
        StartingAnim();
    }
    private void Start()
    {

    }
    private void SetupAnimations()
    {
        _animIDStart = Animator.StringToHash("TargetStart");
        _animIDEnd = Animator.StringToHash("TargetEnd");
    }
    public void HitTarget()
    {
        if (_isActive == false) return;

        ScoreManager.Instance.AddPoints(pointValue);

        Debug.Log($"Hit {_pointValue} points");

        OnTargetHit?.Invoke();

        Spawn3DText();

        //ResetAndDisable();
        _isActive = false;
        EndAnim();
    }
    private void Spawn3DText()
    {
        // spawn 3D text
        Vector3 billboardDirection = GameManager.playerCamera.transform.position - _textSpawnPoint.position;
        PointText text = ObjectPoolManager.SpawnObject(_3dText.gameObject, _textSpawnPoint.position, _textSpawnPoint.rotation, ObjectPoolManager.PoolType.Gameobject).GetComponent<PointText>();
        text.transform.SetParent(null);
        text.transform.forward = billboardDirection;
        text.StartText(pointValue.ToString());
    }
    private void ResetAndDisable()
    {
        // reset settings

        // if respawn enabled use target manager
        if (_respawn)
        {
            targetManager.RespawnTarget(this, _respawnTime);
        }

        // disable gameobject
        gameObject.SetActive(false);
    }

    #region Animations
    private void StartingAnim()
    {
        _animator.enabled = true;
        _animator.SetTrigger(_animIDStart);
    }
    public void StartAnimOver()
    {
        _isActive = true;
        _animator.enabled = false;
    }
    public void EndAnim()
    {
        _animator.enabled = true;
        _animator.SetTrigger(_animIDEnd);
    }
    public void EndAnimOver()
    {
        _animator.enabled = false;
        ResetAndDisable();
    }
    #endregion
}
