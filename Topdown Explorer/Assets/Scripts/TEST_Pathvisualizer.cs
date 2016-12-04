using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TEST_Pathvisualizer : MonoBehaviour {
    public Plane groundPlane; //plane used for selecting locations on the ground( for movement and such)
    public Vector3 startloc;
    public Vector3 endloc;
    void Start () {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        startloc = Vector3.zero;
        endloc = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) PushRightClickEvent(); //write a left click event later
        if (Input.GetMouseButtonDown(1)) PushRightClickEvent();
    }
    void PushRightClickEvent()
    {
        if (startloc == Vector3.zero) startloc = getlocation();
        else if (endloc == Vector3.zero) endloc = getlocation();
        
        if (startloc!=Vector3.zero && endloc!=Vector3.zero)
        {
            Debug.Log("DrawingPath from " + startloc + " to " + endloc);
            List<Vector3> path= AStarPathfinder.GeneratePath(startloc, endloc);

            Color drawcolor = Color.red;
            float currentcount = 0;
            foreach (Vector3 i in path)
            {
                Debug.Log(drawcolor+"drawing node " + currentcount);
                Gizmos.color = drawcolor;
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                
                cube.transform.position = i;
                cube.GetComponent<Renderer>().material.color = drawcolor;
                drawcolor = Color.Lerp(drawcolor, Color.green, currentcount / path.Count);
                currentcount++;

            }
            startloc = Vector3.zero;
            endloc = Vector3.zero;
            Debug.Log("Draw Complete");
        }
    }


    Vector3 getlocation()
    {
        Vector3 retlocation = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // cast a ray normal to the camera at mouseposition
        float rayDistance;  //records distance to groundplane from ray emitter
        if (groundPlane.Raycast(ray, out rayDistance))//do floor check
        {
            retlocation = ray.GetPoint(rayDistance);
            Debug.Log("Clicked floor at " +retlocation); //use this for moving your ship to a clicked on location.
          
        }
        return retlocation;
    }

}
