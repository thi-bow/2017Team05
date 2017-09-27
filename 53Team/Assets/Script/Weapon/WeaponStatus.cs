using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour {
    private int atk;
    [SerializeField]
    private GameObject weapon;
    private WeaponChange _WeaponChange;

    // Use this for initialization
    void Start () {
        _WeaponChange = GetComponent<WeaponChange>();
        weapon = _WeaponChange.GetWeapon();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetWeapon(GameObject obj)
    {
        weapon = obj;
    }

    public GameObject GetEquip()
    {
        return weapon;
    }
}
