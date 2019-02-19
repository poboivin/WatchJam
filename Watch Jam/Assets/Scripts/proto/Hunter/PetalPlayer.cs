using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalPlayer : MonoBehaviour
{
    Animator PetalAnimator;
    [SerializeField]
    AnimationClip  PetalAnimClip;
    bool stillPlaying=false;

    // Start is called before the first frame update
    void Start()
    {
        PetalAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("overlap");
        if (!stillPlaying)
        {
            StartCoroutine(WaitForEndOfAnimation());
        }
    }


    private void OnTriggeEnter2D(Collider other)
    {
        Debug.Log("overlap");
        if (!stillPlaying)
        {
            StartCoroutine(WaitForEndOfAnimation());
        }
    }



    

  






    IEnumerator WaitForEndOfAnimation()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        stillPlaying = true;
        PetalAnimator.Play("PetalAnim",0,0f);
        Debug.Log("play");
        yield return new WaitForSeconds(PetalAnimClip.length);
        GetComponent<SpriteRenderer>().enabled = false;
        
        stillPlaying = false;
    }

}

