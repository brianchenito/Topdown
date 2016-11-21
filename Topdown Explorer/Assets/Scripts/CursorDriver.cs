using UnityEngine;
using System.Collections;

public class CursorDriver : MonoBehaviour {

    public Texture2D cursorTexture;
    public Vector2 hotspot;
    void Start () {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);

    }
    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    // Update is called once per frame
    void Update () {
	
	}
}
