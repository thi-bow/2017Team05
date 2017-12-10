using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PargeAttackCollider : MonoBehaviour {

    [SerializeField] float sizeUpspeed = 1.0f;
    bool _parge = false;
    int _attackPower = 1000;
    float _collderSize = 5.0f;
	
	// Update is called once per frame
	void Update ()
    {
        if (_parge)
        {
            this.GetComponent<SphereCollider>().radius += Time.deltaTime * sizeUpspeed;
            if (this.GetComponent<SphereCollider>().radius >= _collderSize)
            {
                _parge = false;
                this.gameObject.SetActive(false);
            }
        }
		
	}

    public void PargeStart(int power, float collderSize)
    {
        this.GetComponent<SphereCollider>().radius = 0.5f;
        _attackPower = power;
        _collderSize = collderSize;
        _parge = true;
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.GetComponent<BoneCollide>() != null && other.tag != this.tag)
        {
            other.gameObject.GetComponent<BoneCollide>().Damage(_attackPower, Weapon.Attack_State.NULL);
        }
    }


}
