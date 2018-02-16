using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Beam_bullet : Enemy_bullet {

    public override void OnRent()
    {
        base.OnRent();
        GetComponent<TrailRenderer>().enabled = true;
    }
}
