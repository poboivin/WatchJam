﻿using System.Collections;
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
        public GameObject Hurtzone;
        
       

        [HideInInspector]
        public Vector3 StartLeftPosition;
        [HideInInspector]
        public Vector3 EndLeftPosition;
        [HideInInspector]
        public Vector3 StartRightPosition;
        [HideInInspector]
        public Vector3 EndRightPosition;

        

    }
    //AudioStuff
    public AudioSource DoorOpenSource;
    public AudioSource DoorCloseSource;
    public AudioSource AlarmSource;
    public AudioSource LaserSource;

    public float doorAudio = 0.3f;
    //index 0 = Right Door set
    //index 1 = Middle Door Set
    //index 2 = Left Door set
    public Doors[] DoorSets;
    public Sprite[] LightSprites;


    //Lerp Values
    float CurrentLerpTime;

    [SerializeField]
    Color StartColor;
[SerializeField]
    Color EndColor;
    
     //Delay Variables

    [SerializeField]
    float[] NextDoorOpenRange;
    [SerializeField]
    float TimeHurtZoneLasts;
    [SerializeField]
    float DelayBeforeDoorOpens;
    [SerializeField]
    float LerpTime = 3f;
    [SerializeField]
    float LerpTimeChargeup = 3f;
    [SerializeField]
    float PressureDoorStart=80f;
    int PreviousDoor;

    //TIMER VARIABLES
    float WaitStart;
    float BeamStartTime=.15f;


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
       // AS.PlayOneShot(DoorOpenClip, doorAudio);
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
        //AS.PlayOneShot(DoorCloseClip, doorAudio);
        float perc = CurrentLerpTime / LerpTime;
        DoorSets[doors].DoorLeft.transform.position = Vector3.Lerp(DoorSets[doors].EndLeftPosition, DoorSets[doors].StartLeftPosition, perc);
        DoorSets[doors].DoorRight.transform.position = Vector3.Lerp(DoorSets[doors].EndRightPosition, DoorSets[doors].StartRightPosition, perc);
    }



    //public void ChargeHurtZone(int Zone)
    //{
    //   // Debug.Log("Hurt Zone ACtive");
    //    CurrentLerpTime += Time.deltaTime;

    //    if (CurrentLerpTime > LerpTimeChargeup)
    //    {
    //        CurrentLerpTime = LerpTimeChargeup;
    //    }
    //    float ChargePerc = CurrentLerpTime / LerpTimeChargeup;

    //    SpriteRenderer ZoneSprite = DoorSets[Zone].Hurtzone.GetComponentInChildren<SpriteRenderer>();
    //    //SpriteRenderer ZoneSprite02 = DoorSets[Zone].Hurtzone.GetComponent<HurtZone>().CompanionVisual;
    //    ZoneSprite.color = Color.Lerp(StartColor, EndColor, ChargePerc);
    //    //ZoneSprite02.color = Color.Lerp(StartColor, EndColor, ChargePerc);



    //}


    //HandlesWhen doors close and open

       IEnumerator TriggerDoorOpening()
       {
        //Wait a random amount of time to trigger next doors
        yield return new WaitForSeconds(Random.Range(NextDoorOpenRange[0], NextDoorOpenRange[1]));

        if (Time.time < PressureDoorStart)
        {
            int DoorToOpen = Random.Range(0, 3);
            while (DoorToOpen == PreviousDoor)
            {
                //Ensures last door open isn't the same
                DoorToOpen = Random.Range(0, 3);
            }

            PreviousDoor = DoorToOpen;


            StartCoroutine(OpenCloseRandomDoor(DoorToOpen));
        }

        else
        {
            int Door1 = Random.Range(0, 3);
            int Door2 = Random.Range(0, 3);

            while(Door1 == Door2)
            {
                 Door1 = Random.Range(0, 3);
                 Door2 = Random.Range(0, 3);
            }

            StartCoroutine(OpenCloseRandomMultipleDoors(Door1, Door2));


        }

       

     

       }


  //Handles opening and closing doors





    IEnumerator OpenCloseRandomDoor(int RD)
    {
        //Turn Door Lights To Red and activate caution strip
        //ALARMBEGIN
        AlarmSource.Play();
        DoorSets[RD].Light.sprite = LightSprites[1];

        yield return new WaitForSeconds(DelayBeforeDoorOpens);

        //Begin opening doors
        //DOOROPEN SOUND
        DoorOpenSource.Play();
        while (DoorSets[RD].DoorLeft.transform.position.x > DoorSets[RD].EndLeftPosition.x)
        {
            LerpDoorsOpen(RD);
            yield return new WaitForFixedUpdate();
        }

        //Begin activating hurt zone
        CurrentLerpTime = 0;
        
        
        DoorSets[RD].Hurtzone.SetActive(true);

        WaitStart = Time.time;

        LaserSource.Play();
        while (Time.time-WaitStart<=BeamStartTime)
        {
            
            yield return new WaitForFixedUpdate();
        }

        CurrentLerpTime = 0;
       
        //active hurtzone for x amount of time
        DoorSets[RD].Hurtzone.GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitForSeconds(TimeHurtZoneLasts);

        DoorSets[RD].Hurtzone.GetComponent<BoxCollider2D>().enabled = false;
        DoorSets[RD].Hurtzone.GetComponentInChildren <SpriteRenderer>().color = StartColor;
        DoorSets[RD].Hurtzone.SetActive(false);
        DoorSets[RD].Light.sprite = LightSprites[0];
        //CLOSEDOORSOUND
        DoorCloseSource.Play();
        while (DoorSets[RD].DoorLeft.transform.position.x < DoorSets[RD].StartLeftPosition.x)
        {
            LerpDoorsClosed(RD);
            yield return new WaitForFixedUpdate();
        }
        CurrentLerpTime = 0;
        StartCoroutine(TriggerDoorOpening());
        yield break;

    }

    IEnumerator OpenCloseRandomMultipleDoors(int RD1,int RD2)
    {
        DoorSets[RD1].Light.sprite = LightSprites[1];
        DoorSets[RD2].Light.sprite = LightSprites[1];
        //ALARMSOUND
        AlarmSource.Play();
        yield return new WaitForSeconds(DelayBeforeDoorOpens);
        //DOOR OPEN SOUND
        DoorOpenSource.Play();
        //Switch Light Color Red
        while (DoorSets[RD1].DoorLeft.transform.position.x > DoorSets[RD1].EndLeftPosition.x)
        {
            LerpDoorsOpen(RD1);
            LerpDoorsOpen(RD2);
            yield return new WaitForFixedUpdate();
        }
        CurrentLerpTime = 0;
        //LASER SOUND
        LaserSource.Play();
        DoorSets[RD1].Hurtzone.SetActive(true);
        DoorSets[RD2].Hurtzone.SetActive(true);

        WaitStart = Time.time;

        while (Time.time-WaitStart<=BeamStartTime)
        {
           
            yield return new WaitForFixedUpdate();
        }

        DoorSets[RD1].Hurtzone.GetComponent<BoxCollider2D>().enabled = true;
        DoorSets[RD2].Hurtzone.GetComponent<BoxCollider2D>().enabled = true;
        CurrentLerpTime = 0;

        //Turn on electricity
        yield return new WaitForSeconds(TimeHurtZoneLasts);

       
        DoorSets[RD1].Light.sprite = LightSprites[0];
        DoorSets[RD2].Light.sprite = LightSprites[0];

        DoorSets[RD1].Hurtzone.GetComponent<BoxCollider2D>().enabled = false;
        DoorSets[RD2].Hurtzone.GetComponent<BoxCollider2D>().enabled = false;
        DoorSets[RD1].Hurtzone.GetComponentInChildren<SpriteRenderer>().color = StartColor;
        DoorSets[RD2].Hurtzone.GetComponentInChildren<SpriteRenderer>().color = StartColor;
        DoorSets[RD1].Hurtzone.SetActive(false);
        DoorSets[RD2].Hurtzone.SetActive(false);


        //DOORCLOSE SOUNDS
        DoorCloseSource.Play();
        while (DoorSets[RD1].DoorLeft.transform.position.x < DoorSets[RD1].StartLeftPosition.x)
        {
            LerpDoorsClosed(RD1);
            LerpDoorsClosed(RD2);
            yield return new WaitForFixedUpdate();
        }
        CurrentLerpTime = 0;
        StartCoroutine(TriggerDoorOpening());
        yield break;



    }
}









