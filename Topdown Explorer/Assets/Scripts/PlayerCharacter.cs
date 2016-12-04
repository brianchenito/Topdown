using UnityEngine;
using System.Collections;
[RequireComponent( typeof(CharacterController))]
/// <summary>
/// driving script of player character.
/// </summary>
public class PlayerCharacter : MonoBehaviour {
    public static Plane groundPlane;
    // Use this for initialization
    private Vector3 inputdirection;
    public float movespeed;
    public float experience;
    public float health;
    public float damage;
    public static float turnspeed;
    protected CharacterController controller;   // controller for this particular character (component needs to be added in editor)

    void Start () {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        movespeed = 25;
        turnspeed = 10;
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    ///
    void Update()
    {
        //move character
        inputdirection.x = (Input.GetAxisRaw("Horizontal"));
        inputdirection.z = (Input.GetAxisRaw("Vertical"));
        controller.Move(inputdirection * movespeed * Time.deltaTime);


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // cast a ray normal to the camera at mouseposition
        float rayDistance;  //records distance to groundplane from ray emitter
        if (groundPlane.Raycast(ray, out rayDistance))//do floor check
        {
            Vector3 cursorpos = ray.GetPoint(rayDistance)+Vector3.up*transform.position.y;
            //Debug.Log(cursorpos);
            Debug.Log(transform.rotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cursorpos - transform.position,Vector3.up), turnspeed * Time.deltaTime);
            
        }
    }
    void FixedUpdate()
    {       
    }
}
