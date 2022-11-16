using UnityEngine;
using UnityEngine.UI;

public class toggleGyro : MonoBehaviour
{
    [SerializeField]
    private GyroCamera gyro;
    [SerializeField]
    private GameObject manualControls;

    /// <summary>
    /// jos t�m�n scriptin/komponentin k�ynnistyess� gyro liikkuminen on p��ll�, laiteaan joystickill� ohjailu pois p��lt�
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
    /// T�m� siis on pelist� l�ytyv�n "GYRO" checkbox asetuksen sis�lt�m� toiminnallisuus.
    /// </summary>
    /// <param name="change">The checkbox element</param>
    public void OnGyroSelected(Toggle change)
    {
        gyro.enabled = change.isOn;
        manualControls.SetActive(!change.isOn);
        this.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
    }
}
