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
    public float meleeVelocityMagnitude = 10;
    private float dashVelocityMagnitude = 40;
    public float minDashVelocityMagnitude = 40;
    public float fullDashVelocityMagnitude = 80;
    public float Duration = .15f;
    public float Timer;
    public bool active = false;
    public float leftOverFactor = .33f;
    Vector3 dir = Vector3.right;
    Vector3 oldScale;
    public float dashChargeTimer = .5f;
    public float fullChargeTime = 2.0f;
    private float chargeRatio;
    public float chargeTimeStamp ;

    public bool temp = false;
    private Transform groundCheck;          // A position marking where to check if the player is grounded.
    public int dashCount;
    public int offgroundDashLimit = 1;
    public int meleeCount = 0;

    public GameObject dashUI;
    public GameObject dashDirIndicator;
    public float maxFreezingCooldown = 2.5f;
    public bool isFrozen = false;
    Vector3 dashDir = Vector3.right;

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
         Debug.LogFormat( "hit {0} in new melee", other.name );
        //if( active )
        //{
        //    LifeSpan otherPlayer = other.gameObject.GetComponent<LifeSpan>();

        //    if( otherPlayer )
        //    {
        //        otherPlayer.SubstactLife( Settings.s.meleeDamage );
        //        Vector3 dir = otherPlayer.transform.position - this.transform.position;

        //        otherPlayer.GetComponent<TimeController>().AddForce( this.GetComponent<Rigidbody2D>().velocity.normalized * Settings.s.bulletKnockBack );
        //    }
        //}

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (active)
        {
            Debug.LogFormat( "hit {0} with body", collision.collider.name );
            DamagePlayer( collision.collider );
        }
    }

    public void DamagePlayer( Collider2D collider )
    {
        LifeSpan otherPlayer = collider.gameObject.GetComponent<LifeSpan>();
        if( otherPlayer )
        {
            if( ignored == null )
            {
                ignored = new List<Collider2D>();
            }

            if( ignored.Contains( collider ) )
                return;

            otherPlayer.SubstactLife( Settings.s.meleeDamage );
            Vector3 dir = otherPlayer.transform.position - this.transform.position;
            otherPlayer.GetComponent<TimeController>().AddForce( this.GetComponent<Rigidbody2D>().velocity.normalized * Settings.s.bulletKnockBack );

            ignored.Add( collider );
            foreach( Collider2D subCollider in GetComponentsInChildren<Collider2D>() )
                Physics2D.IgnoreCollision( subCollider, collider, true );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( active == false && isFrozen == false )
       {
            if( myTimeController.myInputManager.GetButtonDown( Settings.c.TimeStop ) )
            {
                chargeTimeStamp = Time.time;

            }
            grounded = Physics2D.Linecast( transform.position, groundCheck.position, 1 << LayerMask.NameToLayer( "Ground" ) );
            if( grounded )
            {
                dashCount = 0;
                //conter = 0;
                // active = false;
            }

            if( myTimeController.isStopped == true )
            {
                dashDir = new Vector2( myTimeController.myInputManager.GetAxis( Settings.c.MoveXAxis ),
                    myTimeController.myInputManager.GetAxis( Settings.c.MoveYAxis ) );

                // if there is no direction controlled, aiming where the player facing at
                if( dashDir.sqrMagnitude == 0.0f )
                {
                    if( myPlayerControl.facingRight )
                        dashDir = Vector2.right;
                    else
                        dashDir = Vector2.left;
                }

                float elapsedTime = Time.time - chargeTimeStamp;
                elapsedTime = Mathf.Clamp( elapsedTime, dashChargeTimer, fullChargeTime );
                chargeRatio = ( elapsedTime / ( fullChargeTime - dashChargeTimer ) );
                // dashVelocityMagnitude is in between min and full velocity 
                dashVelocityMagnitude = fullDashVelocityMagnitude * chargeRatio + minDashVelocityMagnitude * ( 1 - chargeRatio );

                if( dashDirIndicator && dashCount < offgroundDashLimit )
                {
                    if( dashDirIndicator.activeSelf == false )
                        dashDirIndicator.SetActive( true );

                    float theta_rad = Mathf.Atan2( dashDir.y, dashDir.x );
                    float angle = ( ( theta_rad / Mathf.PI * 180 ) + 360 ) % 360;
                    //Debug.LogFormat( "Dash Dir = {0}, Angle = {1}", dashDir.ToString(), angle );

                    dashDirIndicator.transform.localRotation = Quaternion.Euler( new Vector3( 0, 0, angle ) );
                    dashDirIndicator.transform.localScale = new Vector3( 2.0f, 1.3f, 1.0f ) * ( 0.5f + chargeRatio );
                }
            }

            if( isFrozen == false && myTimeController.myInputManager.GetButtonUp( Settings.c.TimeStop ) )
            {
                if( dashDirIndicator )
                    dashDirIndicator.SetActive( false );

                if( Time.time - chargeTimeStamp < dashChargeTimer )
                {
                    currentState = state.melee;
                    activate();
                }
                else
                {
                    currentState = state.dash;
                    if( dashCount < offgroundDashLimit )
                    {
                        dashCount++;
                        activate();
                    }
                }
            }
        }
        
        if (active == true && myTimeController.isStopped == false)
        {
            float velocityImpact = currentState == state.melee ? meleeVelocityMagnitude : dashVelocityMagnitude;
            myTimeController.myRigidbody2D.velocity = dir.normalized * velocityImpact;

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

                if( dashDir.sqrMagnitude != 0.0f )
                {
                    // make it frozen for freezing cool down time
                    StartCoroutine( FreezePlayer() );
                }
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

        dir = new Vector2( myTimeController.myInputManager.GetAxis( Settings.c.MoveXAxis ),
                        myTimeController.myInputManager.GetAxis( Settings.c.MoveYAxis ) );
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
        if( dashMelee)
            Object.Destroy( dashMelee, maxFreezingCooldown * chargeRatio );

        oldScale = transform.localScale;
        float velocityImpact = currentState == state.melee ? meleeVelocityMagnitude : dashVelocityMagnitude;
        myTimeController.myRigidbody2D.velocity = dir.normalized * velocityImpact;
    }

    IEnumerator FreezePlayer()
    {
        isFrozen = true;
        myTimeController.myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds( maxFreezingCooldown * chargeRatio );
        myTimeController.myRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        isFrozen = false;
    }
}
