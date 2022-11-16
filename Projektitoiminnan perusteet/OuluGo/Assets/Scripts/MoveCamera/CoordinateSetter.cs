using Google.Maps;
using Google.Maps.Coord;
using Google.Maps.Examples;
using UnityEngine;

/// <summary>
/// p�ivitt�� pelaajan sijaintia 3d maailmassa.
/// </summary>
public class CoordinateSetter : MonoBehaviour
{
    [SerializeField]
    private BaseMapLoader bml;

    [SerializeField]
    private GameObject contextObject;

    [SerializeField]
    private GameObject cameraRig;

    [SerializeField]
    private GameObject sijaintiPylvas;

    [Range(0f, 500f)]
    public float viewHeight = 200f;

    /// <summary>
    /// The <see cref="MapsService"/> is the entry point to communicate with to the Maps SDK for
    /// Unity. It provides apis to load map regions, and dispatches events throughout the loading
    /// process. These events, in particular WillCreate/DidCreate events provide a finer control on
    /// each loaded map feature.
    /// </summary>
    [Tooltip("Required maps service (used to communicate with the Maps plugin).")]
    public MapsService MapsService;

    private const float yoLat = 65.059f;
    private const float yoLng = 25.466f;

    private const float kaupLat = 65.014f;
    private const float kaupLng = 25.472f;

    /// <summary>
    /// koitetaan laittaa laitteen gps toiminnallisuus p��lle. ja asetetaan draw distance
    /// sen mukan miksi k�ytt�j� sen aloitusn�kym�ss� m��ritti.
    /// </summary>
    void Awake()
    {
        Input.location.Start(0.1f, 0.1f);
        bml.MaxDistance = SaveSystem.LoadSettings().drawDistance*100;
    }

    /// <summary>
    /// asettelee kameran sek� rigin rotaation halutulla tavalla ja asettaa sen sek� sijaintia kuvaavan gameObjectin position
    /// </summary>
    private void Start()
    {
        Camera camera = cameraRig.GetComponentInChildren<Camera>();
        camera.transform.localRotation = Quaternion.identity;
        //cameraRig.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
        SetPosition();
    }

    [SerializeField]
    private float timer = 1f;
    private float timerTime = 0f;
    /// <summary>
    /// p�ivitet��n pelaajan sijainti timer sekunttim��r�n v�lein.
    /// </summary>
    void Update()
    {
        if (timerTime < 0)
        {
            timerTime = timer;
            SetPosition();
        }
        timerTime -= Time.deltaTime;
    }

    /// <summary>
    /// sijainnin p�ivitt�minen
    /// </summary>
    private void SetPosition()
    {
#if UNITY_EDITOR
        //yliopiston koordinaatteihin jos debugataan koneella
        LatLng playerCoord = new LatLng(yoLat, yoLng);
#elif (UNITY_ANDROID || UNITY_IOS)
        //laitteen koordinaatteihin jos laite on android tai ios
        if (Input.location.status == LocationServiceStatus.Stopped)
        {
            //jos gps toiminnallisuudet on jostain sysyt� pois p�lt�, koitetaan k�ynnist�� ne
            Input.location.Start(0.1f, 0.1f);
        }
        LatLng playerCoord = new LatLng(Input.location.lastData.latitude, Input.location.lastData.longitude);
#endif
        //muutetaan koordinaatit vektoriksi ja asetetaan kamera sen osoittamaan sijaintiin
        Vector3 playerPos = MapsService.Projection.FromLatLngToVector3(playerCoord);
        cameraRig.transform.position =
            new Vector3(
                playerPos.x,
                viewHeight,
                playerPos.z
                );
        sijaintiPylvas.transform.position =
            new Vector3(
                playerPos.x,
                0f,
                playerPos.z
                );
    }
}
