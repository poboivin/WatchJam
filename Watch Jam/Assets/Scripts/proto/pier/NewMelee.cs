using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMelee : MonoBehaviour
{
    public enum state { dash, melee}
    public state currentState;
    [HideInInspector]
    public PierInputManager myInputManager;
    [HideInInspector]
    public LifeSpan myLifeSpan;
    [HideInInspector]
    public TimeController myTimeController;
    [HideInInspector]
    public TimeStopController myTimeStopController;
    [HideInInspector]

    public TimeRewindController myTimeRewindController;

    PlayerControl myPlayerControl;
    public float VelocityMagnitude = 50;
    public float Duration = .15f;
    public float Timer;
    public bool active = false;
    public float leftOverFactor = .33f;
    Vector3 dir = Vector3.right;
    Vector3 oldScale;
    public float chargeTimer = 1;
    public float chargeTimeStamp ;
    public bool temp = false;
    private Transform groundCheck;          // A position marking where to check if the player is grounded.
    public int dashCount;
    public int offgroundDashLimit = 1;
    public int meleeCount = 0;

    public GameObject dashUI;

    private List<Collider2D> ignored;
    private bool grounded = false;
    // Start is called before the first frame update
    void Start()
    {
        myInputManager = GetComponent<PierInputManager>();
        myLifeSpan = GetComponent<LifeSpan>();
        myTimeController = GetComponent<TimeController>();
        myPlayerControl = GetComponent<PlayerControl>();
        myTimeStopController = GetComponent<TimeStopController>();
        myTimeRewindController = GetComponent<TimeRewindController>();
        oldScale = transform.localScale;
        groundCheck = transform.Find("groundCheck");

    }
    private void OnTriggerEnter(Collider other)
    {
         Debug.Log(other.name);
        //if (active)
        //{
        //    LifeSpan otherPlayer = other.gameObject.GetComponent<LifeSpan>();

        //    if (otherPlayer)
        //    {
        //        otherPlayer.SubstactLife(Settings.s.meleeDamage);
        //        Vector3 dir = otherPlayer.transform.position - this.transform.position;

        //        otherPlayer.GetComponent<TimeController>().AddForce(this.GetComponent<Rigidbody2D>().velocity.normalized * Settings.s.bulletKnockBack);
        //    }
        //}

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       // Debug.Log(collision.collider.name);
        if (active)
        {
            LifeSpan otherPlayer = collision.collider.gameObject.GetComponent<LifeSpan>();

            if (otherPlayer)
            {
                if(currentState == state.melee)
                {
                    otherPlayer.SubstactLife(Settings.s.meleeDamage);
                    Vector3 dir = otherPlayer.transform.position - this.transform.position;

                    otherPlayer.GetComponent<TimeController>().AddForce(this.GetComponent<Rigidbody2D>().velocity.normalized * Settings.s.bulletKnockBack);
                }
             


                if (ignored == null)
                {
                    ignored = new List<Collider2D>();
                }

                ignored.Add(collision.collider);
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.collider,true);
                
            }
        }
      
    }
    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (grounded && active == false)
        {
            dashCount = 0;
            //conter = 0;
            // active = false;
        }
        if (myTimeController.myInputManager.GetButtonDown(Settings.c.TimeStop))
        {
            chargeTimeStamp = Time.time;
            
        }
        if (myTimeController.myInputManager.GetButtonUp(Settings.c.TimeStop) && active == false)
        {
            if(Time.time - chargeTimeStamp >= chargeTimer)
            {
                currentState = state.melee;
                activate();

            }
            else
            {
                
                
                currentState = state.dash;
                if(dashCount < offgroundDashLimit)
                {
                    dashCount++;
                    activate();

                }
                



            }
            
        }
       
        if (active == true && myTimeController.isStopped == false)
        {

            myTimeController.myRigidbody2D.velocity = dir.normalized * VelocityMagnitude;
            Timer += Time.deltaTime;
            if (currentState == state.melee)
            {
                transform.localScale = new Vector3(.7f, .2f, 1);
            }
            if (Timer >= Duration)
            {
                transform.localScale = oldScale;

                deactivate();
                myTimeController.myRigidbody2D.velocity *= leftOverFactor;


            }
        }
        else
        {
            deactivate();
        }
    }
    public void deactivate()
    {
        Timer = 0;

        active = false;
        myLifeSpan.SetInvincibility(false);
        myTimeStopController.enabled = true;
        myTimeRewindController.enabled = true;
        if(ignored != null && ignored.Count > 0)
        {
            foreach(Collider2D c in ignored)
            {
                if(c!= null)
                {
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), c, false);

                }

            }
            ignored.Clear();
        }
    }
    public void activate()
    {
        active = true;
        dir = new Vector2(myTimeController.myInputManager.GetAxis(Settings.c.MoveXAxis), myTimeController.myInputManager.GetAxis(Settings.c.MoveYAxis));
        myTimeStopController.enabled = false;
        myTimeRewindController.enabled = false;
        if (currentState == state.melee)
        {
            myLifeSpan.SetInvincibility(true);
            if (dir.sqrMagnitude == 0)
            {
                if (myPlayerControl.facingRight == false)
                {
                    dir = -Vector3.right;
                }
                else
                {
                    dir = Vector3.right;
                }
            }
        }
       
        if (myTimeController.isStopped)
        {
            myTimeController.StopTimeStop();
        }
        if (myTimeController.isRewinding)
        {
            myTimeController.StopRewind();

        }

        GameObject dashMelee = Instantiate( dashUI, gameObject.transform );
        Object.Destroy( dashMelee, 3.0f );

        oldScale = transform.localScale;
        myTimeController.myRigidbody2D.velocity = dir.normalized * VelocityMagnitude;
    }
}
