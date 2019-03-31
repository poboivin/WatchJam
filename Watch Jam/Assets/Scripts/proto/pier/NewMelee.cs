using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMelee : MonoBehaviour
{
    [HideInInspector]
    public PierInputManager myInputManager;
    [HideInInspector]
    public LifeSpan myLifeSpan;
    [HideInInspector]
    public TimeController myTimeController;
    [HideInInspector]
    public TimeStopController myTimeStopController;
    [HideInInspector]
    Rigidbody2D myRigidbody;

    public TimeRewindController myTimeRewindController;

    PlayerControl myPlayerControl;
    private float currentDashForce = 0;
    public float minDashForce = 0;
    public float fullDashForce = 4000;
    public float DashingDuration = .2f;
    private float Timer;
    public bool active = false;
    public Vector2 leftOverFactor = new Vector2( 0.2f, 0.0f );
    Vector3 oldScale;
    public float fullChargeTime = 2.0f;
    private float chargeRatio;
    private float chargeStartTimeStamp;

    public bool temp = false;
    private Transform groundCheck;          // A position marking where to check if the player is grounded.
    public int dashCount;
    public int offgroundDashLimit = 1;

    public GameObject dashUI;
    public GameObject dashDirIndicator;
    public float maxFreezingCooldown = 2.5f;
    public bool isFrozen = false;
    Vector3 dashDir = Vector3.right;

    private List<Collider2D> ignored;
    private bool grounded = false;
    float oldGravityScale;
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
        myRigidbody = GetComponent<Rigidbody2D>();
        oldGravityScale = myRigidbody.gravityScale;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogFormat( "hit {0} in new melee", other.name );
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
            //Debug.LogFormat( "hit {0} with body", collision.collider.name );
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

    void ChargeDash()
    {
        dashDir = GetAimingDirection();
        // if there is no direction controlled, aiming where the player facing at
        if( dashDir.sqrMagnitude == 0.0f )
        {
            if( myPlayerControl.facingRight )
                dashDir = Vector2.right;
            else
                dashDir = Vector2.left;
        }

        chargeRatio = GetChargeRate();
        // ease-in
        float magnitudeRatio = chargeRatio * chargeRatio;
        currentDashForce = fullDashForce * magnitudeRatio + minDashForce * ( 1 - magnitudeRatio );

        if( dashDirIndicator && dashCount < offgroundDashLimit )
        {
            if( dashDirIndicator.activeSelf == false )
                dashDirIndicator.SetActive( true );

            float theta_rad = Mathf.Atan2( dashDir.y, dashDir.x );
            float angle = ( ( theta_rad / Mathf.PI * 180 ) + 360 ) % 360;
            //Debug.LogFormat( "Dash Dir = {0}, Angle = {1}", dashDir.ToString(), angle );

            dashDirIndicator.transform.localRotation = Quaternion.Euler( new Vector3( 0, 0, angle ) );
            dashDirIndicator.transform.localScale = new Vector3( 1.0f, 2.0f, 1.0f ) * ( 0.5f + chargeRatio );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( active == false )
        {
            if( myTimeController.myInputManager.GetButtonDown( Settings.c.TimeStop ) )
            {
                chargeStartTimeStamp = Time.time;
            }

            grounded = Physics2D.Linecast( transform.position, groundCheck.position, 1 << LayerMask.NameToLayer( "Ground" ) );
            // can dash?
            if( isFrozen == false )
            {
                if( grounded )
                {
                    dashCount = 0;
                }

                // if it's charging
                if( myTimeController.isStopped == true )
                {
                    ChargeDash();
                }

                // start to dash
                if( myTimeController.myInputManager.GetButtonUp( Settings.c.TimeStop ) )
                {
                    if( dashDirIndicator )
                        dashDirIndicator.SetActive( false );

                    if( grounded )
                    {
                        activate();
                    }
                    else if( dashCount < offgroundDashLimit )
                    {
                        dashCount++;
                        activate();
                    }
                }
            }
            else
            {
                if( grounded )
                {
                    // not to make it jump higher, if it's dashed near ground
                    RestoreGravityScale();
                }
            }
        }
        else
        {
            // being dashed
            if( myTimeController.isStopped == false )
            {
                Timer += Time.deltaTime;

                // still needs this?
                transform.localScale = new Vector3(.7f, .2f, 1);

                if( Timer >= DashingDuration )
                {
                    transform.localScale = oldScale;

                    deactivate();
                    myRigidbody.velocity *= leftOverFactor;

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

        myTimeStopController.enabled = false;
        myTimeRewindController.enabled = false;
        myLifeSpan.SetInvincibility( true );

        dashDir = GetAimingDirection();
        if ( dashDir.sqrMagnitude == 0)
        {
            if (myPlayerControl.facingRight)
            {
                dashDir = Vector3.right;
            }
            else
            {
                dashDir = Vector3.left;
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
            Destroy( dashMelee, maxFreezingCooldown * chargeRatio );

        oldScale = transform.localScale;

        // change the way of dashing to give a force instead of chainging a velocity
        // this way becomes easier to control the gravity scale
        //Debug.LogFormat( "Dash force = {0}", currentDashForce );
        myRigidbody.AddForce( dashDir.normalized * currentDashForce );
    }

    IEnumerator FreezePlayer()
    {
        //Debug.Log( "Freeze" );
        isFrozen = true;
        myRigidbody.gravityScale = 0.01f;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds( maxFreezingCooldown * chargeRatio );
        //Debug.Log( "Unfreeze" );
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        RestoreGravityScale();
        isFrozen = false;
    }

    public void RestoreGravityScale()
    {
        myRigidbody.gravityScale = oldGravityScale;
    }

    Vector2 GetAimingDirection()
    {
        Vector2 aimingDir = new Vector2( 0.0f, 0.0f ) ;
        if( myInputManager.GetAxis( Settings.c.MainAimXAxis ) != 0 || 
            myInputManager.GetAxis( Settings.c.MainAimYAxis ) != 0 )
        {
            aimingDir.x = myInputManager.GetAxis( Settings.c.MainAimXAxis );
            aimingDir.y = myInputManager.GetAxis( Settings.c.MainAimYAxis );
        }
        else if( myInputManager.GetAxis( Settings.c.AltAimXAxis ) != 0 || 
            myInputManager.GetAxis( Settings.c.AltAimYAxis ) != 0 )
        {
            aimingDir.x = myInputManager.GetAxis( Settings.c.AltAimXAxis );
            aimingDir.y = myInputManager.GetAxis( Settings.c.AltAimYAxis );
        }
        return aimingDir;
    }

    // returns charge rate between min(0) to max(1)
    float GetChargeRate()
    {
        float elapsedTime = Time.time - chargeStartTimeStamp;
        elapsedTime = Mathf.Clamp( elapsedTime, 0, fullChargeTime );
        float ratio = elapsedTime / fullChargeTime;
        return ratio;
    }
}
