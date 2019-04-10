using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour,ImouseAble
{
    [HideInInspector]
    public PierInputManager myInputManager;
  //  public PierInputManager.ButtonName jumpButton;
   // [HideInInspector]

    public Animator myAnimator;
    [HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public AudioClip[] footsteps;			// Array of clips for when the player is running.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.
    public float normalJumpForce = 1500f;

	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck1;
    private Transform groundCheck2;
    private Transform groundCheck3;          // A position marking where to check if the player is grounded.
    public bool grounded = false;          // Whether or not the player is grounded.
                                            //private Animator anim;					// Reference to the player's animator component.

    public float MagicNum = 6;
    private TimeController myTimeController;

    public int PortalEntry = 0;

    public GameObject heroBody;
    public Collider2D myCollider;

    public LayerMask playerMask;

    public bool BouceOffPlayers = true;
    public float BounceOffYValue = 5.0f;
    public float BounceOffXValue = 2.5f;
    
    void Awake()
	{
        //myAnimator = gameObject.GetComponentInChildren<Animator>();
        myTimeController = gameObject.GetComponent<TimeController>();
        myInputManager = gameObject.GetComponent<PierInputManager>();
           // Setting up references.
        groundCheck1 = transform.Find("groundCheck1");
        groundCheck2 = transform.Find("groundCheck2");
        groundCheck3 = transform.Find("groundCheck3");
    }


    void Update()
    {
        grounded = false;
        if (GetComponent<Rigidbody2D>().velocity.y <= 0.1f)
        {
          if (Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
                    Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Ground")) ||
                    Physics2D.Linecast(transform.position, groundCheck3.position, 1 << LayerMask.NameToLayer("Ground")))
                {
                    grounded = true;
                }
                else
                {
                    grounded = false;
                    if(BouceOffPlayers)
                    {
                        BounceOff();
                    }
                    else
                    {

                    }
            
                }
        }
      

        // If the jump button is pressed and the player is grounded then the player should jump.
        if ((myInputManager.GetButtonDown(Settings.c.jumpButton) || myInputManager.GetButtonDown(Settings.c.AltjumpButton)) && grounded)
			jump = true;
            
	}


	void FixedUpdate ()
	{
        if(myAnimator != null)
        {
            myAnimator.SetBool("Grounded", grounded);
            myAnimator.SetFloat("Y Velocity", GetComponent<Rigidbody2D>().velocity.y);
        }
       
		// Cache the horizontal input.
		float h = myInputManager.GetAxis( Settings.c.MoveXAxis);
        float h2 = myInputManager.GetAxis(Settings.c.MainAimXAxis);


        // The Speed animator parameter is set to the absolute value of the horizontal input.
        if (grounded && myTimeController.isStopped == false)
        {
            if(myAnimator != null)
            {
                myAnimator.SetFloat("Velocity", Mathf.Abs(h));

            }


        }
        else
        {
            if (myAnimator != null)
            {
                myAnimator.SetFloat("Velocity", 0);
            }
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
        if (h2 > 0 && heroBody.transform.localScale.x < 0)
        {
            Vector3 theScale = heroBody.transform.localScale;
            theScale.x *= -1;
            heroBody.transform.localScale = theScale;
        }

        // Otherwise if the input is moving the player left and the player is facing right...
        else if(h2 < 0 && heroBody.transform.localScale.x > 0)
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
            if(myAnimator != null)
            {
                myAnimator.SetTrigger("JumpTrigger");

            }
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


	/*public IEnumerator Taunt()
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
				//GetComponent<AudioSource>().clip = taunts[tauntIndex];
				GetComponent<AudioSource>().Play();
			}
		}
	}*/


	/*int TauntRandom()
	{
		// Choose a random index of the taunts array.
		//int i = Random.Range(0, taunts.Length);

		// If it's the same as the previous taunt...
		//if(i == tauntIndex)
			// ... try another random taunt.
			//return TauntRandom();
		//else
			// Otherwise return this index.
			//return i;
	}*/
    void ImouseAble.setEnable(bool val)
    {
        this.enabled = val;
    }

    void BounceOff()
    {
        RaycastHit2D[] groundhit1 = Physics2D.LinecastAll(transform.position, groundCheck1.position, playerMask);
        RaycastHit2D[] groundhit2 = Physics2D.LinecastAll(transform.position, groundCheck2.position, playerMask);
        RaycastHit2D[] groundhit3 = Physics2D.LinecastAll(transform.position, groundCheck3.position, playerMask);

        foreach (RaycastHit2D r1 in groundhit1)
        {
            if (r1.collider != null && r1.collider != myCollider)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(BounceOffXValue, BounceOffYValue),ForceMode2D.Impulse);
            }
        }
        foreach (RaycastHit2D r2 in groundhit2)
        {
            if (r2.collider != null && r2.collider != myCollider)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, BounceOffYValue*2), ForceMode2D.Impulse);
            }
        }
        foreach (RaycastHit2D r3 in groundhit3)
        {
            if (r3.collider != null && r3.collider != myCollider)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-BounceOffXValue, BounceOffYValue), ForceMode2D.Impulse);
            }
        }
    }
}
