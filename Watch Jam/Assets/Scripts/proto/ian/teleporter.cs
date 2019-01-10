using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public class teleporter : MonoBehaviour {

    public teleporter Destination;
    public Transform Exit;
    public float TeleportOffset = 1;
    public AudioClip clip;
    public AudioSource source;
    public bool Veritcal = false;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Veritcal == true)
        {
            if (collision.tag == "Player" && collision.GetComponent<PlayerControl>().PortalEntry == 0)
            {
                collision.GetComponent<PlayerControl>().PortalEntry++;

                Rigidbody2D other = collision.GetComponent<Rigidbody2D>();

                if (other != null && other.bodyType != RigidbodyType2D.Static)
                {
                    if (clip != null && source != null)
                    {
                        source.clip = clip;
                        source.Play();
                    }

                    if (collision.GetComponent<TimeZone>())
                    {
                        Destroy(other.gameObject);
                    }

                    var relPoint = transform.InverseTransformPoint(other.transform.position);
                    var relVelocity = -transform.InverseTransformDirection(other.velocity);
                    other.velocity = Destination.transform.TransformDirection(relVelocity);
                    other.MovePosition(Destination.transform.TransformPoint(relPoint) + (convert2to3(other.velocity.normalized) * TeleportOffset));
                }
            }


            else if (collision.tag == "Player" && collision.GetComponent<PlayerControl>().PortalEntry > 0)
            {
                print("adding to the player");
                collision.GetComponent<PlayerControl>().PortalEntry++;
            }


            else if (collision.tag != "Player")
            {

                Rigidbody2D other = collision.GetComponent<Rigidbody2D>();

                if (other != null && other.bodyType != RigidbodyType2D.Static)
                {
                    if (clip != null && source != null)
                    {
                        source.clip = clip;
                        source.Play();
                    }

                    if (collision.GetComponent<TimeZone>())
                    {
                        Destroy(other.gameObject);
                    }

                    var relPoint = transform.InverseTransformPoint(other.transform.position);
                    var relVelocity = -transform.InverseTransformDirection(other.velocity);
                    other.velocity = Destination.transform.TransformDirection(relVelocity);
                    other.MovePosition(Destination.transform.TransformPoint(relPoint) + (convert2to3(other.velocity.normalized) * TeleportOffset));
                }
            }

        }

        else
        {
           

            Rigidbody2D other = collision.GetComponent<Rigidbody2D>();

            if (other != null && other.bodyType != RigidbodyType2D.Static)
            {
                if (clip != null && source != null)
                {
                    source.clip = clip;
                    source.Play();
                }

                if (collision.GetComponent<TimeZone>())
                {
                    Destroy(other.gameObject);
                }

                var relPoint = transform.InverseTransformPoint(other.transform.position);
                var relVelocity = -transform.InverseTransformDirection(other.velocity);
                other.velocity = Destination.transform.TransformDirection(relVelocity);
                other.MovePosition(Destination.transform.TransformPoint(relPoint) + (convert2to3(other.velocity.normalized) * TeleportOffset));
            }
        }

        return;


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PlayerControl>().PortalEntry>=2)
        {
            collision.GetComponent<PlayerControl>().PortalEntry = 0;
        }
    }


    public static Vector3 convert2to3(Vector2 vector)
    {

        return new Vector3(vector.x, vector.y, 0);
    }
}
