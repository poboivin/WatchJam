using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public partial class Gun : MonoBehaviour,IGun
{
    public class bulletInfo
    {
        public Rigidbody2D body;
        public Vector2 vel;

        public bulletInfo(Rigidbody2D body, Vector2 vel)
        {
            this.body = body;
            this.vel = vel;
        }
    }
    public List<bulletInfo> bullets;
    private PierInputManager myInputManager;
    private Ammo myAmmo;
    private LifeSpan myLifeSpan;
    private PlayerControl myPlayerControl;       // Reference to the PlayerControl script.
    private TimeController myTimeController;
  
   // public PierInputManager.ButtonName ShootButton;   //button to shoot

    public Rigidbody2D rocket;              // Prefab of the rocket.
    private Rigidbody2D OriginalRocket;              // Prefab of the rocket.

    public float speed = 20f;				// The speed the rocket will fire at.
    public float fireRate = 0.3f;
    private float nextFire = 0f;
    private int numBoostedBullet = 0;       // how many bullets are able to fire with boosted fire rate
    public Transform gunPivot;
    public Transform MuzzleFlashPrefab;
    public SpriteRenderer armSprite;


    private GameObject[] RocketsFired;
    public SpecialBarController SpecialBar;
    

    public float angle;
    public float x;
    public float y;

    
    void Awake()
    {
        bullets = new List<bulletInfo>();
        myInputManager = GetComponentInParent<PierInputManager>();
        myLifeSpan = GetComponentInParent<LifeSpan>();
        myTimeController = GetComponentInParent<TimeController>();
        myAmmo = GetComponentInParent<Ammo>();
        // Setting up the references.
        //anim = transform.root.gameObject.GetComponent<Animator>();
        myPlayerControl = transform.root.GetComponent<PlayerControl>();
        OriginalRocket = rocket;
    }
    void Update()
    {
        bool rightStickUsed = false;
        if (myInputManager.GetAxis(Settings.c.MainAimXAxis)!= 0 || myInputManager.GetAxis(Settings.c.MainAimYAxis) != 0)
        {
            x = myInputManager.GetAxis(Settings.c.MainAimXAxis);
            y = myInputManager.GetAxis(Settings.c.MainAimYAxis);
            rightStickUsed = true;
          
        }
        else if (myInputManager.GetAxis(Settings.c.AltAimXAxis) != 0 || myInputManager.GetAxis(Settings.c.AltAimYAxis) != 0)
        {
            x = myInputManager.GetAxis(Settings.c.AltAimXAxis);
            y = myInputManager.GetAxis(Settings.c.AltAimYAxis);
        }
      

        float theta_rad = Mathf.Atan2(y, x);
        float theta_deg = (theta_rad / Mathf.PI * 180) + (theta_rad > 0 ? 0 : 360);
        angle = theta_deg;//(angle + 360) % 360;


        //if (myPlayerControl.facingRight && gunPivot.transform.localScale.x < 0.0f)
        //{
        //    Debug.Log("facing Right");
        //    Vector3 theScale = gunPivot.transform.localScale;
        //    theScale.x *= -1;
        //    gunPivot.transform.localScale = theScale;
        //}
        //else if(!myPlayerControl.facingRight && gunPivot.transform.localScale.x > 0.0f)
        //{
        //    Debug.Log("facing Left");
        //    Vector3 theScale = gunPivot.transform.localScale;
        //    theScale.x *= -1;
        //    gunPivot.transform.localScale = theScale;

        //}
        if (SpecialBar.RapidFireFill.enabled == true)
        {
            SpecialBar.SetRapidBarFill(numBoostedBullet);
        }

        gunPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, angle)); //Rotating!

        bool shoot = myTimeController.isRewinding == false && 
            ( myInputManager.GetButtonDown(Settings.c.ShootButton) || myInputManager.GetButtonDown(Settings.c.AltShootButton) || ( rightStickUsed && Settings.c.AutoRStickShoot )) && 
            ( Time.time > nextFire || (myTimeController.isStopped == true && Settings.s.stopTimeStoreBullet == true ) );

        // If the fire button is pressed...
        if (shoot)
        {
            nextFire = Time.time + fireRate;
            Rigidbody2D prefab = null;
            if (myAmmo.Grenade != null)
            {

                prefab = myAmmo.Grenade;
                myAmmo.Grenade = null;

                // Debug.Log("GRENADE");
            }
            else if (Settings.s.unlimitedAmmo == true)
            {
                prefab = rocket;
            }
            else if (myAmmo.CurrentAmmo > 0 )
            {
                myAmmo.CurrentAmmo--;
                prefab = rocket;
            }
            

            if (prefab != null)
            {
                 // ... set the animator Shoot trigger parameter and play the audioclip.
                //	anim.SetTrigger("Shoot");
                GetComponent<AudioSource>().Play();
                Vector2 dir = Vector2.zero;

                //// If the player is facing right...
                //if (myPlayerControl.facingRight)
                //{
                     // ... instantiate the rocket facing right and set it's velocity to the right. 
                    Rigidbody2D bulletInstance = Instantiate(prefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                    dir = new Vector2(transform.right.x, transform.right.y) * speed;

                    if (prefab == rocket)
                    {
                        bulletInstance.GetComponent<BulletLeach>().myOwner = myLifeSpan;
                        bulletInstance.GetComponent<Rocket>().myOwner = myLifeSpan;

                    }
                    if (myTimeController.isStopped == true  && Settings.s.stopTimeStoreBullet == true)
                    {
                        if (bulletInstance != null)
                        {
                            if (bulletInstance.GetComponent<Rocket>() && bulletInstance.GetComponent<BoxCollider2D>())
                            {
                                bulletInstance.GetComponent<Rocket>().enabled = false;

                                bulletInstance.GetComponent<BoxCollider2D>().enabled = false;
                                bullets.Add(new bulletInfo(bulletInstance, dir));
                            }
                                
                        }

                    }
                    else
                    {
                   // Debug.Log("Bullets Are Here");
                        bulletInstance.velocity = dir;//new Vector2(speed, 0);
                        bulletInstance.transform.right = bulletInstance.velocity;
                        foreach( Collider2D collider in transform.root.GetComponentsInChildren<Collider2D>() )
                        {
                            Physics2D.IgnoreCollision( bulletInstance.GetComponent<Collider2D>(), collider );
                        }
                    }

                // FIX THIS: Codes below looks soooo redundant. is there any reason for that?

                //    /*
                //    //instantiate muzzle flash
                //    Transform clone = Instantiate(MuzzleFlashPrefab, gunPivot.position, gunPivot.rotation) as Transform;
                //    clone.parent = gunPivot;
                //    float size = Random.Range(0.6f, 0.9f);
                //    clone.localScale = new Vector3(size, size, 0);
                //    //Destroy(clone, 0.02f);
                //    Destroy(clone);
                //    */
                ////}
                ////else
                ////{
                //    // Otherwise instantiate the rocket facing left and set it's velocity to the left.

                //    //Rigidbody2D bulletInstance = Instantiate(prefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
                //    dir = new Vector2(transform.right.x, transform.right.y) * speed;
                //    if (prefab == rocket)
                //    {
                //            bulletInstance.GetComponent<BulletLeach>().myOwner = myLifeSpan;
                //            bulletInstance.GetComponent<Rocket>().myOwner = myLifeSpan;


                //    }
                //    if (myTimeController.isStopped == true && Settings.s.stopTimeStoreBullet == true)
                //    {
                //        if (bulletInstance != null)
                //        {
                //            if (bulletInstance.GetComponent<Rocket>() && bulletInstance.GetComponent<BoxCollider2D>())
                //            {
                //                bulletInstance.GetComponent<Rocket>().enabled = false;

                //                bulletInstance.GetComponent<BoxCollider2D>().enabled = false;
                //                bullets.Add(new bulletInfo(bulletInstance, dir));
                //            }
                //        }
                       
                //    }

                //    else
                //    {
                //        bulletInstance.velocity = dir;// new Vector2(-speed, 0);
                    
                //        Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), this.GetComponentInParent<Collider2D>());
                //    }
                //}

                myTimeController.AddForce(-dir.normalized * Settings.s.gunKnockBack);
                // Debug.Log(-dir * Settings.s.gunKnockBack);


                var statistics = myInputManager.GetComponentInParent<PlayerStatistics>();
                if( statistics != null)
                    statistics.RecordFire();
            }

            if( numBoostedBullet > 0 )
                numBoostedBullet--;
        }
	}



    public void StopRocket()
    {
        
        
        
    }

    public void ReseumeRocket()
    {
        if ( Settings.s.stopTimeStoreBullet == true)
        {
            foreach (bulletInfo b in bullets)
            {
                Rigidbody2D RocketBullet = b.body;
                RocketBullet.GetComponent<Rocket>().enabled = true;
                RocketBullet.GetComponent<BoxCollider2D>().enabled = true;
                RocketBullet.velocity = b.vel;
            }

            bullets.Clear();
        }
      
    }

    public void ChangeFireRate( float newFireRate, int numBulletCount )
    {
        SpecialBar.ToggleRapidFireBar(true);
        StartCoroutine( "ChangeFireRateImpl", new object[] { newFireRate, numBulletCount } );
    }

    public IEnumerator ChangeFireRateImpl( object[] parameters )
    {
        float oldFireRate = fireRate;
        fireRate = ( float )parameters[0];
        
        // TO DO : change this effect with the proper one that showing the player is being boosted.
        //TimeAuraController aura = transform.root.gameObject.GetComponentInChildren<TimeAuraController>();
        //if( aura != null )
        //{
        //    aura.TurnOnAura( TimeAuraController.Aura.orange );
        //}

        numBoostedBullet = ( int )parameters[1];
  

        yield return new WaitUntil( () => numBoostedBullet <= 0 );

        //if( aura != null )
        //    aura.TurnOffAura();
        SpecialBar.ToggleRapidFireBar(false);
        fireRate = oldFireRate;

        rocket = OriginalRocket; 
    }
    void IGun.setEnable(bool val)
    {
        this.enabled = val;
    }
}
