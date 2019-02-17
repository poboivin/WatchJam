using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap2 : MonoBehaviour
{

    public float positiveX;
    public float negativeX;
    public float positiveY;
    public float negativeY;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(positiveX, 50, 0), new Vector3(positiveX, -50, 0));

        Gizmos.DrawLine(new Vector3(negativeX, 50, 0), new Vector3(negativeX, -50, 0));

        Gizmos.DrawLine(new Vector3(-50, positiveY, 00), new Vector3(50, positiveY, 0));


        Gizmos.DrawLine(new Vector3(-50, negativeY, 00), new Vector3(50, negativeY, 0));
    }
    // Update is called once per frame
    void Update()
    {
        Rigidbody2D[] allBodys = GameObject.FindObjectsOfType<Rigidbody2D>();

        foreach (Rigidbody2D rb in allBodys)
        {

            if( (rb.position + rb.velocity *Time.deltaTime).x > positiveX)
            {
                rb.isKinematic = true;
                rb.transform.position = (new Vector2(negativeX, rb.position.y));
                rb.isKinematic =false;

            }
            if ((rb.position + rb.velocity * Time.deltaTime).x < negativeX)
            {
                rb.isKinematic = true;

                rb.transform.position = (new Vector2(positiveX, rb.position.y));
                rb.isKinematic = false;
            }

            if ((rb.position + rb.velocity * Time.deltaTime).y > positiveY)
            {
                rb.isKinematic = true;
                rb.transform.position = (new Vector2(rb.position.x, negativeY));
                rb.isKinematic = false;

            }
            if ((rb.position + rb.velocity * Time.deltaTime).y < negativeY)
            {
                rb.isKinematic = true;
                rb.transform.position = (new Vector2(rb.position.x, positiveY));
                rb.isKinematic = false;

            }
        }

    }
}
