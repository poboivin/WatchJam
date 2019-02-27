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



    public void ChargeHurtZone(int Zone)
    {
        CurrentLerpTime += Time.deltaTime;

        if (CurrentLerpTime > LerpTimeChargeup)
        {
            CurrentLerpTime = LerpTimeChargeup;
        }
        float ChargePerc = CurrentLerpTime / LerpTimeChargeup;

        SpriteRenderer ZoneSprite = DoorSets[Zone].Hurtzone.GetComponent<SpriteRenderer>();
        ZoneSprite.color = Color.Lerp(StartColor, EndColor, ChargePerc);
        
        
    }


    //HandlesWhen doors close and open

       IEnumerator TriggerDoorOpening()
       {
        yield return new WaitForSeconds(Random.Range(NextDoorOpenRange[0], NextDoorOpenRange[1]));

        int DoorToOpen = Random.Range(1, 3);
        Debug.Log(DoorToOpen);
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
        DoorSets[1].Light.sprite = LightSprites[1];

        yield return new WaitForSeconds(DelayBeforeDoorOpens);


        while (DoorSets[1].DoorLeft.transform.position.x > DoorSets[1].EndLeftPosition.x)
        {
            LerpDoorsOpen(1);
            yield return new WaitForFixedUpdate();
        }

        CurrentLerpTime = 0;
        DoorSets[1].Hurtzone.SetActive(true);
        while (DoorSets[1].Hurtzone.GetComponent<SpriteRenderer>().color.a< EndColor.a)
        {
            ChargeHurtZone(1);
            yield return new WaitForFixedUpdate();
        }

        CurrentLerpTime = 0;
        DoorSets[1].Hurtzone.GetComponent<BoxCollider2D>().enabled = true; 

        yield return new WaitForSeconds(TimeHurtZoneLasts);

        DoorSets[1].Hurtzone.GetComponent<BoxCollider2D>().enabled = false;
        DoorSets[1].Hurtzone.GetComponent<SpriteRenderer>().color = StartColor;
        DoorSets[1].Hurtzone.SetActive(false);
        DoorSets[1].Light.sprite = LightSprites[0];

        while (DoorSets[1].DoorLeft.transform.position.x< DoorSets[1].StartLeftPosition.x)
        {
            LerpDoorsClosed(1);
            yield return new WaitForFixedUpdate();
        }
        CurrentLerpTime = 0;
        StartCoroutine(TriggerDoorOpening());
        yield break;



    }

    IEnumerator OpenCloseLeftRightDoors()
    {
        DoorSets[0].Light.sprite = LightSprites[1];
        DoorSets[2].Light.sprite = LightSprites[1];

        yield return new WaitForSeconds(DelayBeforeDoorOpens);
        //Switch Light Color Red
        while (DoorSets[0].DoorLeft.transform.position.x > DoorSets[0].EndLeftPosition.x)
        {
            LerpDoorsOpen(0);
            LerpDoorsOpen(2);
            yield return new WaitForFixedUpdate();
        }
        CurrentLerpTime = 0;

        DoorSets[0].Hurtzone.SetActive(true);
        DoorSets[2].Hurtzone.SetActive(true);
        while (DoorSets[0].Hurtzone.GetComponent<SpriteRenderer>().color.a < EndColor.a)
        {
            ChargeHurtZone(0);
            ChargeHurtZone(2);
            yield return new WaitForFixedUpdate();
        }

        DoorSets[0].Hurtzone.GetComponent<BoxCollider2D>().enabled = true;
        DoorSets[2].Hurtzone.GetComponent<BoxCollider2D>().enabled = true;
        CurrentLerpTime = 0;

        //Turn on electricity
        yield return new WaitForSeconds(TimeHurtZoneLasts);
      

        DoorSets[0].Light.sprite = LightSprites[0];
        DoorSets[2].Light.sprite = LightSprites[0];

        DoorSets[0].Hurtzone.GetComponent<BoxCollider2D>().enabled = false;
        DoorSets[2].Hurtzone.GetComponent<BoxCollider2D>().enabled = false;
        DoorSets[0].Hurtzone.GetComponent<SpriteRenderer>().color = StartColor;
        DoorSets[2].Hurtzone.GetComponent<SpriteRenderer>().color = StartColor;
        DoorSets[0].Hurtzone.SetActive(false);
        DoorSets[2].Hurtzone.SetActive(false);



        while (DoorSets[0].DoorLeft.transform.position.x < DoorSets[0].StartLeftPosition.x)
        {
            LerpDoorsClosed(0);
            LerpDoorsClosed(2);
            yield return new WaitForFixedUpdate();
        }
        CurrentLerpTime = 0;
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
