using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour {

    public enum WeaponType
    {
        TestWeapon,
        SampleWeapon
    }

    WeaponType type;

    void Awake()
    {
        if (type == WeaponType.TestWeapon)
        {
            gameObject.AddComponent<TestWeapon>();
        }
        else if (type == WeaponType.SampleWeapon)
        {
            gameObject.AddComponent<SampleWeapon>();
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
