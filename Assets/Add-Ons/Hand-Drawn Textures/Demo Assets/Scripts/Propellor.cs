using UnityEngine;
using System.Collections;

public class Propellor : MonoBehaviour {

	private Transform _trans;

	// Use this for initialization
	void Start () {
		_trans = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		_trans.Rotate(new Vector3(0,30f,0));
	}
}
