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
        public Sprite[] LightSprites;

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

    //Lerp Values
    float CurrentLerpTime;
    float LerpTime = 3f;
    //Times For doors to trigger
    float MidOpenTime=15;
    bool MidOpen;
    float LeftRightOpenTime=60;

    




    //-4.1

    

    // Start is called before the first frame update
    void Start()
    {
        //set up of all doors to be lerped
        for (int i = 0; i < DoorSets.Length; i++)
        {
            Vector3 DoorPositionLeft = DoorSets[i].DoorLeft.transform.position;
            Vector3 DoorPositionRight = DoorSets[i].DoorRight.transform.position;

            //Settingup left doors
            DoorSets[i].StartLeftPosition = DoorPositionLeft;
            DoorSets[i].EndLeftPosition = new Vector3(DoorPositionLeft.x-4.07f, DoorPositionLeft.y, DoorPositionLeft.z);

            //setting up right doors
            DoorSets[i].StartRightPosition = DoorPositionRight;
            DoorSets[i].EndRightPosition = new Vector3(DoorPositionRight.x + 4.07f, DoorPositionRight.y, DoorPositionRight.z);


        }

        StartCoroutine(TriggerDoorOpening());

       


    }

    // Update is called once per frame
    void Update()
    {
        




       

      
    }




    public void LerpDoorsOpen(int doors)
    {
        CurrentLerpTime += Time.deltaTime;
        if (CurrentLerpTime > LerpTime)
        {
            CurrentLerpTime = LerpTime;
        }

        float perc = CurrentLerpTime / LerpTime;
        DoorSets[doors].DoorLeft.transform.position = Vector3.Lerp(DoorSets[doors].StartLeftPosition, DoorSets[doors].EndLeftPosition, perc);
        DoorSets[doors].DoorRight.transform.position = Vector3.Lerp(DoorSets[doors].StartRightPosition, DoorSets[doors].EndRightPosition, perc);
    }

    public void LerpDoorsClosed(int doors)
    {
      
        CurrentLerpTime += Time.deltaTime;
        if (CurrentLerpTime > LerpTime)
        {
            CurrentLerpTime = LerpTime;
        }

        float perc = CurrentLerpTime / LerpTime;
        DoorSets[doors].DoorLeft.transform.position = Vector3.Lerp(DoorSets[doors].EndLeftPosition, DoorSets[doors].StartLeftPosition, perc);
        DoorSets[doors].DoorRight.transform.position = Vector3.Lerp(DoorSets[doors].EndRightPosition, DoorSets[doors].StartRightPosition, perc);
    }


    //HandlesWhen doors close and open

       IEnumerator TriggerDoorOpening()
       {
        yield return new WaitForSeconds(Random.Range(5,10));

        int DoorToOpen = Random.Range(1, 2);

        switch (DoorToOpen)
        {
            case 1:
                StartCoroutine(OpenCloseMiddleDoors());
                break;

            case 2:
                StartCoroutine(OpenCloseLeftRightDoors());
                break;
        }

       }















    //Handles opening and closing doors



    IEnumerator OpenCloseMiddleDoors()
    {
        Debug.Log("middleDoors");
        while (DoorSets[1].DoorLeft.transform.position.x > DoorSets[1].EndLeftPosition.x)
        {
            LerpDoorsOpen(1);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(5);

        while(DoorSets[1].DoorLeft.transform.position.x< DoorSets[1].StartLeftPosition.x)
        {
            LerpDoorsClosed(1);
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(TriggerDoorOpening());
        yield break;



    }

    IEnumerator OpenCloseLeftRightDoors()
    {
        Debug.Log("LeftRight Doors");
        //Switch Light Color Red
        while (DoorSets[0].DoorLeft.transform.position.x > DoorSets[0].EndLeftPosition.x)
        {
            LerpDoorsOpen(0);
            LerpDoorsOpen(2);
            yield return new WaitForFixedUpdate();
        }

        //Turn on electricity
        yield return new WaitForSeconds(5);
        //Turn Off Electricity
        //Swithc Light Color Green


        while (DoorSets[0].DoorLeft.transform.position.x < DoorSets[0].StartLeftPosition.x)
        {
            LerpDoorsClosed(0);
            LerpDoorsClosed(2);
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(TriggerDoorOpening());
        yield break;



    }






    IEnumerator CloseDoors(int doors)
    {
        
        while(DoorSets[doors].DoorLeft.transform.position.x != DoorSets[doors].StartLeftPosition.x)
        {
           
            LerpDoorsClosed(doors);
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }








}
