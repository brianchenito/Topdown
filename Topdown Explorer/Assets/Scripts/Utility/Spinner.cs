using UnityEngine;
using System.Collections;
/// <summary>
/// spins the object this script is attached to.
/// </summary>
public class Spinner : MonoBehaviour {
    public float speed = 10f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, speed * Time.deltaTime,Space.Self);
    }
}
