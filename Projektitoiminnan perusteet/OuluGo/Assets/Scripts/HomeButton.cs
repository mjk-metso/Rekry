using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    /// <summary>
    /// tämä ajetaan kun käyttäjä painaa pelin asetuksista löytyvää "<- Palaa aloitusnäkymään" näppäintä
    /// </summary>
    public void ToHomeScreen()
    {
        SceneManager.LoadScene(0);
    }
}
