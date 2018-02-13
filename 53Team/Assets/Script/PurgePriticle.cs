using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurgePriticle : MonoBehaviour {
    [SerializeField] private float maxSize = 5.0f;
    private float time = 0;

	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        transform.localScale = Vector3.Lerp(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(maxSize, maxSize, maxSize), time);

        if (time >= 1.0f)
        {
            Destroy(gameObject);
        }
		
	}
}
