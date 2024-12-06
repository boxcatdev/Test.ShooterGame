using PatchworkGames;
using TMPro;
using UnityEngine;

public class PointText : MonoBehaviour
{
    [Header("MoveScale")]
    [SerializeField] private Vector3 _startScale = Vector3.one;
    [SerializeField] private Vector3 _endScale = Vector3.one;
    [Space]
    [SerializeField] private float _endOffset = 1f;
    [Space]
    [SerializeField] private float _lifetime = 1f;

    [Header("Text")]
    [SerializeField] private TextMeshPro _3dText;

    private float _progress = 0f;
    private void OnEnable()
    {
        //StartText("+100");
    }
    private void Update()
    {
        _progress += Time.deltaTime;
        if(_progress >= _lifetime) EndText();

        float ratio = _progress / _lifetime;

        // scale
        float diff = _endScale.x - _startScale.x;
        float actual = _startScale.x + (ratio * diff);
        _3dText.transform.localScale = Vector3.one * actual;

        // position
        _3dText.transform.localPosition = new Vector3(0, ratio * _endOffset, 0);
    }
    public void StartText(string points)
    {
        // reset text position
        // change text value

        _3dText.transform.localPosition = Vector3.zero;
        _3dText.transform.localScale = _startScale;

        _3dText.text = "+" + points;

        _progress = 0f;
    }
    public void EndText()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
