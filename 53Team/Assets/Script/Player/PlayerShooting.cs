using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    private Player _player = null;

    // Use this for initialization
    void Start () {
        _player = this.gameObject.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
