using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Google.Maps.Demos.Utilities
{

    /// <summary>
    /// Oma versioni JoustickController classista joka on osa 'Maps SDK for Unity':‰
    /// </summary>
    public class JoystickController2 : MonoBehaviour, IDragHandler, IPointerUpHandler,
      IPointerDownHandler
    {

        #region properties
        // Joystick bounds
        public Image Background;
        // Camera Rig to move
        public Transform CameraRig;
        // Direction to follow
        public Vector3 InputDirection;
        // Lever knob to control rotations
        public Image Lever;
        // Maximum Y rotation speed
        public float MaxRotationSpeedY = 50f;
        // Maximum X rotation speed
        public float MaxRotationSpeedX = 45f;

        #endregion

        /// <summary>
        /// On start, set joystick defaults.
        /// </summary>
        private void Start()
        {
            InputDirection = Vector3.zero;
        }

        /// <summary>
        /// On Updates, adjust the position and CW, CCW rotations of the Rig.
        ///
        /// </summary>
        private void Update()
        {
            if (InputDirection.magnitude != 0 && CameraRig != null)
            {
                //k‰‰nnell‰‰n kameraa ylˆs tai alas
                float rotationDirection = 1f;
                float angle = Vector3.Angle(InputDirection, Vector3.right);

                if (angle > 90f) rotationDirection = -1f;
                if (angle < 80f || angle > 100)
                {
                    CameraRig.transform.Rotate(
                        0f, 
                        rotationDirection * MaxRotationSpeedY * InputDirection.magnitude * Time.deltaTime, 
                        0f, 
                        Space.World);
                }
                else
                {
                    //k‰‰nnell‰‰n kameraa vasemmalle tai oikealle
                    rotationDirection = (Vector3.Angle(InputDirection, Vector3.up) < 90f) ? -1f : 1f;
                    CameraRig.transform.RotateAround(
                      CameraRig.transform.position,
                      CameraRig.GetChild(0).transform.right,
                      rotationDirection * MaxRotationSpeedX * InputDirection.magnitude * Time.deltaTime);
                }
            }
        }

        #region event listeners
        /// <summary>
        /// Implements the IDragHandler interface.
        /// The function converts the drag of the joystick knob on the UI overlay
        /// to a direction vector in worldspace that can be applied to our target.
        /// </summary>
        /// <param name="ped">The pointer event data</param>
        public void OnDrag(PointerEventData ped)
        {
            // Current position
            var pos = Vector2.zero;
            var rect = Background.rectTransform;

            // Move the target based on the Lever's position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
              rect,
              ped.position,
              ped.pressEventCamera,
              out pos);

            pos.x = pos.x / rect.sizeDelta.x;
            pos.y = pos.y / rect.sizeDelta.y;

            InputDirection = new Vector3(pos.x, pos.y, 0f);
            InputDirection = InputDirection.magnitude > 1 ? InputDirection.normalized : InputDirection;

            Lever.rectTransform.anchoredPosition = new Vector3(
              InputDirection.x * (rect.sizeDelta.x / 3),
              InputDirection.y * rect.sizeDelta.y / 3);
        }

        /// <summary>
        /// Implements the IPointerUpHandler interface.
        /// Applies changes in similar ways to the OnDrag function.
        /// </summary>
        /// <param name="ped">The pointer event data</param>
        public void OnPointerDown(PointerEventData ped)
        {
            OnDrag(ped);
        }

        /// <summary>
        /// Implements the IPointerDownHandler interface.
        /// Resets the position of the joystick knob and the direction vector.
        /// </summary>
        /// <param name="ped"></param>
        public void OnPointerUp(PointerEventData ped)
        {
            Lever.rectTransform.anchoredPosition = Vector3.zero;
            InputDirection = Vector3.zero;
        }
        #endregion
    }
}
