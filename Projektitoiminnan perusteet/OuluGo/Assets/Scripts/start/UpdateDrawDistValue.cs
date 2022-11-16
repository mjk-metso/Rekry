using UnityEngine;
using UnityEngine.UI;

public class UpdateDrawDistValue : MonoBehaviour
{
    public Slider drawDistanceSlider;
    private Text textField;

    /// <summary>
    /// hakee t‰m‰n scriptin gameobjektista Text komponentin/skriptin textField muuttujaan.
    /// ja laittaa siihen n‰kym‰‰n arvon Slider komponentista/scriptist‰.
    /// </summary>
    private void Start()
    {
        textField = GetComponent<Text>();
        UpdateValue();
    }

    /// <summary>
    /// asettaa textField tekstiksi Slider komponentin/scriptin arvon.
    /// </summary>
    public void UpdateValue()
    {
        textField.text = $"{drawDistanceSlider.value}00m";
    }
}
