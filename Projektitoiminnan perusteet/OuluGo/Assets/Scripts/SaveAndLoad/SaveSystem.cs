using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using static PointInfo;
using System;

/// <summary>
/// Staattinen luokka joka sis‰lt‰‰ toiminnallisuudet jolla voidaan ladata ja
/// tallentaa asetuksia sek‰ tietoa ker‰tyist‰ cehckpointeista.
/// </summary>
public static class SaveSystem
{
    private static string settingsPath = Application.persistentDataPath + "/oulugo.settings";
    private static SettingsData currentSettingsSave;

    /// <summary>
    /// tallennetaansyˆtetyt arvot asetukset sis‰lt‰v‰‰n tiedostoon laitteen muistiin.
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="areaValue">0=Yliopisto 1=Kaupunki</param>
    /// <param name="drawDistance">for example value 20 will be 2000m </param>
    /// <returns></returns>
    public static List<string> SaveSettings(string groupName, int areaValue, int drawDistance)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(settingsPath, FileMode.Create);

        //ei tehd‰ kokonaan uutta SettingsData olioa koska haluamme s‰ilyt‰‰ vanhan sis‰ll‰ tallessa olevan nimihistorian.
        currentSettingsSave.UpdateGroupName(groupName);
        currentSettingsSave.areaValue = areaValue;
        currentSettingsSave.drawDistance = drawDistance;
        formatter.Serialize(stream, currentSettingsSave);
        stream.Close();
        return currentSettingsSave.GetNameHistory();
    }

    /// <summary>
    /// ladataan asetukset laitten musitissa olevasta tiedostosta.
    /// </summary>
    /// <returns></returns>
    public static SettingsData LoadSettings()
    {
        if (File.Exists(settingsPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(settingsPath, FileMode.Open);
            try
            {
                currentSettingsSave = formatter.Deserialize(stream) as SettingsData;
            }
            catch (Exception e)
            {
                Debug.LogError($"failed to read the save.file at {settingsPath}. Lets delete it.");
                stream.Close();
                File.Delete(settingsPath);
                currentSettingsSave = new SettingsData("", 0, 20);
            }
            stream.Close();
        }
        else
        {
            Debug.LogError("save-file not found in: " + settingsPath);
            currentSettingsSave = new SettingsData("", 0, 20);
        }
        return currentSettingsSave;
    }

    /// <summary>
    /// tallennetaan tieto ker‰tyist‰ suunnistuspisteist‰. T‰m‰ tiedosto tallennetaan samanlaiseksi ryhm‰n nimen kanssa.
    /// </summary>
    /// <param name="collectedPoints"></param>
    public static void SaveProgress(Progress collectedPoints)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SavePath(currentSettingsSave.GetGroupName()), FileMode.Create);

        formatter.Serialize(stream, collectedPoints);
        stream.Close();
    }

    /// <summary>
    /// lataa tiedon ker‰tyist‰ suunnistuspisteist‰ laitteen musitissa oleavsta (ryhm‰n nimi).oulugo tiedostosta.
    /// Jos ep‰onnistuu siin‰, niin palauttaa tiedon ett‰ mit‰n ei olla viel‰ ker‰tty.
    /// </summary>
    /// <returns></returns>
    public static Progress LoadProgress()
    {
        string path = SavePath(currentSettingsSave.GetGroupName());
        Progress collectedPoints;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            try
            {
                collectedPoints = formatter.Deserialize(stream) as Progress;
            }catch (Exception e)
            {
                Debug.LogError($"failed to read the {path} file. Lets just take empty Progress() object then.");
                collectedPoints = new Progress();
            }
            stream.Close();
        }
        else
        {
            Debug.LogError("progress-file not found in: " + path);
            collectedPoints = new Progress();
        }
        return collectedPoints;
    }

    private static string SavePath(string groupName)
    {
        return $"{Application.persistentDataPath}/{groupName}.oulugo";
    }
}

/// <summary>
/// tallenettavan asetus tiedon muoto. ja pari hyˆdyllist‰ methodia
/// </summary>
[System.Serializable]
public class SettingsData
{
    private List<string> nameHistory = new List<string>();
    public List<string> GetNameHistory() { return nameHistory; }
    private string groupName;
    public string GetGroupName() { return groupName; }
    public int areaValue;
    public int drawDistance;
    //todo?
    public SettingsData(string groupName, int areaValue, int drawDistance)
    {
        this.groupName = groupName;
        this.areaValue = areaValue;
        this.drawDistance = drawDistance;
        //lis‰‰ muita tallennetavia arvoja?
    }
    public void UpdateGroupName(string newName)
    {
        if (groupName != null && groupName != "" && !nameHistory.Contains(groupName))
            nameHistory.Add(groupName);
        groupName = newName;
    }
}

/// <summary>
/// tallenettavan edistys tiedon muoto. ja jokunen hyˆdyllinen methodi
/// </summary>
[System.Serializable]
public class Progress
{
    //t‰t‰ timestamppia ei viel‰ t‰ss‰ vaiheesa k‰ytet‰ mihink‰‰n. T‰m‰n avulla voitaisiin periaatteesa tarkistaa ett‰ onko k‰ytt‰j‰ huijannut, jos sellaista ep‰ill‰‰n.
    private DateTime start;
    private List<PointData> points;

    public Progress()
    {
        points = new List<PointData>();
        start = DateTime.Now;
    }
    public void AddPoint(ELocationName location)
    {
        if (GetLocations().Contains(location))
        {
            Debug.LogError($"you already have collected {location}?! That checkpoint should not exist!");
            return;
        }
        points.Add(new PointData(location));
    }
    public List<PointData> GetPoints() { return points; }
    public List<ELocationName> GetLocations() 
    {
        List<ELocationName> locations = new List<ELocationName>();
        foreach (PointData point in points)
        {
            locations.Add(point.GetLocation());
        }
        return locations;
    }
}

/// <summary>
/// yksitt‰isest‰ ker‰tyst‰ checkpointista tallennettava tieto sek‰ contructori ja pari getteri‰
/// </summary>
[System.Serializable]
public class PointData
{
    //t‰t‰ timestamppia ei viel‰ t‰ss‰ vaiheesa k‰ytet‰ mihink‰‰n. T‰m‰n avulla voitaisiin periaatteesa tarkistaa ett‰ onko k‰ytt‰j‰ huijannut, jos sellaista ep‰ill‰‰n.
    private DateTime collected;
    private ELocationName location;

    public PointData(ELocationName location)
    {
        this.location = location;
        collected = DateTime.Now;
    }

    public DateTime GetTimestamp() { return collected; }
    public ELocationName GetLocation() { return location; }
}