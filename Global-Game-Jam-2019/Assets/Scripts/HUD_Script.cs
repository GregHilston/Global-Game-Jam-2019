using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Script : MonoBehaviour
{
	// Tracks the percentage of the house that the player controls
	public float PlayerPercent { get; protected set; }
	// Tracks the percentage of the house the enemy controls
	public float EnemyPercent { get; protected set; }
	// Tracks how many frames the player has been playing the game
	public uint Timer { get; protected set; }

	// UI elements that are updated with changes to the state of the game
	public Text TimerText;
	public Image PlayerBar;
	public Image EnemyBar;
	
	// Start is called before the first frame update
    void Start()
    {
		// Initializes the timer to start counting from 0
		Timer = 0;

		// Initializes both of the percentages of house that the player
		// and enemy control to 0%
		PlayerPercent = 0.0f;
		EnemyPercent = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
		// Timer ticks up every frame
		// Operates under assumption that game runs at steady 60fps
		Timer++;
		
		// Updates the timer UI text with current time.
		// The timer text is formatted as follows: MM:ss:mmm,
		// where M is minutes, s is seconds, and m is milliseconds.
		TimerText.text = (Timer / 3600).ToString("00") + ":" +
			((Timer % 3600) / 60).ToString("00") + ":" +
			((int)((Timer % 60) * 16.67f)).ToString("000");

#if UNITY_EDITOR

		//
		// Special testing in Unity Editor only.
		// This can be used to test changing the percentages of the
		// house that the player and enemy control at any given time.
		//

		// Reduces player's percentage by 5%
		if (Input.GetKeyDown(KeyCode.A))
		{
			PlayerPercent -= 0.05f;
			if (PlayerPercent < 0f)
			{
				PlayerPercent = 0f;
			}
		}
		// Increases player's percentage by 5%
		if (Input.GetKeyDown(KeyCode.D))
		{
			PlayerPercent += 0.05f;
			if (PlayerPercent > 100f)
			{
				PlayerPercent = 100f;
			}
		}
		// Reduces enemy's percentage by 5%
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			EnemyPercent -= 0.05f;
			if (EnemyPercent < 0f)
			{
				EnemyPercent = 0f;
			}
		}
		// Increases enemy's percentage by 5%
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			EnemyPercent += 0.05f;
			if (EnemyPercent > 100f)
			{
				EnemyPercent = 100f;
			}
		}

		// Updates player's progress bar
		PlayerBar.rectTransform.sizeDelta = new Vector2(
			Mathf.Clamp(PlayerPercent * 100f, 0f, 100f),
			PlayerBar.rectTransform.sizeDelta.y);
		PlayerBar.transform.localPosition = new Vector2(
			Mathf.Clamp(50f - (PlayerPercent * 50f), 0f, 50f) * -1f,
			PlayerBar.transform.localPosition.y);
		// Updates enemy's progress bar
		EnemyBar.rectTransform.sizeDelta = new Vector2(
			Mathf.Clamp(EnemyPercent * 100f, 0f, 100f),
			EnemyBar.rectTransform.sizeDelta.y);
		EnemyBar.transform.localPosition = new Vector2(
			Mathf.Clamp(50f - (EnemyPercent * 50f), 0f, 50f),
			EnemyBar.transform.localPosition.y);

#endif
	}

	/// <summary>
	/// Updates UI elements of the HUD based on changes in player and enemy control
	/// </summary>
	/// <param name="newPlayerPercentage">The new percentage of the house the player controls</param>
	/// <param name="newEnemyPercentage">The new percentage of the house the enemy controls</param>
	public void OnControlChange(float newPlayerPercentage, float newEnemyPercentage)
	{
		// Changes player's percentages to new values
		PlayerPercent = newPlayerPercentage;
		EnemyPercent = newEnemyPercentage;

		// NOTE: values for how large the progress bars are are still hard-coded at
		// a maximum of 100 units of length.

		// Updates player's progress bar
		PlayerBar.rectTransform.sizeDelta = new Vector2(
			Mathf.Clamp(PlayerPercent * 100f, 0f, 100f),
			PlayerBar.rectTransform.sizeDelta.y);
		PlayerBar.transform.localPosition = new Vector2(
			Mathf.Clamp(50f - (PlayerPercent * 50f), 0f, 50f) * -1f,
			PlayerBar.transform.localPosition.y);

		// Updates enemy's progress bar
		EnemyBar.rectTransform.sizeDelta = new Vector2(
			Mathf.Clamp(EnemyPercent * 100f, 0f, 100f),
			EnemyBar.rectTransform.sizeDelta.y);
		EnemyBar.transform.localPosition = new Vector2(
			Mathf.Clamp(50f - (EnemyPercent * 50f), 0f, 50f),
			EnemyBar.transform.localPosition.y);
	}

	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.blue;
	//	Gizmos.DrawCube(Vector3.zero, new Vector3(1f, 1f, 1f));
	//}
}
