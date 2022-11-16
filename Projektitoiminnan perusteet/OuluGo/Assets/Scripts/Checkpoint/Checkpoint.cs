using Google.Maps;
using Google.Maps.Coord;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PointInfo;

/// <summary>
/// jokainen checkpoint sis�lt�� t�m�n scriptin/komponentin
/// </summary>
public class Checkpoint : MonoBehaviour
{
    /// <summary>
    /// The <see cref="MapsService"/> is the entry point to communicate with to the Maps SDK for
    /// Unity. It provides apis to load map regions, and dispatches events throughout the loading
    /// process. These events, in particular WillCreate/DidCreate events provide a finer control on
    /// each loaded map feature.
    /// </summary>
    [Tooltip("Required maps service (used to communicate with the Maps plugin).")]
    public MapsService MapsService;

    public ELocationName location;
    public Collider trigger;

    public TMP_Text label;
    public TMP_Text value;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = GetPosition();
        label.text = location.ToString();
        value.text = $"{PointInfo.GetValue(location)}p";
        trigger.enabled = true;
    }

    private void Update()
    {
        //k��nt�� checkpointin tekstit osoittamaan aina kameraa p�in.
        label.transform.parent.LookAt(Camera.main.transform);
    }

    /// <returns>sijainnin koordinaattien sijainti 3d maailmassa Vector3 muutettuna</returns>
    public Vector3 GetPosition()
    {
        return MapsService.Projection.FromLatLngToVector3(
                PointInfo.GetLatLng(location)
                );
    }
}

/// <summary>
/// staattinen luokka joka sis�lt�� tietoa checkpointeista sek� jokusen hy�dyllisen methodin.
/// </summary>
public static class PointInfo
{
    /// <summary>
    /// kuinka arvokas piste on
    /// </summary>
    private static Dictionary<ELocationName, int> value = new Dictionary<ELocationName, int>()
        {
            {ELocationName.Fablab, 1 },
            {ELocationName.YTHS, 1 },
            {ELocationName.Teekkaritalo, 3 },
            {ELocationName.Apinatalo, 2 },
            {ELocationName.Yliopisto, 1 },
            {ELocationName.Toripolliisi, 2 },
            {ELocationName.OulunPaakirjasto, 2 },
            {ELocationName.Poliisiasema, 2 },
            {ELocationName.ValkeaKauppakeskus, 2 },
            {ELocationName.Pekuri, 2 },
            {ELocationName.OulunYliopistollinenSairaala, 3 },
            {ELocationName.Kantakortteli, 2 },
            {ELocationName.Medisiina, 3 },
            {ELocationName.PaskaKaupunniGrafiitti, 2 },
            {ELocationName.KiikelinSaari, 2 },
            {ELocationName.NallikarinUimaranta, 5 },
            {ELocationName.OulunKeilahalli, 2 },
            {ELocationName.OulunEnergiaAreena, 2 },
            {ELocationName.OulunUimahalli, 2 },
            {ELocationName.Tuomiokirkko, 2 },
            {ELocationName.Tietomaa, 2 },
            {ELocationName.TuiranUimaranta, 3 },
            {ELocationName.LinnanmaanLiikuntahalli, 2 },
            {ELocationName.Humanistipallo, 1 },
            {ELocationName.LinnanmaanPrisma, 2 },
            {ELocationName.KasvitieteellinenPuutarha, 2 }
        };

    /// <summary>
    /// pisteen pituus ja leveys koodrinaatit
    /// </summary>
    private static Dictionary<ELocationName, LatLng> coordinates = new Dictionary<ELocationName, LatLng>()
        {
            {ELocationName.Fablab, new LatLng(65.05912f, 25.46864f) },
            {ELocationName.YTHS, new LatLng(65.05788f, 25.47162f) },
            {ELocationName.Teekkaritalo, new LatLng(65.06388f, 25.48391f) },
            {ELocationName.Apinatalo, new LatLng(65.06154f, 25.47974f) },
            {ELocationName.Yliopisto, new LatLng(65.05935f, 25.46628f) },
            {ELocationName.Toripolliisi, new LatLng(65.01329f, 25.46532f) },
            {ELocationName.OulunPaakirjasto, new LatLng(65.01553f, 25.46339f) },
            {ELocationName.Poliisiasema, new LatLng(65.01338f, 25.47182f) },
            {ELocationName.ValkeaKauppakeskus, new LatLng(65.01144f, 25.47240f) },
            {ELocationName.Pekuri, new LatLng(65.01172f, 25.46879f) },
            {ELocationName.OulunYliopistollinenSairaala, new LatLng(65.00705f, 25.51799f) },
            {ELocationName.Kantakortteli, new LatLng(65.00938f, 25.47077f) },
            {ELocationName.Medisiina, new LatLng(65.00729f, 25.52565f) },
            {ELocationName.PaskaKaupunniGrafiitti, new LatLng(65.0128f, 25.4767f) },
            {ELocationName.KiikelinSaari, new LatLng(65.01321f, 25.45959f) },
            {ELocationName.NallikarinUimaranta, new LatLng(65.03076f, 25.41143f) },
            {ELocationName.OulunKeilahalli, new LatLng(65.00151f, 25.45864f) },
            {ELocationName.OulunEnergiaAreena, new LatLng(65.00832f, 25.49485f) },
            {ELocationName.OulunUimahalli, new LatLng(65.00933f, 25.49778f) },
            {ELocationName.Tuomiokirkko, new LatLng(65.01486f, 25.47538f) },
            {ELocationName.Tietomaa, new LatLng(65.01848f, 25.48525f) },
            {ELocationName.TuiranUimaranta, new LatLng(65.02284f, 25.48864f) },
            {ELocationName.LinnanmaanLiikuntahalli, new LatLng(65.05517f, 25.47157f) },
            {ELocationName.Humanistipallo, new LatLng(65.06083f, 25.46773f) },
            {ELocationName.LinnanmaanPrisma, new LatLng(65.05424f, 25.45650f) },
            {ELocationName.KasvitieteellinenPuutarha, new LatLng(65.06292f, 25.46525f) }
        };

    /// <summary>
    /// pisteen kuvaus
    /// </summary>
    private static Dictionary<ELocationName, string> descriptions = new Dictionary<ELocationName, string>()
        {
            {ELocationName.Fablab, "Oulun yliopiston tiloissa toimiva pieni digitaalisen valmistamisen ty�paja." },
            {ELocationName.YTHS, "Ylioppilaiden terveydenhoitos��ti�. Opiskelijoiden p��asiallinen terveydenhuollon asiointipaikka." },
            {ELocationName.Teekkaritalo, "Yliopiston l�hell� sijaitseva vuokrakiinteist� jossa usein j�rjestet��n opiskelijabileit� ja �tapahtumia." },
            {ELocationName.Apinatalo, "L�hes legendaarinen �Apinatalon� kerhohuone sijaitsee Tellervontie 2 A:n kattohuoneistossa." },
            {ELocationName.Yliopisto, "Oulun yliopiston kampus. Kampuksella toimivat sek� Oulun yliopiston tiedekunnat sek� Oulun Ammattikorkeakoulu." },
            {ELocationName.Toripolliisi, "Oulun kauppatorilla seisova pronssinen patsas. Patsaan on valmistanut kuvanveist�j� Kaarlo Mikkonen vuonna 1987." },
            {ELocationName.OulunPaakirjasto, "Keskustassa torin vieress� sijaitseva Oulun p��kirjasto." },
            {ELocationName.Poliisiasema, "Poliisiasema. Hmm. Rakennus jossa on poliiseja tekem�ss� poliisijuttuja." },
            {ELocationName.ValkeaKauppakeskus, "Oulun keskustaan vuonna 2016 avattu noin 60 liikkeen kauppakeskus." },
            {ELocationName.Pekuri, "Opiskelijaravintola oulun keskustan kauppakortteli Pekurissa." },
            {ELocationName.OulunYliopistollinenSairaala, "Oulun p��asiallinen p�ivyst�v� sairaala." },
            {ELocationName.Kantakortteli, "Opiskelijaravintola." },
            {ELocationName.Medisiina, "Opiskelijaravintola." },
            {ELocationName.PaskaKaupunniGrafiitti, "\"1980-luvulla ensimm�isen kerran ilmestynyt grafiitti, joka poistamisyrityksist� huolimatta ilmestyy yh� uudelleen samalle paikalle. Osa kaupungin identiteetti�." },
            {ELocationName.KiikelinSaari, "opiskelijoiden suosikki puisto vappuisin." },
            {ELocationName.NallikarinUimaranta, "Oulun Hietasaaressa sijaitseva hiekkaranta. Alueelta l�ytyy my�s Kylpyl�hotelli Eden, huvipuisto Vauhtipuisto ja Ravintola Nallikari." },
            {ELocationName.OulunKeilahalli, "Keilahalli, josta l�ytyy keilaratojen lis�ksi muun muassa muassa kahvio ja biljardip�yti�." },
            {ELocationName.OulunEnergiaAreena, "Toiselta nimelt��n Oulun j��halli. Yleis�kapasiteetti on 6400, eli kyseess� ei ole mk��n ihan pieni j��halli." },
            {ELocationName.OulunUimahalli, "Yksi Suomen suurimpia uimahalleja. Allasosaston lis�ksi uimahallista l�ytyy kahvio, kokoustilat, kuntosali, liikuntasali sek� judo ja nyrkkeilysali." },
            {ELocationName.Tuomiokirkko, "Oulun tuomiokirkko valmistui vuonna 1777, sen puurakenteet kuitenkin paloivat vuonna 1822 tulipalossa. Kirkko rakennettiin uudelleen arkkitehti Carl Ludvig Engelin piirustusten mukaan." },
            {ELocationName.Tietomaa, "Suomen ensimm�inen tiedekeskus. Tietomaassa on vaihtuvia teeman�yttelyit� liittyen tieteeseen ja tekniikkaan." },
            {ELocationName.TuiranUimaranta, "Uimaranta jonka yhteydest� l�ytyy my�s kes�kahvila." },
            {ELocationName.LinnanmaanLiikuntahalli, "sis�lt�� liikutasalin jossa on mahdollista pelata monenlaisia salipelej�, kuntosalin, musiikkikuntosalin sek� kokoushuoneen." },
            {ELocationName.Humanistipallo, "Matti Peltokankaan taideteos 'Yhtyv�t s�teet', joka tunnetaan tuttavallisesti humanistipallona. Se kuvaa humaniostisen tutkimuksen ymp�ripy�reytt� ja urautuneisuutta." },
            {ELocationName.LinnanmaanPrisma, "Yliopiston l�hettyvill� sijaitsevista kaupoista suurin." },
            {ELocationName.KasvitieteellinenPuutarha, "Oulun yliopiston kasvitieteellinen puutarha on maailman pohjoisimpia tieteellisi� puutarhoja.\n\n" +
            "Yleis�lle avointa kasvikokoelmaa yll�pidet��n opetusta, tutkimusta ja kasvilajien suojelua varten.\n\n" +
            "Yli 4000 eri kasvilajia sis�lt�v� puutarha tarjoaa k�vij�lleen katsauksen pohjoisen kasvillisuuden monimuotoisuuteen ja toimii koekentt�n� monille uusille ja t��ll� harvinaisille kasveille.\n\n" +
            "Maamerkkin� toimivat pyramidikasvihuoneet Romeo ja Julia esitellen noin 1,200 eksoottista kasvilajia, sek� Oulun yliopiston Tiedepuutarhan.\n\n" +
            "(teksti kopioitu Oulun Yliopiston sivustoilta)" }
        };


    public static int GetValue(ELocationName name) { return value[name]; }
    public static LatLng GetLatLng(ELocationName name) { return coordinates[name]; }
    public static string GetDescription(ELocationName name) { return descriptions[name]; }

    /// <summary>
    /// kaikki checkpointit
    /// </summary>
    public enum ELocationName
    {
        Fablab,
        YTHS,
        Teekkaritalo,
        Apinatalo,
        Yliopisto,
        LinnanmaanLiikuntahalli,
        Humanistipallo,
        LinnanmaanPrisma,
        KasvitieteellinenPuutarha,
        Toripolliisi,
        OulunPaakirjasto,
        Poliisiasema,
        ValkeaKauppakeskus,
        Pekuri,
        OulunYliopistollinenSairaala,
        Kantakortteli,
        Medisiina,
        PaskaKaupunniGrafiitti,
        KiikelinSaari,
        NallikarinUimaranta,
        OulunKeilahalli,
        OulunEnergiaAreena,
        OulunUimahalli,
        Tuomiokirkko,
        Tietomaa,
        TuiranUimaranta
    }

    //pisteitten kategorisoimisesta voi olla hy�ty� monella tavalla tulevaisuudessa, jos projektin kehitt�mist� eteenp�in jatketaan.
    public enum EYliopistolla
    {
        Fablab = ELocationName.Fablab,
        YTHS = ELocationName.YTHS,
        Teekkaritalo = ELocationName.Teekkaritalo,
        Apinatalo = ELocationName.Apinatalo,
        Yliopisto = ELocationName.Yliopisto,
        LinnanmaanLiikuntahalli = ELocationName.LinnanmaanLiikuntahalli,
        Humanistipallo = ELocationName.Humanistipallo,
        LinnanmaanPrisma = ELocationName.LinnanmaanPrisma,
        KasvitieteellinenPuutarha = ELocationName.KasvitieteellinenPuutarha
    }
    public enum EKeskustassa
    {
        Toripolliisi = ELocationName.Toripolliisi,
        OulunPaakirjasto = ELocationName.OulunPaakirjasto,
        Poliisiasema = ELocationName.Poliisiasema,
        ValkeaKauppakeskus = ELocationName.ValkeaKauppakeskus,
        Pekuri = ELocationName.Pekuri,
        Kantakortteli = ELocationName.Kantakortteli,
        PaskaKaupunniGrafiitti = ELocationName.PaskaKaupunniGrafiitti,
        KiikelinSaari = ELocationName.KiikelinSaari,
        OulunEnergiaAreena = ELocationName.OulunEnergiaAreena,
        OulunUimahalli = ELocationName.OulunUimahalli,
        Tuomiokirkko = ELocationName.Tuomiokirkko,
        Tietomaa = ELocationName.Tietomaa
    }
    public enum ESyrjaiset
    {
        OulunYliopistollinenSairaala = ELocationName.OulunYliopistollinenSairaala,
        Medisiina = ELocationName.Medisiina,
        TuiranUimaranta = ELocationName.TuiranUimaranta,
        OulunKeilahalli = ELocationName.OulunKeilahalli,
        NallikarinUimaranta = ELocationName.NallikarinUimaranta
    }
}
