using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour
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

    public PierInputManager.ButtonName ShootButton;   //button to shoot


    public Rigidbody2D rocket;              // Prefab of the rocket.

    public float speed = 20f;				// The speed the rocket will fire at.
    public float fireRate = 0.3f;
    private float nextFire = 0f;
    public Transform gunPivot;
    public Transform MuzzleFlashPrefab;

    
    private GameObject[] RocketsFired;

    public float angle;
    public float x;
    public float y;
	void Awake()
	{
        bullets = new List<bulletInfo>();
        myInputManager = GetComponentInParent<PierInputManager>();
        myLifeSpan = GetComponentInParent<LifeSpan>();
        myTimeController = GetComponentInParent<TimeController>();
        myAmmo  = GetComponentInParent<Ammo>();
        // Setting up the references.
        //anim = transform.root.gameObject.GetComponent<Animator>();
        myPlayerControl = transform.root.GetComponent<PlayerControl>();
	}
    void Update()
    {
        x = Mathf.Abs(myInputManager.GetAxis("Horizontal"));
        y = myInputManager.GetAxis("Vertical");
        float theta_rad = Mathf.Atan2(y, x);
        float theta_deg = (theta_rad / Mathf.PI * 180) + (theta_rad > 0 ? 0 : 360);
        angle = theta_deg;//(angle + 360) % 360;



        //Debug.Log(angle + "");


        gunPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, angle)); //Rotating!


        // If the fire button is pressed...
        if (myTimeController.isRewinding == false && myInputManager.GetButtonDown(ShootButton) && Time.time > nextFire)
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

                // If the player is facing right...
                if (myPlayerControl.facingRight)
                {
                     // ... instantiate the rocket facing right and set it's velocity to the right. 
                    Rigidbody2D bulletInstance = Instantiate(prefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                    dir = new Vector2(transform.right.x, transform.right.y) * speed;

                    if (prefab == rocket)
                    {
                        bulletInstance.GetComponent<BulletLeach>().myOwner = myLifeSpan;
                        bulletInstance.GetComponent<Rocket>().myOwner = myLifeSpan;

                    }
                    if (myTimeController.isStopped == true)
                    {
                        bulletInstance.GetComponent<Rocket>().enabled = false;
                        bulletInstance.GetComponent<BoxCollider2D>().enabled = false;
                        //bulletInstance.isKinematic = true;
                        bullets.Add(new bulletInfo(bulletInstance, dir));
                        print(bullets.Count);

                        
                    
                    }

                    else
                    {
                        bulletInstance.velocity = dir;//new Vector2(speed, 0);
                        
                        Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), this.GetComponentInParent<Collider2D>());
                    }

                

               
                    
                    /*
                    //instantiate muzzle flash
                    Transform clone = Instantiate(MuzzleFlashPrefab, gunPivot.position, gunPivot.rotation) as Transform;
                    clone.parent = gunPivot;
                    float size = Random.Range(0.6f, 0.9f);
                    clone.localScale = new Vector3(size, size, 0);
                    //Destroy(clone, 0.02f);
                    Destroy(clone);
                    */
                }
                else
                {
                    // Otherwise instantiate the rocket facing left and set it's velocity to the left.

                    Rigidbody2D bulletInstance = Instantiate(prefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
                    dir = new Vector2(transform.right.x, transform.right.y) * -speed;
                    if (prefab == rocket)
                    {
                            bulletInstance.GetComponent<BulletLeach>().myOwner = myLifeSpan;
                            bulletInstance.GetComponent<Rocket>().myOwner = myLifeSpan;


                    }
                    if (myTimeController.isStopped == true)
                    {

                        bulletInstance.GetComponent<Rocket>().enabled = false;
                        bulletInstance.GetComponent<BoxCollider2D>().enabled = false;
                        bullets.Add(new bulletInfo(bulletInstance, dir));
                    }

                    else
                    {
                        bulletInstance.velocity = dir;// new Vector2(-speed, 0);
                    
                        Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), this.GetComponentInParent<Collider2D>());
                    }
                 
                }

                myTimeController.AddForce(-dir * Settings.s.gunKnockBack);
               // Debug.Log(-dir * Settings.s.gunKnockBack);
            }
          
        }
	}



    public void StopRocket()
    {
        
        
        
    }

    public void ReseumeRocket()
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
