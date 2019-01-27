using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required when Using UI elements.



public class generateSeed : MonoBehaviour
{
    public InputField mainInputField;
    public Button generateSeedButton, playButton, createSeed;
    public Text seedText;
    public string seedString = "";
    public string seed = "";
    // Start is called before the first frame update
    void Start()
    {
        generate();
        generateSeedButton.onClick.AddListener(TaskOnClick);
        playButton.onClick.AddListener(PlayOnClick);
        createSeed.onClick.AddListener(CreateClick);
    }

    void TaskOnClick()
    {
        generate();
    }

    void CreateClick()
    {
        seed = mainInputField.text;
        
    }

    void PlayOnClick()
    {
        PlayerPrefs.SetString("player_seed", seed);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {
        //seedString = string.Concat("Your seed: " , seed);
        seedString = ("Your seed: " + seed);
        seedText.text = seedString;
        
    }

    void generate()
    {
        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want
        string randomSeed = "";

        int charAmount = Random.Range(10, 15); //set those to the minimum and maximum length of your string
        for (int i = 0; i < charAmount; i++)
        {
            randomSeed += glyphs[Random.Range(0, glyphs.Length)];
        }

        seed =  randomSeed;
    }

}
