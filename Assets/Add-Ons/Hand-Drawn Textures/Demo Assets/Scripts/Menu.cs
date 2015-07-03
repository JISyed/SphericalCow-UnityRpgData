using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class Menu : MonoBehaviour {
	
	// The set of replacement materials to be assigned to each gameobject (that has a renderer)  
	public GameObject[] cameraPos;

	private int i;

	private Transform cam;


	// Use this for initialization
	void Start () {
		
		//cameraPos = GameObject.FindGameObjectsWithTag("cameraPos");
		cam = Camera.main.transform;
	}

	void Update(){

		if(Input.GetKeyDown(KeyCode.RightArrow)){
			i++;
	}
		else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			i--;
		}
		i = Mathf.Clamp(i,0, cameraPos.Length-1);

		cam.position = Vector3.Lerp(cam.position, cameraPos[i].transform.position, Time.deltaTime);
	}

	// Simple menu to allow the user to switch materials	
	void OnGUI() {
		
		//GUIStyle style = new GUIStyle();
		//style.font.material.color = Color.grey;
		
		GUI.contentColor = Color.black;

		GUI.Label(new Rect(10, 10, 400, 30), "Hand-Drawn Shader");
		GUI.Label(new Rect(Screen.width-100, 10, 100, 30), "Original");


		
		GUI.Label(new Rect(10, Screen.height-30, 400, 30), "Unity Hand-Drawn Shader Demo. (c) 2015 Alastair Aitchison");
		GUI.Label(new Rect(10, Screen.height-50, 400, 30), "Press left/right arrows to scroll through example materials");



	}


}
