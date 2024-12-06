using PatchworkGames;
using System;
using UnityEngine;

public class GunTest : MonoBehaviour
{
    private InputHandler _input;

    [Header("Gun Settings")]
    [SerializeField] private Camera _playerCam;
    [SerializeField] private int _ammoCount = 10;
    public int ammoCount => _ammoCount;
    [SerializeField] private int _maxAmmoCount = 10;
    public int maxAmmoCount => _maxAmmoCount;
    [SerializeField] private float _reloadTime = 3f;

    private bool _canShoot = true;
    private bool _isReloading = false;
    [SerializeField]
    private float _reloadProgress;

    [Header("References")]
    [SerializeField] private Animator _gunAnimator;
    [SerializeField] private Transform _barrelEnd;
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem _hitPrefab;
    [SerializeField] private ParticleSystem _shootPrefab;

    // animations
    private int _animIDShoot;
    private int _animIDReloadStart;
    private int _animIDReloadEnd;

    // actions
    public Action<int> OnAmmoChanged = delegate { };

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }
    private void OnEnable()
    {
        _input.OnShootAction += TryShoot;
        _input.OnReloadAction += StartReload;
    }
    private void OnDisable()
    {
        _input.OnShootAction -= TryShoot;
        _input.OnReloadAction -= StartReload;
    }
    private void Start()
    {
        SetupAnimators();

        OnAmmoChanged?.Invoke(_ammoCount);
    }
    private void Update()
    {
        if(_isReloading)
        {
            _reloadProgress -= Time.deltaTime;
            if(_reloadProgress <= 0)
            {
                EndReload();
            }
        }
    }
    private void SetupAnimators()
    {
        _animIDShoot = Animator.StringToHash("Shoot");
        _animIDReloadStart = Animator.StringToHash("ReloadStart");
        _animIDReloadEnd = Animator.StringToHash("ReloadEnd");
    }
    private void TryShoot()
    {
        if(_canShoot == false) return;
        if(_ammoCount <= 0) return;

        // spawn shoot particles every time
        // also do shoot animation
        GameObject shootParticles = ObjectPoolManager.SpawnObject(_shootPrefab.gameObject, _barrelEnd.position, Quaternion.identity);
        shootParticles.transform.SetParent(_gunAnimator.transform);
        shootParticles.transform.forward = _barrelEnd.forward;
        
        // do gun logic
        _gunAnimator.SetTrigger(_animIDShoot);
        _canShoot = false;
        _ammoCount--;

        OnAmmoChanged?.Invoke(_ammoCount);

        // check if something was hit
        Collider collider = Utility.GetMouseHitCollider3D(_playerCam);
        RaycastHit hit = Utility.GetMouseHit3D(_playerCam);
        if (collider != null)
        {
            Debug.Log($"Hit {collider}");

            // spawn hit particles
            GameObject particles = ObjectPoolManager.SpawnObject(_hitPrefab.gameObject, hit.point, Quaternion.identity);
            //particles.transform.SetParent(hit.transform);
            particles.transform.forward = hit.normal;

            // do hit logic
            ITarget target = collider.GetComponent<ITarget>();
            if (target != null)
            {
                target.HitTarget();
            }
        }

        // check if need to reload
        if(_ammoCount <= 0)
        {
            StartReload();
        }
    }
    private void StartReload()
    {
        _isReloading = true;
        _canShoot = false;
        _reloadProgress = _reloadTime;

        _gunAnimator.SetTrigger(_animIDReloadStart);
    }
    private void EndReload()
    {
        _gunAnimator.SetTrigger(_animIDReloadEnd);

        _isReloading = false;
        _canShoot = true;
        _ammoCount = _maxAmmoCount;

        OnAmmoChanged?.Invoke(_ammoCount);
    }

    public void EndShootAnimation()
    {
        _canShoot = true;
    }
}
