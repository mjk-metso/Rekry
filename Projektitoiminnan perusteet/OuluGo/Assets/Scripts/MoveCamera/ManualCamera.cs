using UnityEngine;
using UnityEngine.UI;

public class ManualCamera : MonoBehaviour
{
    private float rotationX = 30f;
    public float maxAngle = 60f;
    public float minAngle = 20f;
    private Vector2 startPos;
    private Vector2 direction;

    public Toggle gyroCheckbox;
    public GyroCamera gyrocontrol;

    /// <summary>
    /// tarkistetaan onko laitteessa GYRO sensori, ja jos on niin otetaan se k�ytt��n
    /// Jos ei, niin otetaan k�ytt��n joystickill� kameran ohjaileminen.
    /// </summary>
    private void Start()
    {
        checkActive();
    }

    private void checkActive() {
        if (gyroCheckbox.isOn) {
            this.enabled = false;
            gyrocontrol.enabled = true;
        } else {
            this.enabled = true;
            gyrocontrol.enabled = false;
        }
    }

    private void Update()
    {
        checkActive();
        //Kaannetaan kameraa sormea liikuttamalla
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase) {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    direction = touch.position - startPos;
                    if (direction.y > 0) { rotationX -= 1f; }
                    if (direction.y < 0) { rotationX += 1f; }
                    if (direction.x > 0) { transform.Rotate(0f, 1f, 0f); }
                    if (direction.x < 0) { transform.Rotate(0f, -1f, 0f);}
                    if (rotationX > maxAngle) { rotationX = maxAngle; }
                    if (rotationX < minAngle) { rotationX = minAngle; }
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }

        float rotationY = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        transform.Rotate(rotationX, 0f, 0f);
    }
}
