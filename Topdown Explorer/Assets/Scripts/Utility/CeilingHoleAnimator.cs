using UnityEngine;
using System.Collections;

public class CeilingHoleAnimator : MonoBehaviour {
    public GameObject left;
    public GameObject right;
    public Light thislight;
    private float i = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (i>100&&i < 600)
        {
            right.transform.localPosition = Vector3.Lerp(new Vector3(3, 0, 5), new Vector3(0, 0, 5), (i-100) / 500);
            left.transform.localPosition = Vector3.Lerp(new Vector3(-3, 0, 5), new Vector3(0, 0, 5), (i-100) / 500);
        }
        else if (i>600)
        {
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        //Debug.Log(i);
        i++;
        thislight.intensity = thislight.intensity * .997f;

    }
}
