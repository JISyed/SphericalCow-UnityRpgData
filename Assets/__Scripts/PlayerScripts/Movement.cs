using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour 
{
	public playerType player;
	Animator myAnimator;
	public float gravity = 20.0F;
	private Quaternion playerRotation;

	float rotationSpeed = 450f;

	float curMoveSpeed;
	public float sneakSpeed = 1f;
	public float walkSpeed = 2f;
	public float runSpeed = 4f;

	Vector3 input;
	Vector3 motion;

	void Start()
	{
		myAnimator = GetComponent<Animator>();		

		if(gameObject.tag == "Player1")
			player = playerType.Player1;
		else if(gameObject.tag == "Player2")
			player = playerType.Player2;
		else if(gameObject.tag == "Player3")
			player = playerType.Player3;
		else if(gameObject.tag == "Player4")
			player = playerType.Player4;
	}

	void Update()
	{

		//Sets Movement Direction
		CharacterController controller = GetComponent<CharacterController>();


		switch (player)
		{
		case playerType.Player1:
			if (controller.isGrounded) 
				input = new Vector3(Input.GetAxis("Player1Horizontal"), 0, Input.GetAxis("Player1Vertical"));

			if(Input.GetButton("Player1Horizontal") || Input.GetButton("Player1Vertical"))
				myAnimator.SetBool("Walking", true);
			else
				myAnimator.SetBool("Walking", false);

//			if(Input.GetButtonDown("Player1AttackMain"))
//				print ("Attacking");
//
//			if(Input.GetButtonDown("Player1AttackSecondary"))
//				print ("Secondary Attack");
//
//			if(Input.GetButtonDown("Player1Use"))
//			   print ("Use");




			break;

		case playerType.Player2:
			if (controller.isGrounded) 
				input = new Vector3(Input.GetAxis("Player2Horizontal"), 0, Input.GetAxis("Player2Vertical"));




			break;

		case playerType.Player3:
			if (controller.isGrounded) 
				input = new Vector3(Input.GetAxis("Player3Horizontal"), 0, Input.GetAxis("Player3Vertical"));




			break;

		case playerType.Player4:
			if (controller.isGrounded) 
				input = new Vector3(Input.GetAxis("Player4Horizontal"), 0, Input.GetAxis("Player4Vertical"));




			break;
		}



	// Sets Movement Speed
		if(Input.GetKey(KeyCode.LeftControl))
		{
			curMoveSpeed = sneakSpeed;
		}
		else if(Input.GetKey(KeyCode.LeftShift))
		{
		   curMoveSpeed = runSpeed;
			myAnimator.SetBool("Running", true);
		}
		else
		{
			curMoveSpeed = walkSpeed;
			myAnimator.SetBool("Running", false);

		}

	
		// Attacking

		if(Input.GetKey(KeyCode.Space))
			myAnimator.SetTrigger("Attack");



		if(controller.isGrounded)
			{
			if(input != Vector3.zero) // looks in direction of movement
			{
				playerRotation = Quaternion.LookRotation(input);
				transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle (transform.eulerAngles.y, playerRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
			}
			motion = input;
			motion *= (Mathf.Abs (input.x) == 1 && Mathf.Abs (input.z) == 1)?.7f:1;		// keeps angle speed the same as hor/vert
			motion *= curMoveSpeed;
		}

		motion += Vector3.up * -gravity;
		controller.Move(motion * Time.deltaTime);
	}
}

public enum playerType 
{
	Player1,
	Player2,
	Player3,
	Player4
}
