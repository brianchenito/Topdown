using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Camera))]

public class CameraDriver : MonoBehaviour {
    public GameObject player;
    private static Vector3 offset = new Vector3(0,35,-8);// camera offset from target
    public static Plane groundPlane;
    private static float smoothSpeed = 5f;
    void Start () {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        player = GameObject.Find("PlayerCharacter");
        if (player == null) throw new System.Exception("unable to find PlayerCharacter");
	}
	
	// Update is called once per frame
	void Update () {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // cast a ray normal to the camera at mouseposition
        float rayDistance;  //records distance to groundplane from ray emitter
        Vector3 rayhit = Vector3.zero;
        if (groundPlane.Raycast(ray, out rayDistance))//do floor check
        {
            rayhit= ray.GetPoint(rayDistance);

        }
        Vector3 pclocation = player.transform.position;
        transform.position = Vector3.Lerp(transform.position,pclocation + offset, smoothSpeed * Time.deltaTime);

    }
}
