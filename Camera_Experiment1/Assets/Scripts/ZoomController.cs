﻿using UnityEngine;
using System.Collections;

//This script is designed to keep the player and the blade prop on screen at all times.

public class ZoomController : MonoBehaviour {

	public Transform target;
	public Transform blade;
	//public float bottom = -10f; // Bottom edge seen by the camera
	public float lead = 2.5f; // amout to lead the target
	public float minView = 30f; // closest the camera gets.
	public float margin = 2f; // added to distance 
	public float camRefresh = 0.5f;
	public float lerpRate = 2f;
	//public float wiggleRoom = 2f;
	public float horWiggleRoom = 2f;
	public float vertWiggleRoom = 2f;

	Vector2 playerPos;
	Vector2 lastPos;
	Vector2 direction;
	Vector2 leadPoint;

	//define screen edges
	public float bottomBound;
	public float topBound;
	public float rightBound;
	public float leftBound;
	float levelWidth;
	float levelHeight;
	
	// Use this for initialization
	void Start () {
		lastPos = target.transform.position;
		StartCoroutine(GetDirection());
		SetBounds ();
	}
	
	// Update is called once per frame after Update (put camera stuff here)
	void LateUpdate () {

		playerPos = target.transform.position;

		Vector2 leadPoint = (direction *  (lead)) + playerPos;

		float distance = Vector2.Distance(target.transform.position, blade.transform.position);
		distance= Mathf.Max(distance, minView); // gets whichever is bigger.
		//distance /= 2.0f;
		distance = (distance * 0.5f) + margin;

		float midpointX = (leadPoint.x + blade.position.x) * 0.5f;
		float midpointY = (leadPoint.y + blade.position.y) * 0.5f;

		Vector3 targVector = new Vector3 (midpointX, midpointY, -10f); //gives this camera a new position
		float targCamSize = distance;
		//transform.position = targVector;

		//checks if the new camera settings will take it outside the levelbounds
		//Debug.Log("1 targCamSize:" + targCamSize+"Level Height:"+levelHeight);

		targCamSize = CheckCameraZoom(targCamSize);
		targVector = CheckCameraPos(targVector, targCamSize);

		//Debug.Log("2 targCamSize:" + targCamSize+"Level Height:"+levelHeight);
		//Debug.Log("targVector:" + targVector);

	
		//apply new target vector
		camera.orthographicSize = targCamSize; //sets the view field of the camera.

		transform.position = Vector3.Lerp(transform.position, targVector, Time.deltaTime * lerpRate); //lerp to new position

	}

	void SetBounds ()
	{
		GameObject[] ground = GameObject.FindGameObjectsWithTag("Ground");
		bottomBound = ground[0].transform.position.y;

		GameObject[] ceiling = GameObject.FindGameObjectsWithTag("Ceiling");
		topBound = ceiling[0].transform.position.y;

		GameObject[] leftwall = GameObject.FindGameObjectsWithTag("LeftWall");
		leftBound = leftwall[0].transform.position.x;

		GameObject[] rightwall = GameObject.FindGameObjectsWithTag("RightWall");
		rightBound = rightwall[0].transform.position.x;

		levelWidth = Mathf.Abs (rightBound - leftBound);
		levelHeight = Mathf.Abs (topBound - bottomBound);
	}

	public Vector3 CheckCameraPos(Vector3 checkVec, float height)
	{
		Vector3 _checkVec = checkVec;

		float ydistance = height; // this is half the height of the screen
		float xdistance = (height) * camera.aspect; //this should be half the width of the screen

		//check bottom
		if ((_checkVec.y - ydistance) < bottomBound) {
			_checkVec.y = bottomBound + ydistance;		
		}

		//check top
		if ((_checkVec.y + ydistance) > topBound) {
			_checkVec.y = topBound - ydistance;		
		}

		//check left
		if ((_checkVec.x - xdistance) < leftBound) {
			Debug.Log("Left wall");
			_checkVec.x = leftBound + xdistance;		
		}

		//check right
		if ((_checkVec.x + xdistance) > rightBound) {
			_checkVec.x = rightBound - xdistance;		
		}
	
	
		Vector3 finalVector = _checkVec;
		
		return finalVector;
	}

	public float CheckCameraZoom (float height)
	{
		float ydistance = height*2f; // this is the height of the screen
		float xdistance = (height*2) * camera.aspect; //this should be the width of the screen


		//Check cam width;
	/*	if (xdistance > levelWidth) {

			float clampHeight = (levelWidth * 0.5f) / camera.aspect;
			Debug.Log("Too Wide!:" +clampHeight);
			return clampHeight;		
		}*/
		if (ydistance > levelHeight) {
			
			float clampHeight = levelHeight * 0.5f;
			//Debug.Log("Too Tall!:"+clampHeight);

			return clampHeight;		
		}
		else{
			return height;
			//Debug.Log("Just Right!:"+height);
		}
	}



	IEnumerator GetDirection()
	{
		while(true)
		{
			Vector2 moveVec = new Vector2 (playerPos.x - lastPos.x, playerPos.y - lastPos.y);

			if (moveVec != Vector2.zero){ 
				float deltaY= Mathf.Abs(lastPos.y - playerPos.y);
				float deltaX= Mathf.Abs(lastPos.x - playerPos.x);
				Vector2 tempDirection = moveVec.normalized;

				if ( deltaX >= horWiggleRoom)
				{
					direction.x = tempDirection.x;
					lastPos.x = target.transform.position.x;
				}
				if (deltaY >= vertWiggleRoom)
				{
					direction.y = tempDirection.y;
					lastPos.y = target.transform.position.y;
				}
			}



			yield return new WaitForSeconds(camRefresh);
		}
	}



}

	
