using UnityEngine;
using System.Collections;

public class Turbulence : MonoBehaviour {

	private Transform trans;
	private Quaternion startRotation;
	private Quaternion lastRotation;
	private float maxAngle = 10;

	// Use this for initialization
	void Start () {
	
		trans = GetComponent<Transform>();
		startRotation = lastRotation = trans.rotation;


		int mapX = 1;

		mapX = -1 * ((mapX - 1) / 2) + 6;

	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 turbulence = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f),Random.Range(-10f,10f))* Time.deltaTime;

		trans.Rotate(turbulence);

		if (Quaternion.Angle (trans.rotation, startRotation) > maxAngle)
			trans.rotation = lastRotation;

		lastRotation = trans.rotation;

	}
}
