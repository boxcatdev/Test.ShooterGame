using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
//using static Unity.Cinemachine.InputAxisControllerBase<T>;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputHandler))]
public class FPSController : MonoBehaviour
{
    private InputHandler _input;
    private CharacterController _character;
    private Animator _animator;

    public const float PI_MULT = 57.29f;
    private const float _threshold = 0.01f;


    [Header("Movement")]
    [SerializeField] private float _playerSpeed = 5f;
    [Space]
    [SerializeField] private bool _canMove = true;

    private float _targetSpeed = 0f;

    [Header("Camera")]
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _rotationSpeed = 1.0f;

    [SerializeField] private float _topClamp = 90f;
    [SerializeField] private float _bottomClamp = -90f;

    private float _cineTargetPitch;
    private float _rotationVelocity;



    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _character = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        MoveUpdate();
    }
    private void LateUpdate()
    {
        LookUpdate();
    }

    private void MoveUpdate()
    {
        if(_canMove == false) return;

        #region Free Cam
        /*//float targetSpeed = 0f;
        Vector3 adjMove = new Vector3(_input.move.x, 0, _input.move.y).normalized;

        if (!adjMove.Equals(Vector3.zero))
        {
            // move in direction of input
            float targetSpeed = _playerSpeed;
            _targetSpeed = Mathf.Lerp(_targetSpeed, targetSpeed, 5f * Time.fixedDeltaTime);

            _character.Move(adjMove * _targetSpeed * Time.fixedDeltaTime);
            //_animator.SetFloat(_animIDSpeed, _targetSpeed);

            // rotate in direction of movement
            *//*float targetRotation = Mathf.Atan2(adjMove.x, adjMove.z) * PI_MULT;
            targetRotation = Mathf.Round(targetRotation);
            Quaternion targetQ = Quaternion.Euler(0.0f, targetRotation, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQ, 20f * 0.01f);*//*

            // grounded check
            //_isGrounded = _controller.isGrounded;
        }
        else
        {
            _targetSpeed = Mathf.Lerp(_targetSpeed, 0f, 10f * Time.fixedDeltaTime);
            //_animator.SetFloat(_animIDSpeed, _targetSpeed);

            //float animSpeed = _animator.GetFloat(_animIDSpeed);
            //float targetSpeed = Mathf.Lerp(animSpeed, 0, 2f * Time.fixedDeltaTime);
            //_animator.SetFloat(_animIDSpeed, 0);
        }*/
        #endregion

        float targetSpeed = _playerSpeed;

        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_character.velocity.x, 0.0f, _character.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.isGamepad ? _input.move.magnitude : 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _targetSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.fixedDeltaTime * 5f);
            _targetSpeed = Mathf.Round(_targetSpeed * 1000f) / 1000f;
        }
        else
        {
            _targetSpeed = targetSpeed;
        }

        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        // if there is a move input rotate player when the player is moving
        if (_input.move != Vector2.zero)
        {
            // move
            inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
        }

        // move the player
        _character.Move(inputDirection.normalized * (_targetSpeed * Time.deltaTime) + new Vector3(0.0f, 0f, 0.0f) * Time.fixedDeltaTime);

    }
    private void LookUpdate()
    {
        // if there is an input
        if (_input.look.sqrMagnitude >= _threshold)
        {
            float deltaTimeMultiplier = _input.isGamepad ? 1.5f : 1.0f;

            _cineTargetPitch += _input.look.y * _rotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = _input.look.x * _rotationSpeed * deltaTimeMultiplier;

            // clamp pitch rotation
            _cineTargetPitch = ClampAngle(_cineTargetPitch, _bottomClamp, _topClamp);

            // Update Cinemachine camera target pitch
            _cameraTarget.transform.localRotation = Quaternion.Euler(-_cineTargetPitch, 0.0f, 0.0f);

            // rotate the player left and right
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
