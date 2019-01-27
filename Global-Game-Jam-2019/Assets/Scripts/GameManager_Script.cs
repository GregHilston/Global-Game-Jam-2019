using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_Script : MonoBehaviour
{
	public GameObject TestChar;
	public GameObject TestObj;

	public HingeJoint2D CharGrabJoint;
	public CircleCollider2D TestGrabBox;

	public GameObject WallHitboxPrefab;
	public GameObject FloorTilePrefab;

	public GameObject[] RedObjs;
	public GameObject[] BlueObjs;

	public HUD_Script HUDScr;
	public MapGenerator MapGenScr;

	public Sprite StandingSpr;
	public Sprite GrabbingSpr;
	public Sprite[] WalkingSprs;

    public Camera mainCamera;

	bool isGrabbing;
	bool isWalking;
    bool isKicking;
    bool isCameraFollowingPlayer = false;
	Collider2D[] collArr;
	ContactFilter2D grabFilter;

	Random rand;

    bool playedIsGrabbing = false;

    // Start is called before the first frame update
    void Start()
    {
		Application.targetFrameRate = 60;

		AudioManager.Instance.PlayAudioFile(AudioManager.AudioFile.MainTheme, true);

        CharGrabJoint.enabled = false;
		isGrabbing = false;
		isWalking = false;
		collArr = new Collider2D[1];
		grabFilter = new ContactFilter2D();
		grabFilter.SetLayerMask(LayerMask.GetMask("Grabbable"));

		rand = new Random();

		//MapGenScr.GenerateMap();

		for (int i = 0; i < MapGenScr.AllWalls.Count; i++)
		{
			GameObject tmpObj = Instantiate(WallHitboxPrefab, transform);
			Vector2 aVec = new Vector2(MapGenScr.AllWalls[i].Pt0.Item1, MapGenScr.AllWalls[i].Pt0.Item2);
			Vector2 bVec = new Vector2(MapGenScr.AllWalls[i].Pt1.Item1, MapGenScr.AllWalls[i].Pt1.Item2);
			Vector2 cVec = bVec - aVec;
			tmpObj.transform.localPosition = aVec + (cVec / 2f) -
				new Vector2(MapGenerator.rows / 2f, MapGenerator.columns / 2f);

			if (Mathf.Abs(cVec.x) < 0.001f)
			{
				tmpObj.transform.localScale = new Vector3(1f, Mathf.Abs(cVec.y) + 1f, 1f);
			}
			else
			{
				tmpObj.transform.localScale = new Vector3(Mathf.Abs(cVec.x) + 1f, 1f, 1f);
			}
		}
		for (int i = 0; i < MapGenScr.board.Length; i++)
		{
			for (int j = 0; j < MapGenScr.board[0].Length; j++)
			{
				if (MapGenScr.board[i][j] == MapGenerator.TileType.Floor ||
					MapGenScr.board[i][j] == MapGenerator.TileType.InteralDoor)
				{
					GameObject tmpObj = Instantiate(FloorTilePrefab, transform);
					tmpObj.transform.localPosition = new Vector3(i, j, 2f) -
						new Vector3(MapGenerator.rows / 2f, MapGenerator.columns / 2f, 0f);
				}
			}
		}

		for (int i = 0; i < MapGenScr.ObjLocations.Count; i++)
		{
			GameObject tmpObj = Instantiate(RedObjs[Random.Range(0,RedObjs.Length)], transform);
			tmpObj.transform.localPosition =
				new Vector3(MapGenScr.ObjLocations[i].Item1, MapGenScr.ObjLocations[i].Item2, 1f) -
				new Vector3(MapGenerator.rows / 2f, MapGenerator.columns / 2f, 0f);

			GameObject tmpObj2 = Instantiate(BlueObjs[Random.Range(0, BlueObjs.Length)], transform);
			tmpObj2.transform.localPosition =
				new Vector3(2f * i, -1f, 1f) -
				new Vector3(MapGenerator.rows / 2f, MapGenerator.columns / 2f, 0f);
		}

		for (int i = 0; i < MapGenScr.board.Length; i++)
		{
			for (int j = 0; j < MapGenScr.board[0].Length; j++)
			{
				MapGenScr.hasThisSpaceBeenChecked[i][j] = false;
			}
		}

		(int tmpX, int tmpY) = MapGenScr.FindFirstInstOfUncheckedTile(MapGenerator.TileType.ExternalDoor);
		TestChar.transform.position =
			new Vector3(tmpX, tmpY, 0f) -
			new Vector3(MapGenerator.rows / 2f, MapGenerator.columns / 2f, 0f);
	}

    private void ToggleCamera()
    {
        isCameraFollowingPlayer = !isCameraFollowingPlayer; // toggle ma dude

        if (isCameraFollowingPlayer)
        {
            mainCamera.orthographicSize = 8;
            mainCamera.transform.SetParent(TestChar.transform);
            mainCamera.transform.localPosition = new Vector3(0, 0, -10);
        }
        else
        {
            mainCamera.orthographicSize = 16;
            mainCamera.transform.SetParent(null);
            mainCamera.transform.position = new Vector3(0, 0, -10);
        }
    }

    private void PlayOrStopAppropriateAudio(float massOfObject)
    {
        float heavyMass = 3.0f;
        float dogMass = 0.0f;
        float floatSubstraction = 0.5f;

        // STOP IN THE NAME OF MA DUDE
        if (!isWalking)
        {
            AudioManager.Instance.StopAudioFile(AudioManager.AudioFile.FootstepsWood);
            AudioManager.Instance.StopAudioFile(AudioManager.AudioFile.Scraping);
        }

        if (!isGrabbing)
        {
            AudioManager.Instance.StopAudioFile(AudioManager.AudioFile.LiftingGrunt);
            AudioManager.Instance.StopAudioFile(AudioManager.AudioFile.Scraping);
        }

        if (!isKicking)
        {
            AudioManager.Instance.StopAudioFile(AudioManager.AudioFile.Kick);

        }


        // STARTS 

        if (isWalking && !isGrabbing)
        {
            AudioManager.Instance.PlayAudioFile(AudioManager.AudioFile.FootstepsWood);
        }

        if (isGrabbing && massOfObject >= heavyMass)
        {
            if (!playedIsGrabbing)
            {
                playedIsGrabbing = true;
                AudioManager.Instance.PlayAudioFile(AudioManager.AudioFile.LiftingGrunt);
            }
        }
        else if (isGrabbing && (massOfObject - dogMass) < floatSubstraction)
        {
            playedIsGrabbing = true;
            AudioManager.Instance.PlayAudioFile(AudioManager.AudioFile.Dog);
        }
        else if (isGrabbing && massOfObject < heavyMass)
        {
            if (!playedIsGrabbing)
            {
                playedIsGrabbing = true;
                AudioManager.Instance.PlayAudioFile(AudioManager.AudioFile.PickupVase);
            }
        }

        if (isGrabbing && isWalking && massOfObject > heavyMass)
        {
            AudioManager.Instance.PlayAudioFile(AudioManager.AudioFile.Scraping);
        }

        if (isKicking)
        {
            Debug.Log("kick!");
            AudioManager.Instance.PlayAudioFile(AudioManager.AudioFile.Kick);
            isKicking = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //#if UNITY_EDITOR

        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCamera();
        }

        if (Input.GetKey(KeyCode.RightArrow))
		{
			TestChar.GetComponent<Rigidbody2D>().velocity += new Vector2(1f, 0f);
			//TestChar.GetComponent<Animator>().SetBool("isWalking", !isGrabbing);
			isWalking = true;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			TestChar.GetComponent<Rigidbody2D>().velocity += new Vector2(-1f, 0f);
			//TestChar.GetComponent<Animator>().SetBool("isWalking", !isGrabbing);
			isWalking = true;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			TestChar.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, 1f);
			//TestChar.GetComponent<Animator>().SetBool("isWalking", !isGrabbing);
			isWalking = true;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			TestChar.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, -1f);
			//TestChar.GetComponent<Animator>().SetBool("isWalking", !isGrabbing);
			isWalking = true;
		}
		if (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow) &&
			!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
		{
			//TestChar.GetComponent<Animator>().SetBool("isWalking", false);
			isWalking = false;
		}

        //if (Input.GetKeyDown(KeyCode.A) && (TestObj.transform.position - TestChar.transform.position).magnitude <= 1.5f)
        if (Input.GetKeyDown(KeyCode.A) && TestGrabBox.OverlapCollider(grabFilter, collArr) == 1)
		{
			CharGrabJoint.enabled = true;
			CharGrabJoint.connectedBody = collArr[0].GetComponent<Rigidbody2D>();
			isGrabbing = true;

			TestChar.GetComponent<SpriteRenderer>().sprite = GrabbingSpr;
		}
		if (Input.GetKeyUp(KeyCode.A) && isGrabbing)
		{
			CharGrabJoint.enabled = false;
			isGrabbing = false;

			TestChar.GetComponent<SpriteRenderer>().sprite = StandingSpr;
		}

		if (isGrabbing)
		{
			Vector2 tmpPos = TestObj.transform.position - TestChar.transform.position;
			TestChar.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(tmpPos.y, tmpPos.x));

			if (Input.GetKeyDown(KeyCode.S))
			{
				CharGrabJoint.connectedBody.AddForce(
					(CharGrabJoint.connectedBody.transform.position - TestChar.transform.position).normalized * 10000f);

				CharGrabJoint.enabled = false;
				isGrabbing = false;
                isKicking = true;

				TestChar.GetComponent<SpriteRenderer>().sprite = StandingSpr;
			}
        }
		else
		{
			if (TestChar.GetComponent<Rigidbody2D>().velocity.SqrMagnitude() > 0f)
			{
				Vector2 tmpVec = TestChar.GetComponent<Rigidbody2D>().velocity;
				//Vector2 tmpPos = TestObj.transform.position - TestChar.transform.position;
				TestChar.transform.localRotation = Quaternion.RotateTowards(
					TestChar.transform.rotation,
					Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(tmpVec.y, tmpVec.x)),
					5f);
			}
			if (isWalking)
			{
				TestChar.GetComponent<SpriteRenderer>().sprite = WalkingSprs[HUDScr.Timer % 32 / 8];
			}
			else
			{
				TestChar.GetComponent<SpriteRenderer>().sprite = StandingSpr;
			}
		}

        PlayOrStopAppropriateAudio(CharGrabJoint.connectedBody.mass);

		List<GameObject> allTmpObjs = new List<GameObject>();
		allTmpObjs.AddRange(GameObject.FindGameObjectsWithTag("Blue"));
		allTmpObjs.AddRange(GameObject.FindGameObjectsWithTag("Red"));

		//Debug.Log("BEFORE: " + allTmpObjs.Count.ToString());

		allTmpObjs.RemoveAll(m =>
			m.transform.position.x < MapGenerator.rows / -2f ||
			m.transform.position.x > MapGenerator.rows / 2f ||
			m.transform.position.y < MapGenerator.columns / -2f ||
			m.transform.position.y > MapGenerator.columns / 2f);

		//Debug.Log("AFTER: " + allTmpObjs.Count.ToString());

		float blueInHouse = allTmpObjs.FindAll(x => x.tag == "Blue").Count;
		float redInHouse = allTmpObjs.FindAll(x => x.tag == "Red").Count;

		//Debug.Log("BLUE: " + blueInHouse.ToString() + "; RED: " + redInHouse.ToString());

		//if (Input.GetKeyDown(KeyCode.L))
		//{
		//	blueInHouse = 11f;
		//}

		if (allTmpObjs.Count < 1)
		{
			HUDScr.OnControlChange(0f, 0f);
		}
		else
		{
			if (blueInHouse / allTmpObjs.Count >= 1f && blueInHouse > 9f)
			{
				calculateHighScoreAndEndGame();
			}
			else
			{
				HUDScr.OnControlChange(blueInHouse / allTmpObjs.Count, redInHouse / allTmpObjs.Count);
			}
		}
		//#endif
	}

	void calculateHighScoreAndEndGame()
	{
		string highScoreSeed = PlayerPrefs.GetString("high_score_seed", "NONE");
		string seed = PlayerPrefs.GetString("player_seed", "NONE");
		int highScore = PlayerPrefs.GetInt("highscore", 0);
		int score = PlayerPrefs.GetInt("player_time", 0);
		if (score < highScore)
		{
			PlayerPrefs.SetInt("highscore", score);
			PlayerPrefs.SetString("high_score_seed", seed);
		}
		SceneManager.LoadScene("EndMenu");
	}

    private void Awake()
    {
        // CAMERA HACK BOIIIII
        mainCamera.transform.rotation = Quaternion.identity; // lets not rotate ma dudes
    }

    private void LateUpdate()
    {
        // CAMERA HACK BOIIIII
        mainCamera.transform.rotation = Quaternion.identity; // lets not rotate ma dudes
    }
}
