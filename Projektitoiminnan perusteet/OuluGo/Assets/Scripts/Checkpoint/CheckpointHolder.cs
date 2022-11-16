using Google.Maps;
using System;
using System.Collections.Generic;
using UnityEngine;
using static PointInfo;

/// <summary>
/// luo checkpointit joita ei olla viel� ker�tty pelimaailmaan.
/// </summary>
public class CheckpointHolder : MonoBehaviour
{
    /// <summary>
    /// The <see cref="MapsService"/> is the entry point to communicate with to the Maps SDK for
    /// Unity. It provides apis to load map regions, and dispatches events throughout the loading
    /// process. These events, in particular WillCreate/DidCreate events provide a finer control on
    /// each loaded map feature.
    /// </summary>
    [Tooltip("Required maps service (used to communicate with the Maps plugin).")]
    public static CheckpointHolder Instance;
    public MapsService MapsService;
    public List<Checkpoint> checkpoints;
    public GameObject checkpointPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCheckpointGroup("Yliopisto", typeof(EYliopistolla));
        SpawnCheckpointGroup("Keskusta", typeof(EKeskustassa));
        SpawnCheckpointGroup("Syrj�iset", typeof(ESyrjaiset));
    }

    void Awake() {
        Instance = this;
    }
    
    /// <summary>
    /// luo m��r�tyn kategorian checkpointit pelimaailmaan
    /// </summary>
    /// <param name="name">give a name(in unitys hierarcy) for the parent object of these checkpoints of this category.</param>
    /// <param name="enumerator">The checkpoints that will be spawned</param>
    private void SpawnCheckpointGroup(string name, Type enumerator)
    {
        List<ELocationName> points = new List<ELocationName>();
        foreach(PointData point in SaveSystem.LoadProgress().GetPoints())
        {
            points.Add(point.GetLocation());
        }
        GameObject checkpointGroup = new GameObject(name);
        checkpointGroup.transform.parent = this.transform;
        foreach (ELocationName loc in Enum.GetValues(enumerator))
        {
            if (points.Count > 0 && points.Contains(loc))
            {
                //Debug.Log("already collected " + loc.ToString());
                continue;
            }
            Checkpoint newCp = Instantiate(checkpointPrefab, checkpointGroup.transform).GetComponent<Checkpoint>();
            newCp.name = loc.ToString();
            newCp.MapsService = MapsService;
            newCp.location = loc;
            checkpoints.Add(newCp);
        }
    }
}
