using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    [SerializeField]
    Animator ArarataAnimator;
    PlayerControl PC;
    

    // Start is called before the first frame update
    void Start()
    {
        PC = GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
