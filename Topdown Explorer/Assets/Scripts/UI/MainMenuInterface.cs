using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// primary manager for all main menu functionality.
/// </summary>
public class MainMenuInterface : MonoBehaviour {
    public SQLInterface sql;
    public GameManager gameManager;

    public GameObject saveList;// direct set reference to save files ui display
    public GameObject saveMask;
    public List<GameObject> saveButtons;


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
        if (saves != null||saves.Count<1)
        {
            saveList.transform.Find("No Saves").gameObject.SetActive(false);
            foreach (KeyValuePair<int, string> pair in saves)
            {
                addToSaveList(pair.Key, pair.Value);
            }
        }
        else
        {
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
        //sql.GenerateNewTiles(new IntVector(50,0), new IntVector(50, 1),1);
    }


    public void addToSaveList(int index, string label)
    {
        GameObject newbutton = GameObject.Instantiate(Resources.Load("Prefabs/LoadLevelButton")) as GameObject;
        newbutton.transform.SetParent (saveMask.transform);
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
        gameManager.Loadscreen.SetActive(true);
        PlayerEntry = newgameScreen.transform.FindChild("InputFieldPlayer").GetComponent<InputField>().text;
        if (PlayerEntry == "") PlayerEntry = QuoteGenerator.GenerateName();
        SaveEntry = newgameScreen.transform.FindChild("InputFieldSave").GetComponent<InputField>().text;
        if (SaveEntry == "") SaveEntry = "New Save " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        int index = sql.CreateNewSave(SaveEntry, PlayerEntry);

        Debug.Log("instantiating save with Player, "+ PlayerEntry+" Save, "+ SaveEntry);
        gameManager.newgame = true;
        gameManager.LaunchGame(index);

    }
    public void Hyperlink()
    {
        Application.OpenURL("https://github.com/brianchenito/Topdown");
    }
}
