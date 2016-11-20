using UnityEngine;
using System.Collections;
[RequireComponent( typeof(CharacterController))]
/// <summary>
/// driving script of player character.
/// </summary>
public class PlayerCharacter : MonoBehaviour {

    // Use this for initialization
    private Vector3 inputdirection;
    public float movespeed;
    public float experience;
    public float health;
    public float damage;
    protected CharacterController controller;   // controller for this particular character (component needs to be added in editor)

    void Start () {
        movespeed = 25;
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    ///
    void Update () {
        inputdirection.x = (Input.GetAxisRaw("Horizontal"));
        inputdirection.z = (Input.GetAxisRaw("Vertical"));

        controller.Move(inputdirection*movespeed * Time.deltaTime);
    }
    void FixedUpdate()
    {       
    }
}
