using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
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
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.
    public float normalJumpForce = 1500f;

	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;          // Whether or not the player is grounded.
                                            //private Animator anim;					// Reference to the player's animator component.

    public float MagicNum = 6;
    private TimeController myTimeController;

    public int PortalEntry = 0;

    public GameObject heroBody;
    
    void Awake()
	{
        myAnimator = gameObject.GetComponentInChildren<Animator>();
        myTimeController = gameObject.GetComponent<TimeController>();
        myInputManager = gameObject.GetComponent<PierInputManager>();
           // Setting up references.
        groundCheck = transform.Find("groundCheck");
	}


	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		// If the jump button is pressed and the player is grounded then the player should jump.
		if(myInputManager.GetButtonDown(jumpButton) && grounded)
			jump = true;
	}


	void FixedUpdate ()
	{

		// Cache the horizontal input.
		float h = myInputManager.GetAxis( "MoveHorizontal");

        // The Speed animator parameter is set to the absolute value of the horizontal input.
        if (grounded && myTimeController.isStopped == false)
        {
            myAnimator.SetFloat("Velocity", Mathf.Abs(h));

        }
        else
        {
            myAnimator.SetFloat("Velocity", 0);

        }
        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
        {
            if(Settings.s.airControl != 1 && grounded == false)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce * Settings.s.airControl);

            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
            }
        }


        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed && grounded == true)
        {
            // ... set the player's velocity to the maxSpeed in the x axis.
            GetComponent<Rigidbody2D>().velocity  = (new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y));

            // GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y));
        }
        // If the input is moving the player right and the player is facing left...
        if (h > 0 && heroBody.transform.localScale.x < 0)
        {
            Vector3 theScale = heroBody.transform.localScale;
            theScale.x *= -1;
            heroBody.transform.localScale = theScale;
        }

        // Otherwise if the input is moving the player left and the player is facing right...
        else if(h < 0 && heroBody.transform.localScale.x > 0)
        {
            Vector3 theScale = heroBody.transform.localScale;
            theScale.x *= -1;
            heroBody.transform.localScale = theScale;

        }
			
        if(heroBody.transform.localScale.x > 0)
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }
		// If the player should jump...
		if(jump)
		{
            doJump();		
		}
	}
    public float remaningForce;
    public float JumpMultiplier = 6;

    public void doJump()
    {
        if(grounded == true)
        {
            remaningForce = jumpForce * (JumpMultiplier); ;
            int i = Random.Range(0, jumpClips.Length);
            AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
        }
        if(myTimeController.timeScale != 1)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, remaningForce));

        }

        if (myTimeController.timeScale < 1) // speed down
        {
            remaningForce -= jumpForce * myTimeController.timeScale;

        }
        else if (myTimeController.timeScale > 1) //speed up
        {
            remaningForce -= jumpForce;//* myGravity.timeScale ;

        }
        else  // normal speed
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, normalJumpForce));
            jump = false;
            return;
            //remaningForce -= jumpForce + MagicNum;
        }
        if (remaningForce <= 0)
        {
            jump = false;
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


	public IEnumerator Taunt()
	{
		// Check the random chance of taunting.
		float tauntChance = Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds(tauntDelay);

			// If there is no clip currently playing.
			if(!GetComponent<AudioSource>().isPlaying)
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom();

				// Play the new taunt.
				GetComponent<AudioSource>().clip = taunts[tauntIndex];
				GetComponent<AudioSource>().Play();
			}
		}
	}


	int TauntRandom()
	{
		// Choose a random index of the taunts array.
		int i = Random.Range(0, taunts.Length);

		// If it's the same as the previous taunt...
		if(i == tauntIndex)
			// ... try another random taunt.
			return TauntRandom();
		else
			// Otherwise return this index.
			return i;
	}
}
