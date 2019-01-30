using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTelaport : MonoBehaviour {
    public GameObject[] TelporterSprites;
    private bool AlreadyUsed;

	// Use this for initialization
	void Start () {
        //Turn off all telaport sprites on play
        for (int i = 0; i < TelporterSprites.Length; i++)
        {
            
            TelporterSprites[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }
	

    //turns on the telaporter sprites and trigger box
    public void TurnOnTelporter()
    {
    
        for (int i =0; i<TelporterSprites.Length; i++)
        {
          
            TelporterSprites[i].GetComponent<SpriteRenderer>().enabled = true;
        }
        GetComponent<Collider2D>().enabled = true;

            
    }

    //if player overlaps the collider add to players readied , try to load the level and destroy the actor
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && AlreadyUsed==false)
        {
            AlreadyUsed = true;
            TutorialManager.PlayersReadied++;
            FindObjectOfType<TutorialManager>().LoadLevel();
            Destroy(collision.gameObject);
        }
  
    

    }
}
