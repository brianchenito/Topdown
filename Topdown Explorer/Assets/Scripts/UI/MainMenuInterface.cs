using UnityEngine;
using System.Collections;
/// <summary>
/// primary manager for all main menu functionality.
/// </summary>
public class MainMenuInterface : MonoBehaviour {

    public GameObject saveList;// direct set reference to save files ui display
    public GameObject quitMenu;// dirct set reference to quit ui display
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// toggle visibility of saveList.
    /// </summary>
    /// <param name="active"> wheter to turn savelist on or off</param>
    public void showSavesList(bool active)
    {
        Debug.Log("showing savelist");
        saveList.SetActive(active);
    }
    /// <summary>
    /// call up the quit menu.
    /// </summary>
    public void showQuit()
    {
        quitMenu.SetActive(true);
    }
}
