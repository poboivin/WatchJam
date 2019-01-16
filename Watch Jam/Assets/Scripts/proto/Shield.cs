using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    [SerializeField]
    private GameObject OwlSprite = null;
    [SerializeField]
    private float shieldRadius = 50.0f;
    [SerializeField]
    private float rotationSpeed = 100.0f;
    private const int maxShieldCount = 3;
    private int shieldCount = 3;

    ShieldPiece[] shiledPieces;

	// Use this for initialization
	void Start () {
        shiledPieces = GetComponentsInChildren<ShieldPiece>();
    }
	
	// Update is called once per frame
	void Update () {
		foreach( var piece in shiledPieces )
        {
            if( piece.isHit == true && piece.gameObject.activeSelf == true )
            {
                shieldCount -= 1;
                piece.gameObject.SetActive( false );
            }
        }

        if( shieldCount == 0 )
        {
            Debug.Log( "Remove shiled" );
            gameObject.SetActive( false );
        }
	}

    private void FixedUpdate()
    {
        if( OwlSprite )
            transform.Rotate( -Vector3.forward, Time.fixedDeltaTime * rotationSpeed );

    }

}
