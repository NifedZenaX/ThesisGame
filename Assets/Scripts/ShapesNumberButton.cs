using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShapesNumberButton : MonoBehaviour
{
    [SerializeField] private Image _highlighter;
    public Image highlighter { get => _highlighter; }

    [SerializeField] private Button _button;
    public Button button { get => _button; }

    [SerializeField] private TextMeshProUGUI _buttonText;
    public TextMeshProUGUI btnText { get => _buttonText; }
}
