using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required when Using UI elements.


public class endGameScript : MonoBehaviour
{
    public Button menuButton, quitButton;
    public Text seedText, highScoreText;
    public string seedString = "";
    public string seed = "";
    public string highScore = "";
    public int  score = 0;
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
        score = PlayerPrefs.GetInt("player_time", 0);
        seedString = ("Your Map's Seed was: " + seed);
        highScore = ("Your time through the level was: " + score);
        highScoreText.text = highScore;
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
