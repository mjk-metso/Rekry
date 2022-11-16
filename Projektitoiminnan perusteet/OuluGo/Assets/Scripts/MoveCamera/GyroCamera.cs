using UnityEngine;
using UnityEngine.UI;

public class GyroCamera : MonoBehaviour
{
    private float rotationX = 30f;
    public float maxAngle = 70f;
    public float minAngle = 30f;
    private Quaternion correction = Quaternion.Euler(90f, 0f, 0f);
    private Vector2 startPos;
    private Vector2 direction;
    private Quaternion _lastgyro;

    public Toggle gyroCheckbox;
    public ManualCamera manualcontrol;

    /// <summary>
    /// tarkistetaan onko laitteessa GYRO sensori, ja jos on niin otetaan se k�ytt��n
    /// Jos ei, niin otetaan k�ytt��n joystickill� kameran ohjaileminen.
    /// </summary>
    private void Start()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            gyroCheckbox.isOn = false;
            this.enabled = false;
        }
        else
        {
            Input.gyro.enabled = false;
            Input.gyro.enabled = true;
            Input.compass.enabled = true;
            Input.gyro.updateInterval = 0.1f;
            _lastgyro = new Quaternion(0, 0, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        }
    }

    private void checkActive() {
        if (gyroCheckbox.isOn) {
            this.enabled = true;
            manualcontrol.enabled = false;
        } else {
            this.enabled = false;
            manualcontrol.enabled = true;
        }
    }

    private void Update()
    {
        checkActive();
        //restartataan gyro jos kolme sormea n�yt�ll�
        if (Input.touchCount == 3)
        {
            Input.gyro.enabled = false;
            Input.gyro.enabled = true;
        }
        //k��net��n kameraa laitteen gyro sensorin antamien arvojen mukaisesti.
        Quaternion gyroValue = new Quaternion(0, 0, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        gyroValue = correction * gyroValue;
        transform.rotation = Quaternion.RotateTowards(_lastgyro, gyroValue, 0.01f);
        transform.Rotate(-rotationX, 0f, 0f);
        _lastgyro = gyroValue;

        //Kaannetaan kameraa ylos/alas sormea liikuttamalla
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase) {
                    case TouchPhase.Began:
                        startPos = touch.position;
                        break;

                    case TouchPhase.Moved:
                        direction = touch.position - startPos;
                        if (direction.y > 0) { rotationX += 1f; }
                        if (direction.y < 0) { rotationX -= 1f; }
                        if (rotationX > maxAngle) { rotationX = maxAngle; }
                        if (rotationX < minAngle) { rotationX = minAngle; }
                        break;

                    case TouchPhase.Ended:
                        break;
            }
        }
    }
}
