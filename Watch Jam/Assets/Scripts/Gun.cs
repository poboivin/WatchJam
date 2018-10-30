using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
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

    public float angle;
    public float x;
    public float y;
	void Awake()
	{
        myInputManager = GetComponentInParent<PierInputManager>();
        myLifeSpan = GetComponentInParent<LifeSpan>();
        myTimeController = GetComponentInParent<TimeController>();
        myAmmo  = GetComponentInParent<Ammo>();
        // Setting up the references.
        //anim = transform.root.gameObject.GetComponent<Animator>();
        myPlayerControl = transform.root.GetComponent<PlayerControl>();
	}
	void Update ()
	{
        x = Mathf.Abs( myInputManager.GetAxis("Horizontal"));
        y = myInputManager.GetAxis("Vertical");
        float theta_rad  = Mathf.Atan2(y, x);
        float theta_deg = (theta_rad / Mathf.PI * 180) + (theta_rad > 0 ? 0 : 360);
        angle = theta_deg;//(angle + 360) % 360;



        //Debug.Log(angle + "");

        
        gunPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, angle)); //Rotating!

        
		// If the fire button is pressed...
		if(myTimeController.isRewinding == false && myInputManager.GetButtonDown(ShootButton) && Time.time > nextFire)
		{
            nextFire = Time.time + fireRate;
            Rigidbody2D prefab = null ;
            if (myAmmo.Grenade != null)
            {
               
                prefab = myAmmo.Grenade;
                myAmmo.Grenade = null;

                // Debug.Log("GRENADE");
            }
            else if (myAmmo.CurrentAmmo > 0)
            {
                myAmmo.CurrentAmmo--;
                prefab = rocket;
            }
               

            if(prefab != null)
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
                    bulletInstance.velocity = dir;//new Vector2(speed, 0);
                    if(prefab == rocket)
                    {
                        bulletInstance.GetComponent<BulletLeach>().Owner = myLifeSpan;

                    }
                    Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), this.GetComponentInParent<Collider2D>());

                }
                else
                {
                    // Otherwise instantiate the rocket facing left and set it's velocity to the left.
                    Rigidbody2D bulletInstance = Instantiate(prefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
                    dir = new Vector2(transform.right.x, transform.right.y) * -speed;

                    bulletInstance.velocity = dir;// new Vector2(-speed, 0);
                    if (prefab == rocket)
                    {
                        bulletInstance.GetComponent<BulletLeach>().Owner = myLifeSpan;

                    }
                    Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), this.GetComponentInParent<Collider2D>());
                }

                myTimeController.AddForce(-dir * Settings.s.gunKnockBack);
               // Debug.Log(-dir * Settings.s.gunKnockBack);
            }
          
        }
	}
}
