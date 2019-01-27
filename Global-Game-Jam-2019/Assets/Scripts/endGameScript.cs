using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required when Using UI elements.


public class endGameScript : MonoBehaviour
{
    public Button menuButton, quitButton;
    public Text seedText, scoreText, highText;
    public string seedString = "";
    public string seed = "";
    public string highScore = "";
    public string highScoreSeed = "";
    public int  score = 0;
    public int high = 0;
    // Start is called before the first frame update
    void Start()
    {
        menuButton.onClick.AddListener(PlayOnClick);
        quitButton.onClick.AddListener(QuitOnClick);
        highScoreSeed = PlayerPrefs.GetString("high_score_seed", "NONE");
        seed = PlayerPrefs.GetString("player_seed", "NONE");
        high = PlayerPrefs.GetInt("highscore", 0);
        score = PlayerPrefs.GetInt("player_time", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //seed = "dfdfg2321414";
        highScore = ("Your time through the level was: " + score);
        scoreText.text = highScore;
        seedText.text = ("Your Map's Seed was: " + seed);
        highText.text = ("The highest score is: " + score + " With Map seed: " + highScoreSeed);
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
