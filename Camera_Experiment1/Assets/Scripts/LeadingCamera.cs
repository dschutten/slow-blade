using UnityEngine;
using System.Collections;

public class LeadingCamera : MonoBehaviour {
	
	public Transform target;
	//public float bottom = -10f; // Bottom edge seen by the camera
	public float lead = 2.5f; // amout to lead the target
	//public float above= 5f;// amount above the target pivot to see
	public float minView = 7.5f;
	public float margin = 2f;
	public float lerpRate=2f;

	public Vector3 smoothDampVelocity;
	public float smoothTime = 2.5f;

	float playerSpeed;


	Vector2 playerPos;
	Vector2 lastPos;
	Vector2 direction;
	Vector2 leadPoint;


	// Use this for initialization
	void Start () {
		playerPos = target.transform.position;
		lastPos = target.transform.position;
		StartCoroutine(GetDirection());
	}
	
	// Update is called once per frame after Update (put camera stuff here)
	void LateUpdate () {

		playerPos = target.transform.position;
		leadPoint = (direction * (lead)) + (playerPos);

		Vector3 targVector = new Vector3 (leadPoint.x, leadPoint.y, -10f); //gives this camera a new position
		//transform.position = targVector;

		//transform.position = Vector3.Lerp(transform.position, targVector, Time.deltaTime * lerpRate);
		transform.position = Vector3.SmoothDamp(transform.position, targVector, ref smoothDampVelocity, smoothTime);
		//transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );

	}


	IEnumerator GetDirection()
	{
		while(true)
		{
			Vector2 moveVec = new Vector2 (playerPos.x - lastPos.x, playerPos.y - lastPos.y);
			//if (moveVec != Vector2.zero){
				direction = moveVec.normalized;
			//}
			lastPos = target.transform.position;
			yield return new WaitForSeconds(0.5f);
		}
	}
	
}


