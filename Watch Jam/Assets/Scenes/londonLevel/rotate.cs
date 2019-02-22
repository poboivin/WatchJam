using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    private float speed = 0f;
    public float timeToCompleteCircle = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        speed = (Mathf.PI * 2) / timeToCompleteCircle;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);

        //transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
        
    }
}
