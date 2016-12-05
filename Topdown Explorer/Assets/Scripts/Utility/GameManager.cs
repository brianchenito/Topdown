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

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
