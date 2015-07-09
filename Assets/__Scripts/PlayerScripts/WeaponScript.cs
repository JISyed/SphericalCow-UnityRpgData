using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour 
{
	public weaponType weaponBeingUsed;

	Renderer rend;

	int PlayerNumber;
	public GameObject playerPlane;

	public int DmgMin;
	int DmgMax;
	float rateOfFire;
	float range;

	public Texture[] PistolGuy;
	public Texture[] RifleGuy;
	public Texture[] ShotgunGuy;
	public Texture[] BazokaGuy;

	void Start()
	{
		if(gameObject.tag == "Player1")
			PlayerNumber = 0;
		else if(gameObject.tag == "Player2")
			PlayerNumber = 1;
		else if(gameObject.tag == "Player3")
			PlayerNumber = 2;
		else if(gameObject.tag == "Player4")
			PlayerNumber = 3;

		rend = playerPlane.GetComponent<Renderer>();
		rend.material.mainTexture = PistolGuy[PlayerNumber];

	}

	void Attack()
	{

	}

	void Update()
	{
		if(Input.GetKey(KeyCode.Alpha1))
		{
		   rend.material.mainTexture = PistolGuy[PlayerNumber];
			weaponBeingUsed = weaponType.Pistol;
		}
		else if(Input.GetKey(KeyCode.Alpha2))
		{
		   rend.material.mainTexture = RifleGuy[PlayerNumber];
			weaponBeingUsed = weaponType.Rifle;

		}
		else if(Input.GetKey(KeyCode.Alpha3))
		{
			rend.material.mainTexture = ShotgunGuy[PlayerNumber];
			weaponBeingUsed = weaponType.Shotgun;

		}
		else if(Input.GetKey(KeyCode.Alpha4))
		{
			rend.material.mainTexture = BazokaGuy[PlayerNumber];
			weaponBeingUsed = weaponType.Bazooka;

		}

		switch(weaponBeingUsed)
		{
		case weaponType.Pistol:

			DmgMin = 5;
			DmgMax = 25;
			range = 5f;

			break;

		case weaponType.Rifle:

			DmgMin = 10;
			DmgMax = 35;
			range = 10f;

			break;

		case weaponType.Shotgun:

			DmgMin = 30;
			DmgMax = 50;
			range = 2.5f;

			break;

		case weaponType.Bazooka:

			break;

		}
	}

	// These properties were written to remove
	// some warnings about unused variables
	public int DamageMax
	{
		get
		{
			return this.DmgMax;
		}
	}

	public float Range
	{
		get
		{
			return this.range;
		}
	}

} // End WeaponScript

public enum weaponType
{
	Pistol,
	Rifle,
	Shotgun,
	Bazooka
}