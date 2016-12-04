using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class pathNode
{
    Vector3 position { get; set; }
    pathNode previous;
    float heuristic { get; set; }
    float partialcost { get; set; }
    public pathNode(Vector3 _position)
    {
        this.position = _position;
        this.previous = null;
        heuristic = int.MaxValue;
        partialcost = int.MaxValue;

    }
    public pathNode(Vector3 _position, pathNode _previous)
    {
        this.position = _position;
        this.previous = _previous;
        heuristic = int.MaxValue;
        partialcost = int.MaxValue;
    }
    public pathNode(Vector3 _position, pathNode _previous, float _heuristic, float _partial)
    {
        this.position = _position;
        this.previous = _previous;
        heuristic = _heuristic;
        partialcost = _partial;
    }

}


public class AStarPathfinder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// generate a lowish granularity(5 units per node) path from startpos to endpos.
    /// vectors in list represent absolute node positions.
    /// </summary>
    /// <param name="startpos"></param>
    /// <param name="endpos"></param>
    /// <returns></returns>
    public List<Vector3> GeneratePath(Vector3 startpos, Vector3 endpos)
    {
        List<pathNode>path = new List<pathNode>();// list of nodes to move toward in sequence
        List<pathNode> closedSet = new List<pathNode>();  // explored set
        List<pathNode> fringe = new List<pathNode>();// q to explore
        fringe.Add(new pathNode(startpos,null, guesstimate(startpos,endpos),0));
        while (fringe.Count != 0)
        {

        }











        return null;
    }

    private float guesstimate(Vector3 start, Vector3 end)
    {
        return Vector3.Magnitude(end - start);
    }
}
