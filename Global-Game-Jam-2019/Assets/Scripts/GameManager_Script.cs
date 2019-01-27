using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Script : MonoBehaviour
{
	public GameObject TestChar;
	public GameObject TestObj;

	public HingeJoint2D CharGrabJoint;
	public CircleCollider2D TestGrabBox;

	public HUD_Script HUDScr;

	public Sprite StandingSpr;
	public Sprite GrabbingSpr;
	public Sprite[] WalkingSprs;

	bool isGrabbing;
	bool isWalking;
	Collider2D[] collArr;
	ContactFilter2D grabFilter;
	
	// Start is called before the first frame update
    void Start()
    {
		CharGrabJoint.enabled = false;
		isGrabbing = false;
		isWalking = false;
		collArr = new Collider2D[1];
		grabFilter = new ContactFilter2D();
		grabFilter.SetLayerMask(LayerMask.GetMask("Grabbable"));
    }

    // Update is called once per frame
    void Update()
    {
//#if UNITY_EDITOR

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

        if (isWalking)
        {
            AudioManager.Instance.PlayAudioFile(AudioManager.AudioFile.FootstepsWood);
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

//#endif
	}
}
