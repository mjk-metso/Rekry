using UnityEngine;
using UnityEngine.UI;

public class toggleGyro : MonoBehaviour
{
    [SerializeField]
    private GyroCamera gyro;
    [SerializeField]
    private GameObject manualControls;

    /// <summary>
    /// jos tämän scriptin/komponentin käynnistyessä gyro liikkuminen on päällä, laiteaan joystickillä ohjailu pois päältä
    /// </summary>
    private void Start()
    {
        if (gyro.enabled)
        {
            manualControls.SetActive(false);
        }
    }

    /// <summary>
    /// Enables / Disables the <see cref="FloatingOriginUpdater"/> script.
    /// Tämä siis on pelistä löytyvän "GYRO" checkbox asetuksen sisältämä toiminnallisuus.
    /// </summary>
    /// <param name="change">The checkbox element</param>
    public void OnGyroSelected(Toggle change)
    {
        gyro.enabled = change.isOn;
        manualControls.SetActive(!change.isOn);
        this.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
    }
}
