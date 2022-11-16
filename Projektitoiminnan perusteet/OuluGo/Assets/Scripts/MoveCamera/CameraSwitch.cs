using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera playerCam;
    public Camera mapCam;

    public float OrthoZoomSpeed = 0.5f;
    public float MaxOrthographicSize = 2000f;
    public float MinOrthographicSize = 200f;

    // Start is called before the first frame update
    void Start() {
        playerCam.enabled = true;
        mapCam.enabled = false;
    }

    private void Update() {
      if (Input.touchCount == 2) {
          // Store both touches.
          var touchZero = Input.GetTouch(0);
          var touchOne = Input.GetTouch(1);

          // Find the position in the previous frame of each touch.
          Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
          Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

          // Find the magnitude of the vector (the distance) between the touches in each frame.
          float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
          float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

          // Find the difference in the distances between each frame.
          float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

          // If the camera is orthographic...
          if (mapCam.enabled) {
              // ... change the orthographic size based on the change in distance between the touches.
              mapCam.orthographicSize += deltaMagnitudeDiff * OrthoZoomSpeed;

              // Make sure the orthographic size is clamped.
              mapCam.orthographicSize = Mathf.Clamp(mapCam.orthographicSize,
                MinOrthographicSize, MaxOrthographicSize);
            }
        }
    }

    public void SwapCam() {
        if (playerCam.enabled) {
          playerCam.enabled = false;
          mapCam.enabled = true;
        } else {
          mapCam.enabled = false;
          playerCam.enabled = true;
        }
    }
}
