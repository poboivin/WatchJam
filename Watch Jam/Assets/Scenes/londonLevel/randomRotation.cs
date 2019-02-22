using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomRotation : MonoBehaviour
{
    public float speed;
    public int rotateDir;

    // Start is called before the first frame update
    void Start()
    {
        rotateDir = Random.Range(0, 2);
        speed = Random.Range(20f, 41f);

    }

    // Update is called once per frame
    void Update()
    {
        if (rotateDir == 0)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        }
        else
        {
            transform.Rotate(Vector3.back * Time.deltaTime * speed);
        }
    }
}
