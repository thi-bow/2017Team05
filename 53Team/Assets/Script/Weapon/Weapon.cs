using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private WeaponList _WeaponList;
    private WeaponStatus _WeaponStatus;
    private WeaponChange _WeaponChange = null;

	// Use this for initialization
	void Start () {
        _WeaponList = this.gameObject.GetComponent<WeaponList>();
        _WeaponChange = this.gameObject.GetComponent<WeaponChange>();
	}
	
	// Update is called once per frame
	void Update () {
        _WeaponChange.Change();
	}
}
