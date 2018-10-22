using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {
    public Rigidbody2D body;
    public TimeBody myTimeBody;
    public Vector2 oldVelocity;
    bool first = true;

    [SerializeField]
    private float _timeScale = 1;
    private float newTimeScale = 1;
    private float lastTimeScale = 1;
    public Vector2 vel;
    public float factor = 0.25f;
    public PierInputManager inputManager;
    public PierInputManager.ButtonName button;
    public float timeScale
    {
        get { return _timeScale; }
        set
        {
            lastTimeScale = _timeScale;

            _timeScale = Mathf.Abs(value);
            newTimeScale = _timeScale;
        }
    }

    void Awake()
    {
        timeScale = _timeScale;
   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //timeFactor = 0;
            body.isKinematic = true;
            oldVelocity = body.velocity;
            body.velocity = Vector2.zero;
            body.angularVelocity = 0;
            myTimeBody.isRecording = false;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            //timeFactor = 1;
            body.isKinematic = false;
            body.velocity = oldVelocity;
            myTimeBody.isRecording = true;

        }
        if (Input.GetButtonDown("Fire2"))
        {
            timeScale = factor;
            //body.massh /= factor;
           // body.gravityScale *= factor;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            timeScale = 1f;


        }
    }
    public void AddForce(Vector2 force)
    {
    //    force = (force / timeScale);
        // force /= newTimeScale / lastTimeScale; f = m * a
        body.AddForce(force, ForceMode2D.Force);
    }
    public void AddImpulse(Vector2 force)
    {
        // if(timeScale!= 1)
        //force = (force /timeScale) /2; 
        force /= timeScale;
       //force /= newTimeScale / lastTimeScale; //f = m * a
        body.AddForce(force, ForceMode2D.Impulse);

       // body.velocity += force;
        
    }
    void FixedUpdate()
    {
       body.gravityScale *= newTimeScale / lastTimeScale;
      //  body.velocity *= newTimeScale / lastTimeScale;
       body.mass /= newTimeScale  / lastTimeScale;
       // body.drag /= newTimeScale / lastTimeScale;
     
        // body.angularVelocity *= newTimeScale / lastTimeScale;
        //vel = body.velocity;
        if(vel.magnitude> 30)
        {
            Debug.Log(newTimeScale + " " +lastTimeScale);
        }
        lastTimeScale = newTimeScale;
    }
}
