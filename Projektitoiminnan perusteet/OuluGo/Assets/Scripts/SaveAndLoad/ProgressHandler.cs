using TMPro;
using UnityEngine;

/// <summary>
/// t‰m‰ scripti/komponentti on pelaajan sijaintia kuvaavassa gameObjectissa ja huolehtii siit‰ mit‰
/// tapahtuu kun se osuu checkpointtiin.
/// </summary>
public class ProgressHandler : MonoBehaviour
{
    private Progress progress;
    public TMP_Text scoreTxt;
    // Start is called before the first frame update
    void Start()
    {
        progress = SaveSystem.LoadProgress();
        UpdateScoreTxt();
    }

    /// <summary>
    /// tarkistaa ett‰ onko osuttu kappaleeseen joka sis‰lt‰‰ Checkpoint scriptin/komponentin,
    /// ja jos ollaan niin t‰m‰ ker‰‰ sen ja tallentaa tiedon muistiin ja sen j‰lkeen k‰y p‰ivitt‰m‰ss‰ 
    /// k‰ytt‰j‰n pisteet n‰kyvill‰ olevaan tekstielementtiin.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("triggereEnter " + other.gameObject.name);
        Checkpoint c = other.GetComponentInParent<Checkpoint>();
        if (c != null)
        {
            progress.AddPoint(c.location);
            Destroy(c.gameObject);
            SaveSystem.SaveProgress(progress);
        }
        UpdateScoreTxt();
    }

    /// <summary>
    /// p‰ivitt‰‰ k‰ytt‰j‰lle n‰kyviin sen tiedon kuinka monta pistett‰ h‰n on saanut t‰h‰n menness‰ ker‰‰mist‰‰n 
    /// checkpointeista tallennus tiedoston mukaan.
    /// </summary>
    private void UpdateScoreTxt()
    {
        int score = 0;
        Progress p = SaveSystem.LoadProgress();
        foreach (PointData pd in p.GetPoints())
        {
            score += PointInfo.GetValue(pd.GetLocation());
        }
        scoreTxt.text = $"Pisteet: {score}";
    }
}
