using UnityEngine;
using System.Collections;

public class KinematicPlayerControl : MonoBehaviour
{
	[HideInInspector]
	public PierInputManager myInputManager;
	public PierInputManager.ButtonName jumpButton;
	[HideInInspector]

	public Animator myAnimator;
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	public float maxSpeed = 15f;			// The player ground movespeed.
	public float airSpeed = 25f;// The player air movespeed.
	public float jumpHeight = 9.0f;
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;          // Whether or not the player is grounded.
	//private Animator anim;				// Reference to the player's animator component.

	//public float MagicNum = 6; ?
	private TimeController myTimeController;
	private Rigidbody2D rb;
	private float speed = 0;
	private float jumpTime = 0f;			//used to count up to max timelength
	private float jumpTimeMax;				//Maximum timelength of a jump
	private float aSpeed;  					//variable to change airspeed in time zones



	public int PortalEntry = 0;

	void Awake()
	{
		myAnimator = gameObject.GetComponentInChildren<Animator>();
		myTimeController = gameObject.GetComponent<TimeController>();
		myInputManager = gameObject.GetComponent<PierInputManager>();
		groundCheck = transform.Find("groundCheck");
		rb = GetComponent<Rigidbody2D> ();

		//rb.bodyType = RigidbodyType2D.Kinematic;  \\This would make the player fall through the map//
	}


	void Update()
	{

		if (myTimeController.timeScale != 1) //if in a timezone, change air speed so height will remain the same
			aSpeed = airSpeed * myTimeController.timeScale;
		else
			aSpeed = airSpeed;
		
		jumpTimeMax = jumpHeight/aSpeed; // always calculate jump time in regards to speed, to keep the same height in case of time zone.  9.0/25.0f in this instance

		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		// If the jump button is pressed and the player is grounded then the player should jump.
		if (myInputManager.GetButtonDown (jumpButton) && grounded)
		{
			jump = true;
		}

		if (myInputManager.GetButtonUp (jumpButton) || jumpTime >= jumpTimeMax)  //ends the jump after button is let go or after a max amount of time
		{
			jump = false;
			jumpTime = 0.0f;
		}

		if (jump)
		{
			jumpTime += Time.deltaTime;  //Times the jump
		}

		if (Time.timeScale < 1) {

		}
	}


	void FixedUpdate ()
	{

		// Cache the horizontal input.
		float h = myInputManager.GetAxis( "Horizontal");
			
		// The Speed animator parameter is set to the absolute value of the horizontal input.
		if (grounded && myTimeController.isStopped == false)
		{
			myAnimator.SetFloat("Velocity", Mathf.Abs(h));

		}
		else
		{
			myAnimator.SetFloat("Velocity", 0);
		}

		if (h == 0) //if no input, dont move. (with Mathf.Sign(h), 0 is technically positive so it will move without input)
			speed = 0f;
		else
			speed = maxSpeed;
		
	

		Vector2 xMovement = Mathf.Sign (h) * new Vector2 (1, 0) * speed * myTimeController.timeScale * Time.deltaTime;
		Vector2 yMovement = new Vector2 (0, 1) * aSpeed *  Time.deltaTime;

		if (!myTimeController.isStopped && !myTimeController.isRewinding) //if time isn't stopped or rewinding
		{
			if (grounded)  //if on the ground
			{
				rb.MovePosition (rb.position + xMovement); //move normally
			} 
			if (jump)//if in the air, while jumping
			{ 
				rb.MovePosition (rb.position + xMovement /* *Settings.s.airControl*/ + yMovement);
			} 
			if (!grounded && !jump) //if in the air, while falling
			{
				rb.MovePosition (rb.position + xMovement /* *Settings.s.airControl*/ - yMovement);
			}
		}
			
		if (h > 0 &&  transform.localScale.x < 0)
		{
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;

		}

		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && transform.localScale.x > 0)
		{
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		if( transform.localScale.x > 0)
		{
			facingRight = true;

		}
		else
		{
			facingRight = false;
		}

	}


	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


}
