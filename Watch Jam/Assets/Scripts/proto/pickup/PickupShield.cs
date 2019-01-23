using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShield : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = 100.0f;
    private const int maxShieldCount = 3;
    private int shieldCount = 3;

    ShieldPiece[] shieldPieces;
    List<int> hitIds;

	// Use this for initialization
	void Start () {
        hitIds = new List<int>();
        shieldPieces = GetComponentsInChildren<ShieldPiece>();
        foreach( var piece in shieldPieces )
        {
            piece.gameObject.SetActive( false );
        }
    }
	
	// Update is called once per frame
	void Update () {
        if( shieldCount > 0 )
        {
            foreach( var piece in shieldPieces )
            {
                if( piece.isHit == true && piece.gameObject.activeSelf == true
                    && hitIds.Contains( piece.hitId ) == false )
                {
                    shieldCount -= 1;
                    hitIds.Add( piece.hitId );
                    piece.gameObject.SetActive( false );
                }
            }
        }
	}

    private void FixedUpdate()
    {
        transform.Rotate( Vector3.forward, Time.fixedDeltaTime * rotationSpeed );
    }

    public void RefreshShield()
    {
        hitIds.Clear();
        shieldCount = maxShieldCount;
        foreach( var piece in shieldPieces )
        {
            piece.RefreshShield();
        }
    }

}
