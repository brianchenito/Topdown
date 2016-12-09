using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
    float shake = .3f;
    int count = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (count > 100)
        {
            transform.position = transform.position + Random.onUnitSphere * shake;
            shake = shake * .992f;
        }
        count++;
        if (count > 600) enabled = false;
	}
}
