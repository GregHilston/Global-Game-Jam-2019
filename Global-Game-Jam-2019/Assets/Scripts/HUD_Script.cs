using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Script : MonoBehaviour
{
    public float PlayerPercent { get; set; }
	public float EnemyPercent { get; set; }
	public int Timer { get; private set; }

	public Text TimerText;
	public Image PlayerBar;
	public Image EnemyBar;
	
	// Start is called before the first frame update
    void Start()
    {
		Timer = 0;
		PlayerPercent = 0.0f;
		EnemyPercent = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
		Timer++;
		//TimerText.text = string.Format("{0,2:D2}", (Timer / 3600).ToString()) + ":" +
		//	string.Format("{0,2:D2}", ((Timer % 3600) / 60).ToString()) + ":" +
		//	string.Format("{0,3:D3}", ((Timer % 60) * 16.67f).ToString());
		TimerText.text = (Timer / 3600).ToString("00") + ":" +
			((Timer % 3600) / 60).ToString("00") + ":" +
			((int)((Timer % 60) * 16.67f)).ToString("000");

		if (Input.GetKeyDown(KeyCode.A))
		{
			PlayerPercent -= 0.05f;
			if (PlayerPercent < 0f)
			{
				PlayerPercent = 0f;
			}
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			PlayerPercent += 0.05f;
			if (PlayerPercent > 100f)
			{
				PlayerPercent = 100f;
			}
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			EnemyPercent -= 0.05f;
			if (EnemyPercent < 0f)
			{
				EnemyPercent = 0f;
			}
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			EnemyPercent += 0.05f;
			if (EnemyPercent > 100f)
			{
				EnemyPercent = 100f;
			}
		}

		PlayerBar.rectTransform.sizeDelta = new Vector2(
			Mathf.Clamp(PlayerPercent * 100f, 0f, 100f),
			PlayerBar.rectTransform.sizeDelta.y);
		PlayerBar.transform.localPosition = new Vector2(
			Mathf.Clamp(50f - (PlayerPercent * 50f), 0f, 50f) * -1f,
			PlayerBar.transform.localPosition.y);

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
