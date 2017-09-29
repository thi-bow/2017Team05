using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] GameObject player;

    private GameObject numWeapon;

    private WeaponChange _WeaponChange;
    //private WeaponStatus _WeaponStatus;

    [SerializeField] private GameObject fpsCamera;
    [SerializeField] private GameObject tpsCamera;

    public GameObject reticle;

    // Use this for initialization
    void Start () {
        //player = GameObject.Find("Player_Sample");

        //_WeaponStatus = this.gameObject.GetComponent<WeaponStatus>();
        _WeaponChange = this.gameObject.GetComponent<WeaponChange>();
        numWeapon = _WeaponChange.GetWeapon();

        CameraCheck();
    }
	
	// Update is called once per frame
	void Update () {
        _WeaponChange.Change();
        CameraCheck();
    }

    public void Reload()
    {
        if(numWeapon.name == "TestWeapon")
        {
            numWeapon.GetComponent<TestWeapon>().Reload();
        }
        if (numWeapon.name == "SampleWeapon")
        {
            numWeapon.GetComponent<SampleWeapon>().Reload();
        }
    }

    public void Shooting()
    {
        if (numWeapon.name == "TestWeapon")
        {
            numWeapon.GetComponent<TestWeapon>().Shot();
        }
        if (numWeapon.name == "SampleWeapon")
        {
            numWeapon.GetComponent<SampleWeapon>().Shot();
        }
    }

    public void SetWeapon(GameObject obj)
    {
        numWeapon = obj;
    }

    public GameObject GetWeapon()
    {
        return numWeapon;
    }

    public void CameraCheck()
    {
        if (fpsCamera.activeInHierarchy)
        {
            transform.parent = fpsCamera.transform;
            reticle.SetActive(true);
        }
        if (tpsCamera.activeInHierarchy)
        {
            transform.parent = player.transform;
            reticle.SetActive(false);
        }
    }
}
