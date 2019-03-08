using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEffectScript : MonoBehaviour
{
    public float duration = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy( gameObject, duration );
    }

    // Update is called once per frame
    void Update()
    {
    }
}
