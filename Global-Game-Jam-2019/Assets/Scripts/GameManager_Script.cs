using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Script : MonoBehaviour
{
	public GameObject TestChar;
	public GameObject TestObj;

	public HingeJoint2D CharGrabJoint;
	public CircleCollider2D TestGrabBox;

	bool isGrabbing;
	Collider2D[] collArr;
	ContactFilter2D grabFilter;
	
	// Start is called before the first frame update
    void Start()
    {
		CharGrabJoint.enabled = false;
		isGrabbing = false;
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
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			TestChar.GetComponent<Rigidbody2D>().velocity += new Vector2(-1f, 0f);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			TestChar.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, 1f);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			TestChar.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, -1f);
		}

		//if (Input.GetKeyDown(KeyCode.A) && (TestObj.transform.position - TestChar.transform.position).magnitude <= 1.5f)
		if (Input.GetKeyDown(KeyCode.A) && TestGrabBox.OverlapCollider(grabFilter, collArr) == 1)
		{
			CharGrabJoint.enabled = true;
			CharGrabJoint.connectedBody = collArr[0].GetComponent<Rigidbody2D>();
			isGrabbing = true;
		}
		if (Input.GetKeyUp(KeyCode.A))
		{
			CharGrabJoint.enabled = false;
			isGrabbing = false;
		}

		if (isGrabbing)
		{
			Vector2 tmpPos = TestObj.transform.position - TestChar.transform.position;
			TestChar.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(tmpPos.y, tmpPos.x));
		}
		else if (TestChar.GetComponent<Rigidbody2D>().velocity.SqrMagnitude() > 0f)
		{
			Vector2 tmpVec = TestChar.GetComponent<Rigidbody2D>().velocity;
			//Vector2 tmpPos = TestObj.transform.position - TestChar.transform.position;
			TestChar.transform.localRotation = Quaternion.RotateTowards(
				TestChar.transform.rotation,
				Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(tmpVec.y, tmpVec.x)),
				4f);
		}

//#endif
	}
}
