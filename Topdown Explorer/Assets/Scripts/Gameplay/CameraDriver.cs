using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Camera))]

public class CameraDriver : MonoBehaviour {
    public GameObject player;
    public static Plane groundPlane;
    private static float smoothSpeed = 7f;  // speed at which camera moves
    private static float lookdistance = 4.2f; // distance that the camera moves towards cursor
    private static Vector3 offset = new Vector3(0,50,-8);// camera offset from target

    void Start () {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        player = GameObject.Find("PlayerCharacter");
        if (player == null) throw new System.Exception("unable to find PlayerCharacter");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 400, transform.position.z), smoothSpeed * Time.deltaTime);

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 35, transform.position.z), smoothSpeed * Time.deltaTime);

        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // cast a ray normal to the camera at mouseposition
        float rayDistance;  //records distance to groundplane from ray emitter
        Vector3 cursoradditive = Vector3.zero;
        if (groundPlane.Raycast(ray, out rayDistance))//do floor check
        {
            Vector3 adddir = Vector3.Normalize(ray.GetPoint(rayDistance) - transform.position);
            float addistance = Mathf.Log(Vector3.Magnitude(ray.GetPoint(rayDistance) - transform.position));
            cursoradditive = adddir*addistance;

        }
        Vector3 pclocation = player.transform.position;
        transform.position = Vector3.Lerp(transform.position,pclocation +lookdistance*cursoradditive+ offset, smoothSpeed * Time.deltaTime);

    }
}
