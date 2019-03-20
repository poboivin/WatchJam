using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTimedBomb : MonoBehaviour
{
    [SerializeField]
    public int timedBombCount = 5;

    [SerializeField]
    public float timeOutDuration = 3.0f;

    [SerializeField]
    public float explosionRangeScale = 10.0f;
}
