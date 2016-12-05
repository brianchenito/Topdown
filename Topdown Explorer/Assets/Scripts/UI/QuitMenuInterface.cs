using UnityEngine;
using System.Collections;

public class QuitMenuInterface : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// disable the quit menu.
    /// </summary>
    public void hideQuit()
    {
        this.gameObject.SetActive(false);
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
