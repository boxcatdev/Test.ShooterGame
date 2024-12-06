using UnityEngine;

public class Spin : MonoBehaviour
{
    [Header("Spin Settings")]
    [SerializeField] private float _spinSpeed = 10f;
    [SerializeField] private bool _xAxis;
    [SerializeField] private bool _yAxis;
    [SerializeField] private bool _zAxis;

    private void FixedUpdate()
    {
        if (_xAxis) transform.Rotate(transform.right * _spinSpeed * Time.fixedDeltaTime);
        if (_yAxis) transform.Rotate(transform.up * _spinSpeed * Time.fixedDeltaTime);
        if (_zAxis) transform.Rotate(transform.forward * _spinSpeed * Time.fixedDeltaTime);
    }
}
