using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public PierInputManager inputManager;
    public Ammo myAmmo;
    private LifeSpan myLifeSpan;
    public PierInputManager.ButtonName button;
    public Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.
    public Transform gunPivot;

	private PlayerControl playerCtrl;       // Reference to the PlayerControl script.
    private TimeController myTimeController;                            //private Animator anim;					// Reference to the Animator component.
    public float angle;
    public float x;
    public float y;
	void Awake()
	{
        myLifeSpan = GetComponentInParent<LifeSpan>();
        myTimeController = GetComponentInParent<TimeController>();
        // Setting up the references.
        //anim = transform.root.gameObject.GetComponent<Animator>();
        playerCtrl = transform.root.GetComponent<PlayerControl>();
	}
	void Update ()
	{
        x =Mathf.Abs( inputManager.GetAxis("Horizontal"));
        y = inputManager.GetAxis("Vertical");
        float theta_rad  = Mathf.Atan2(y, x);
        float theta_deg = (theta_rad / Mathf.PI * 180) + (theta_rad > 0 ? 0 : 360);
        angle = theta_deg;//(angle + 360) % 360;



        //Debug.Log(angle + "");

        
        gunPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, angle)); //Rotating!

        
		// If the fire button is pressed...
		if(inputManager.GetButtonDown(button) && myAmmo.CurrentAmmo >0)
		{
            myAmmo.CurrentAmmo--;
            // ... set the animator Shoot trigger parameter and play the audioclip.
            //	anim.SetTrigger("Shoot");
            GetComponent<AudioSource>().Play();
            Vector2 dir = Vector2.zero;
            // If the player is facing right...
            if (playerCtrl.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
                dir = new Vector2(transform.right.x, transform.right.y) * speed;
                bulletInstance.velocity = dir;//new Vector2(speed, 0);
                bulletInstance.GetComponent<BulletLeach>().Owner = myLifeSpan;

            }
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
                dir = new Vector2(transform.right.x, transform.right.y) * -speed;

                bulletInstance.velocity = dir;// new Vector2(-speed, 0);
                bulletInstance.GetComponent<BulletLeach>().Owner = myLifeSpan;

            }
            myTimeController.AddForce(-dir* Settings.s.gunKockBack);
        }
	}
}
