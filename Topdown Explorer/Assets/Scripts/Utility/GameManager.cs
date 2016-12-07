using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(SQLInterface))]
/// <summary>
/// interprets io from <see cref="SQLInterface"/> and performs in game actions.  
/// </summary>
public class GameManager : MonoBehaviour {
    public static GameObject Character;
    public static Dictionary<int, GameObject> EnemyClasses;
    public static Dictionary<int, GameObject> PickupClasses;
    public static Dictionary<int, GameObject> PropClasses; 

	// Use this for initialization
	void Start () {
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

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void LaunchGame(int index) { }
}
