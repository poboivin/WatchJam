using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public class teleporter : MonoBehaviour {

    public teleporter Destination;
    public Transform Exit;
    public float TeleportOffset = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D other = collision.GetComponent<Rigidbody2D>();

        if(other != null && other.bodyType != RigidbodyType2D.Static)
        {
            if (collision.GetComponent<TimeZone>())
            {
                Destroy(other.gameObject);
            }
            // other.isKinematic = true;

         //   other.MovePosition(Destination.Exit.position);
            //  other.isKinematic = false ;
            var relPoint = transform.InverseTransformPoint(other.transform.position);
            var relVelocity = -transform.InverseTransformDirection(other.velocity);
            other.velocity = Destination.transform.TransformDirection(relVelocity);
            other.MovePosition ( Destination.transform.TransformPoint(relPoint) + (convert2to3(other.velocity. normalized) * TeleportOffset));

            // other.transform.rotation = Destination.rotation;
        }

    }
    public static Vector3 convert2to3(Vector2 vector)
    {

        return new Vector3(vector.x, vector.y, 0);
    }
}
