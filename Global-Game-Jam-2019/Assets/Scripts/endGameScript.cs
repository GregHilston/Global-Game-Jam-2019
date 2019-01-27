using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required when Using UI elements.


public class endGameScript : MonoBehaviour
{
    public Button menuButton, quitButton;
    public Text seedText;
    public string seedString = "";
    public string seed = "";
    // Start is called before the first frame update
    void Start()
    {
        menuButton.onClick.AddListener(PlayOnClick);
        quitButton.onClick.AddListener(QuitOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        seed = PlayerPrefs.GetString("player_seed", "NO SEED FOUND");
        seedString = ("Your Map's See was: " + seed);
        seedText.text = seedString;
    }

    void PlayOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void QuitOnClick()
    { 
            Application.Quit();
        
    }

}
