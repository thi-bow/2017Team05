using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    [SerializeField] GameObject _parentParts = null;
    [SerializeField] Player _player = null;
    [SerializeField] CharaBase.Parts _parts = CharaBase.Parts.Body;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            return;
        }
        else if(other.tag == "Enemy")
        {

        }
        _player.PartsPurge(_parts);
        this.gameObject.transform.SetParent(_parentParts.transform);
        this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
