using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// primary manager for all main menu functionality.
/// </summary>
public class MainMenuInterface : MonoBehaviour {
    public SQLInterface sql;
    public static GameManager gameManager;

    public GameObject saveList;// direct set reference to save files ui display
    public List<GameObject> saveButtons;

    public GameObject quitMenu;// dirct set reference to quit ui display

    public GameObject newgameScreen;

    private string PlayerEntry;
    private string SaveEntry;

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
        //clear old loadup
        foreach (GameObject g in saveButtons)
        {
            Object.Destroy(g);
        }
        saveButtons.Clear();
        List<KeyValuePair<int, string>> saves = sql.GetSaves();
        if (saves != null)
        {
            saveList.transform.Find("No Saves").gameObject.SetActive(false);
            foreach (KeyValuePair<int, string> pair in saves)
            {
                addToSaveList(pair.Key, pair.Value);
            }
        }
        else
        {
            Debug.Log("<Color=red>GetSaves() is not yet implemented</Color>");
            saveList.transform.Find("No Saves").gameObject.SetActive(true);

        }

        saveList.SetActive(active);
    }
    /// <summary>
    /// show menu for creating a new game.
    /// </summary>
    /// <param name="active"></param>
    public void shownewGameScreen(bool active)
    {
        newgameScreen.SetActive(active);
    }

    /// <summary>
    /// call up the quit menu.
    /// </summary>
    public void showQuit()
    {
        quitMenu.SetActive(true);
    }
    public void addToSaveList(int index, string label)
    {
        GameObject newbutton = GameObject.Instantiate(Resources.Load("Prefabs/LoadLevelButton")) as GameObject;
        newbutton.transform.parent = saveList.transform;
        newbutton.GetComponentInChildren<Text>().text = label;
        newbutton.transform.localPosition = new Vector2(0,150-50*saveButtons.Count);
        saveButtons.Add(newbutton);
        newbutton.GetComponent<Button>().onClick.AddListener(() => gameManager.LaunchGame(index));

    }
    
 
    /// <summary>
    /// Start a game.
    /// </summary>
    /// <param name="saveFile"></param>
    public void LaunchNewGame()
    {
        int index = sql.CreateNewSave(SaveEntry, PlayerEntry);
        PlayerEntry = newgameScreen.transform.FindChild("InputFieldPlayer").GetComponent<InputField>().text;
        SaveEntry = newgameScreen.transform.FindChild("InputFieldSave").GetComponent<InputField>().text;
        Debug.Log("instantiating save with Player, "+ PlayerEntry+" save "+ SaveEntry);
        
        gameManager.LaunchGame(index);

    }
    public void Hyperlink()
    {
        Application.OpenURL("https://github.com/brianchenito/Topdown");
    }
}
