using UnityEngine;

[CreateAssetMenu(fileName = "UIDictionarySO", menuName = "Scriptable Objects/UIDictionarySO")]
public class UIDictionarySO : ScriptableObject
{
    [Header("Move Sprites")]
    [SerializeField] private Sprite _moveKeyboard;
    [SerializeField] private Sprite _moveGamepad;

    [Header("Look Sprites")]
    [SerializeField] private Sprite _lookKeyboard;
    [SerializeField] private Sprite _lookGamepad;

    [Header("Shoot Sprites")]
    [SerializeField] private Sprite _shootKeyboard;
    [SerializeField] private Sprite _shootGamepad;

    [Header("Reload Sprites")]
    [SerializeField] private Sprite _reloadKeyboard;
    [SerializeField] private Sprite _reloadGamepad;

    [Header("Use Sprites")]
    [SerializeField] private Sprite _useKeyboard;
    [SerializeField] private Sprite _useGamepad;


    public Sprite GetSprite(PGInput inputType, bool isGamepad)
    {
        switch (inputType)
        {
            default: 
                return null;
            case PGInput.Move:
                return isGamepad ? _moveGamepad : _moveKeyboard;
            case PGInput.Look:
                return isGamepad ? _lookGamepad : _lookKeyboard;
            case PGInput.Reload:
                return isGamepad ? _reloadGamepad : _reloadKeyboard;
            case PGInput.Shoot:
                return isGamepad ? _shootGamepad : _shootKeyboard;
            case PGInput.Use:
                return isGamepad ? _useGamepad : _useKeyboard;
        }
    }
}

public enum PGInput { Move, Look, Shoot, Reload, Use}
