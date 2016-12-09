using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public GameManager manager;
    public IntVector GlobalCoords;
    public int index;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("<color=green> ENTERED TILE" + index+"</color>");
        manager.checkNewTiles(GlobalCoords);
    }

}
