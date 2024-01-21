using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WireModuleComponents : MonoBehaviour
{
    public List<Image> wires;
    public List<Button> buttons;
    public List<TextMeshProUGUI> buttonTexts;

    public static Dictionary<ColorEnum, Color> colorDict = new Dictionary<ColorEnum, Color> {
        { ColorEnum.Blue, new Color(0f, 0f, 1f)},
        { ColorEnum.Red, new Color(1f, 0f, 0f)},
        { ColorEnum.Green, new Color(0f, 1f, 0f)} };
}
