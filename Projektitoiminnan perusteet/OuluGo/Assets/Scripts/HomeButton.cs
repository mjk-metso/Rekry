using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    /// <summary>
    /// t�m� ajetaan kun k�ytt�j� painaa pelin asetuksista l�ytyv�� "<- Palaa aloitusn�kym��n" n�pp�int�
    /// </summary>
    public void ToHomeScreen()
    {
        SceneManager.LoadScene(0);
    }
}
