using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class StartButton : MonoBehaviour
{
    public InputField groupName;
    public Text groupNameRequirements;

    public int nameMinLength = 1;
    public int nameMaxLength = 30;

    public Text message;

    public Dropdown area;
    public Slider drawDistance;

    // Start is called before the first frame update
    void Start()
    {
        SettingsData loaded = SaveSystem.LoadSettings();
        groupName.text = loaded.GetGroupName();
        area.value = loaded.areaValue;
        drawDistance.value = loaded.drawDistance;

        groupNameRequirements.text = $"({nameMinLength}-{nameMaxLength} kirjainta)";
        message.gameObject.SetActive(false);
    }

    /// <summary>
    /// kun timer arvoksi annetaan esim 2, silloin message nimisen Text komponentin/scriptin gameobjekti laitetaan pois p‰‰lt‰ 2 sekunnin p‰s‰t‰.
    /// </summary>
    private float timer = 0f;
    void Update()
    {
        if (message.gameObject.activeSelf)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                message.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// t‰t‰ kutsutaan silloin kun pelin aloittamis napista on painettu.
    /// </summary>
    public void StartGame()
    {
        if (groupName.text.Length < nameMinLength)
        {
            CreateMessage($"Nimen pit‰‰ sis‰lt‰‰ v‰hint‰‰n ({nameMinLength}) merkki‰!");
        }
        else if (groupName.text.Length > nameMaxLength)
        {
            CreateMessage($"Nimi voi sis‰lt‰‰ korkeintaan ({nameMaxLength}) merkki‰!");
        }
        else
        {
            Regex rgx = new Regex("[^A-Za-z0-9]");
            if (rgx.IsMatch(groupName.text))
            {
                if (groupName.text == "(history)" || groupName.text == "(historia)")
                {
                    string nameHistory = "";
                    foreach (string s in SaveSystem.LoadSettings().GetNameHistory())//SaveSystem.SaveSettings(groupName.text, area.value, (int)drawDistance.value))
                    {
                        nameHistory += $" {s},";
                    }
                    CreateMessage(nameHistory);
                }
                else
                {
                    CreateMessage("Nimi ei saa sis‰lt‰‰ erikoismerkkej‰!");
                }
                return;
            }
            //t‰ss‰ tallennetaan asetukset laitteen musitiin ja siirryt‰‰n karttan‰kym‰‰n.
            SaveSystem.SaveSettings(groupName.text, area.value, (int)drawDistance.value);
            SceneManager.LoadScene(1);

            //Nimest‰ voidaan halutessa/tarvittaessa egneroida myˆs numeroita, jolloin voidaan esimerkiksi tehd‰
            //samanryhm‰n nimen syˆtt‰neille pelaajille kaikille samanlainen suunnistusreitti.
            //Mutta emme m‰‰r‰‰ ryhmille reittej‰ ainakan t‰ss‰ ohjelman versiossa
            /*Random rnd = new Random(groupName.text.GetHashCode());
            string rndMessage = "";
            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                int newNumb = rnd.Next(3);
                sum += newNumb;
                rndMessage += $" {newNumb}";
            }
            rndMessage += $" \n\n{sum}";
            CreateMessage(rndMessage);*/
        }
    }

    /// <summary>
    /// t‰ll‰ saadaan n‰ytˆlle n‰kyviin virheilmoituksia tms.
    /// </summary>
    /// <param name="text">the message to user</param>
    private void CreateMessage(string text)
    {
        message.text = text;
        timer = 2f;
        if (!message.gameObject.activeSelf)
        {
            message.gameObject.SetActive(true);
        }
    }
}
