using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = 100.0f;
    private const int maxShieldCount = 3;
    private int shieldCount = 3;

    ShieldPiece[] shieldPieces;

	// Use this for initialization
	void Start () {
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
                if( piece.isHit == true && piece.gameObject.activeSelf == true )
                {
                    shieldCount -= 1;
                    piece.gameObject.SetActive( false );
                }
            }
        }
	}

    private void FixedUpdate()
    {
        transform.Rotate( -Vector3.forward, Time.fixedDeltaTime * rotationSpeed );
    }

    public void RefreshShield()
    {
        shieldCount = maxShieldCount;
        foreach( var piece in shieldPieces )
        {
            piece.RefreshShield();
        }
    }

}
