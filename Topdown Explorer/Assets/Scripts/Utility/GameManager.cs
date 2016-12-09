using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(SQLInterface))]
/// <summary>
/// interprets io from <see cref="SQLInterface"/> and performs in game actions.  
/// </summary>
public class GameManager : MonoBehaviour {
    //locals
    private bool isPaused;
    private AsyncOperation loadop;
    public  bool newgame;

    //gameplay
    private int activeSaveIndex = 0;
    private List<GameObject> tiles=new List<GameObject>();


    //ui
    public GameObject pauseScreen;
    public GameObject Loadscreen;
    public GameObject quitMenu;// dirct set reference to quit ui display
    public Text loadtext;

    //helpers
    public static int mapscale=100;
    public SQLInterface sql;
    public static Dictionary<int, GameObject> EnemyClasses;
    public static Dictionary<int, GameObject> PickupClasses;
    public static Dictionary<int, GameObject> PropClasses;
    public static GameObject TileFab;
	// Use this for initialization
	void Start () {
        newgame = false;
        isPaused = false;
        DontDestroyOnLoad(transform.gameObject);

        EnemyClasses =new Dictionary<int, GameObject>()
        {
            {1,Resources.Load("Prefabs/SlimeBig") as GameObject },
            {2,Resources.Load("Prefabs/SlimeMed") as GameObject },
            {3,Resources.Load("Prefabs/SlimeSmall") as GameObject },
            {4,Resources.Load("Prefabs/Skeltal") as GameObject },
            {5,Resources.Load("Prefabs/SkeltalArcher") as GameObject },
            {6,Resources.Load("Prefabs/SkeltalShield") as GameObject },
            {7,Resources.Load("Prefabs/WizardGuy") as GameObject },
            {8,Resources.Load("Prefabs/Bat") as GameObject },
        };
        PickupClasses = new Dictionary<int, GameObject>
        {
            { 1, Resources.Load("Prefabs/HealthSmall") as GameObject},
            { 2, Resources.Load("Prefabs/HealthBig") as GameObject},
            { 3, Resources.Load("Prefabs/TreasureChest") as GameObject},
            { 4, Resources.Load("Prefabs/Coin") as GameObject},
            { 5, Resources.Load("Prefabs/Arrow") as GameObject},
            { 6, Resources.Load("Prefabs/Shield") as GameObject},
            { 7, Resources.Load("Prefabs/Bow") as GameObject},
            { 8, Resources.Load("Prefabs/FlameSword") as GameObject},  
        };
        PropClasses = new Dictionary<int, GameObject>
        {
            { 1, Resources.Load("Prefabs/Barrel") as GameObject},
            { 2, Resources.Load("Prefabs/Table") as GameObject},
            { 3, Resources.Load("Prefabs/WallHoriz") as GameObject},
            { 4, Resources.Load("Prefabs/WallVert") as GameObject},
            { 5, Resources.Load("Prefabs/WallBendLBot") as GameObject},
            { 6, Resources.Load("Prefabs/WallBendRTop") as GameObject},
            { 7, Resources.Load("Prefabs/SpikedPole") as GameObject},
            { 8, Resources.Load("Prefabs/Chandelier") as GameObject},
        };
        TileFab = Resources.Load("Prefabs/Tile") as GameObject;
    }
	
	void Update () {
        if (loadop != null)
        {
            if (loadop.isDone)
            {
                if (newgame)
                {
                    GameObject.Find("CeilingHole").GetComponent<CeilingHoleAnimator>().enabled=true;
                    GameObject.Find("Camera").GetComponent<CameraShake>().enabled = true;
                    GameObject.Find("CeilingHole").transform.GetChild(0).gameObject.SetActive(true);
                }

                // start instantiating tiles
                List<KeyValuePair<int,IntVector>> tilesdata=sql.getAssociatedTiles(activeSaveIndex);
                foreach (KeyValuePair<int, IntVector> k in tilesdata)
                {
                    Debug.Log("instantiating tile " + k.Key + " at " + k.Value.x + ", " + k.Value.y);
                    GameObject newTile = GameObject.Instantiate(TileFab);
                    newTile.transform.position= new Vector3(k.Value.x, 0, k.Value.y)*mapscale;
                    newTile.GetComponent<Tile>().GlobalCoords = k.Value;
                    newTile.GetComponent<Tile>().index = k.Key;

                    tiles.Add(newTile);
                }

                Loadscreen.SetActive(false);
                loadop = null;
                Time.timeScale = 1;
            }

        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                isPaused = false;
                pauseScreen.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                isPaused = true;
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
            }
        }

    }
    public void LaunchGame(int index)
    {
        Debug.Log("LaunchingGame");
        activeSaveIndex = index;
        loadtext.text = QuoteGenerator.LoadingPhrase();
        Loadscreen.SetActive(true);
        loadop=SceneManager.LoadSceneAsync("Scenes/TopDownScene");
        Time.timeScale = 0;


    }
    /// <summary>
    /// call up the quit menu.
    /// </summary>
    public void showQuit()
    {
        quitMenu.SetActive(true);
    }

    public void ReturntoMainMenu()
    {
        isPaused = false;

        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().name == "Scenes/TopDownScene")
        {
            Debug.Log(" returning");
            loadop = SceneManager.LoadSceneAsync("Scenes/MainMenu");
            Loadscreen.SetActive(true);
            Destroy(gameObject);
        }
        else
        {
            hidePauseScreen();
        }
    }
    public void hidePauseScreen()
    {
        pauseScreen.SetActive(false);
    }
}
