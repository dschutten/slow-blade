using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public Text toggleText;
	private bool camSplit;

	public GameObject cameraObject;
	private ZoomController zoomController;

	public GameObject blade;
	public GameObject player;

	// Use this for initialization
	void Start () {
		zoomController = cameraObject.GetComponent <ZoomController>();
		toggleText.text = "Press 'T' to see Blade.";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.T)) 
		{
			camSplit = !camSplit;
			switchCamMode();
		}


	}

	void switchCamMode (){
		if (camSplit){
			zoomController.blade = 	blade.transform;
			toggleText.text = "Press 'T' to ignore the Blade.";
		}else{
			zoomController.blade = player.transform;
			toggleText.text = "Press 'T' to see Blade.";
		}
	}



}
