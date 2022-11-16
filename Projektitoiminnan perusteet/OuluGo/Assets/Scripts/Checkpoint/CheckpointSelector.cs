using UnityEngine;
using UnityEngine.UI;
using static PointInfo;

/// <summary>
/// t�m�n avulla saadaan n�yt�lle n�kyviin checkpointista lis�tietoja, sit� painaisemalla.
/// </summary>
public class CheckpointSelector : MonoBehaviour
{
    private GameObject plane;
    private Text header;
    private Text description;
    private Text value;

    // Start is called before the first frame update
    void Start()
    {
        plane = this.transform.GetChild(0).gameObject;
        header = plane.transform.GetChild(0).GetComponent<Text>();
        description = plane.transform.GetChild(1).GetComponent<Text>();
        value = plane.transform.GetChild(2).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (plane.activeInHierarchy)
            {
                //jos ikkuna on n�kyviss� niin suljetaan se.
                plane.SetActive(false);
            }
            else
            {
                //jos ikkunaa ei n�kyviss�, niin tarkistetaan ett� osuttiinko sormella checkpointtiin..
                RaycastHit hit;
                Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    //Debug.Log($"{hit.transform.name} is clicked by mouse");
                    Checkpoint c = hit.transform.GetComponentInParent<Checkpoint>();
                    if (c != null)
                    {
                        //..jos osuttiin niin laitetaan sen tiedot k�ytt�j�n n�kyville.
                        ELocationName el = c.location;
                        header.text = el.ToString();
                        description.text = PointInfo.GetDescription(el);
                        value.text = $"Arvo: {PointInfo.GetValue(el)}";
                        plane.SetActive(true);
                    }
                }
            }
        }
    }
}
