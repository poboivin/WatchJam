using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class FutureDoorManager : MonoBehaviour
{
    [System.Serializable]
    public struct Doors
    {
        public GameObject DoorLeft;
        public GameObject DoorRight;
        public SpriteRenderer Light;

        [HideInInspector]
        public Vector3 StartLeftPosition;
        [HideInInspector]
        public Vector3 EndLeftPosition;
        [HideInInspector]
        public Vector3 StartRightPosition;
        [HideInInspector]
        public Vector3 EndRightPosition;

    }
    
    //index 0 = Right Door set
    //index 1 = Middle Door Set
    //index 2 = Left Door set
    public Doors[] DoorSets;

    




    //-4.1

    

    // Start is called before the first frame update
    void Start()
    {
        //for(int i = 0; i<DoorSets.Length; i++)
        //{
        //    DoorSets[i].StartLeftPosition = DoorSets[i].DoorLeft.transform.position;
        //    DoorSets[i].EndLeftPosition = DoorSets[i].DoorLeft.transform.position-
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
