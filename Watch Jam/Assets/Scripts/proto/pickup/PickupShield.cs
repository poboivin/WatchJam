using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShield : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = 100.0f;
    [SerializeField]
    private float radiusScale = 1.0f;

    private const int maxShieldCount = 3;
    private int shieldCount = 3;

    List<ShieldPiece> shieldPieces;
    List<int> hitIds;

	// Use this for initialization
	void Start () {
        hitIds = new List<int>();
        shieldPieces = new List<ShieldPiece>();

        for( int i = 0; i < gameObject.transform.childCount; i++ )
        { 
            var child = gameObject.transform.GetChild( i );
            var curScale = child.gameObject.transform.localScale;
            child.gameObject.transform.localScale = new Vector3( curScale.x * radiusScale, curScale.y * radiusScale, 1.0f );
            var shieldPiece = child.GetComponentInChildren<ShieldPiece>();
            if( radiusScale != 0.0f )
            {
                // not to scale an owl sprite no matter what radius scale is.
                for( int j = 0; j < shieldPiece.gameObject.transform.childCount; j++ )
                {
                    var owl = shieldPiece.gameObject.transform.GetChild( j );
                    var owlScale = owl.gameObject.transform.localScale;
                    owl.gameObject.transform.localScale = new Vector3( owlScale.x / radiusScale, owlScale.y / radiusScale, 1.0f );
                }
            }
            shieldPiece.gameObject.SetActive( false );
            shieldPieces.Add( shieldPiece );
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
