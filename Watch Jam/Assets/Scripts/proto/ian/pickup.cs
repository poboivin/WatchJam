using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public enum pickUpType {
        health,
        grenade,
        shield,         // Owl
        rapidfire,      // Cat
        shotgun,        // Multiple bullets spread out 
        bounce,         // Bullet can bounce the wall
        pierce,
        thicc,
        warp,
        timedBomb,
        warpAndPierce
    }
    public pickUpType type;
    public Rigidbody2D Grenade;     // Prefab of the rocket.
    private TextMesh textMesh;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    // Use this for initialization
    Vector3 start;
    Vector3 end;
    void Start()
    {
        start = transform.position;
        textMesh = GetComponentInChildren<TextMesh>();
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = LerpUtility.SineWave(Time.time, start, Vector3.up, 0.2f);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            collision.GetComponent<PlayerControl>().PlayPickUp();

            if (type == pickUpType.grenade)
            {
                Ammo player = collision.GetComponent<Ammo>();
                if (player != null && player.enabled == true)
                {
                    player.Grenade = Grenade;
                    textMesh.text = "GRENADE!";
                }
            }
            else if (type == pickUpType.shield)
            {
                PickupShield player = collision.GetComponentInChildren<PickupShield>();
                if (player != null)
                {
                    player.RefreshShield();
                    textMesh.text = "SHIELD!";
                }
            }
            else if (type == pickUpType.rapidfire)
            {
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if (playerGun != null)
                {
                    PickupFireBoost boost = gameObject.GetComponent<PickupFireBoost>();
                    if (boost != null)
                    {
                        playerGun.SetGunPower(new GunPowerFiringBoost(playerGun, boost));
                        if (Grenade != null)
                            playerGun.rocket = Grenade;
                    }
                    textMesh.text = "BURST!";
                }
            }
            else if (type == pickUpType.bounce)
            {
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if (playerGun != null)
                {
                    PickupBounce bounceInfo = gameObject.GetComponent<PickupBounce>();
                    if (bounceInfo != null)
                    {
                        playerGun.SetGunPower(new GunPowerBulletBounce(playerGun, bounceInfo));
                        if (Grenade != null)
                            playerGun.rocket = Grenade;
                    }
                    textMesh.text = "BOUNCE!";
                }
            }
            else if (type == pickUpType.shotgun)
            {
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if (playerGun != null)
                {
                    PickupShotgun shotgunInfo = gameObject.GetComponent<PickupShotgun>();
                    if (shotgunInfo != null)
                    {
                        playerGun.SetGunPower(new GunPowerShotgun(playerGun, shotgunInfo));
                        if (Grenade != null)
                            playerGun.rocket = Grenade;
                    }
                    textMesh.text = "SHOTGUN!";
                }
            }
            else if (type == pickUpType.pierce)
            {
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if (playerGun != null)
                {
                    PickupPierce pierceShotInfo = gameObject.GetComponent<PickupPierce>();
                    if (pierceShotInfo != null)
                    {
                        playerGun.SetGunPower(new GunPowerPierce(playerGun, pierceShotInfo));
                        if (Grenade != null)
                            playerGun.rocket = Grenade;
                    }
                    textMesh.text = "PIERCE!";
                }
            }
            else if (type == pickUpType.thicc)
            {
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if (playerGun != null)
                {
                    PickupThicc thiccInfo = gameObject.GetComponent<PickupThicc>();
                    if (thiccInfo != null)
                    {
                        playerGun.SetGunPower(new GunPowerThicc(playerGun, thiccInfo));
                        if (Grenade != null)
                            playerGun.rocket = Grenade;
                    }
                    textMesh.text = "THICC!";
                }
            }
            else if (type == pickUpType.warp)
            {
                Debug.Log("hi.");
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if (playerGun != null)
                {
                    Debug.Log("hi..");
                    PickupWarp warpInfo = gameObject.GetComponent<PickupWarp>();
                    if (warpInfo != null)
                    {
                        Debug.Log("hi...");
                        playerGun.SetGunPower(new GunPowerWarp(playerGun, warpInfo));
                        if (Grenade != null)
                        {
                            Debug.Log("hi....");
                            playerGun.rocket = Grenade;
                        }
                    }
                    textMesh.text = "WARP!";
                }
            }
            else if (type == pickUpType.timedBomb)
            {
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if (playerGun != null)
                {
                    PickupTimedBomb bombInfo = gameObject.GetComponent<PickupTimedBomb>();
                    if (bombInfo != null)
                    {
                        playerGun.SetGunPower(new GunPowerTimedBomb(playerGun, bombInfo));
                        if (Grenade != null)
                        {
                            playerGun.rocket = Grenade;
                        }
                    }
                    textMesh.text = "TIMEBOMB!";
                }
            }
            else if (type == pickUpType.warpAndPierce)
            {
               
                Gun playerGun = collision.GetComponentInChildren<Gun>();
                if (playerGun != null)
                {
                    PickupWarpAndPierce warpAndPierceInfo = gameObject.GetComponent<PickupWarpAndPierce>();
                    if (warpAndPierceInfo != null)
                    {
                       
                        playerGun.SetGunPower(new GunPowerWarpAndPierce(playerGun, warpAndPierceInfo));
                        if (Grenade != null)
                        {
                            playerGun.rocket = Grenade;
                        }
                    }
                    textMesh.text = "PIERCING!";
                }
            }

            StartCoroutine(PickedUp());
           
           
        }

    }

    IEnumerator PickedUp()
    {
        bc.enabled = false;
        sr.enabled = false;
        //play pickup sound
        yield return new WaitForSeconds(0.5f);
        Destroy( gameObject);
    }
}
