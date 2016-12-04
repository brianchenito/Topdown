using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class pathNode
{
    public static List<Vector3> adjacents = new List<Vector3>()
    { new Vector3(2.5f,0,0),new Vector3(-2.5f,0,0),new Vector3(0,0,2.5f),new Vector3(0,0,-2.5f),
        new Vector3(2.5f,0,2.5f),new Vector3(2.5f,0,-2.5f),new Vector3(-2.5f,0,2.5f),new Vector3(-2.5f,0,-2.5f),
    };
    public Vector3 position { get; set; }
    public pathNode previous;
    public float heuristic { get; set; }
    public float partialcost { get; set; }
    public float fScore
    {
        get { return heuristic + partialcost; }
    }
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


public class AStarPathfinder {
    static int depthlimit = 500;
    static int layerMask = 1 << 9;
    /// <summary>
    /// generate a lowish granularity(5 units per node) path from startpos to endpos.
    /// vectors in list represent absolute node positions.
    /// </summary>
    /// <param name="startpos"></param>
    /// <param name="endpos"></param>
    /// <returns></returns>
    public static List<Vector3> GeneratePath(Vector3 startpos, Vector3 endpos)
    {
        HashSet<Vector3> closedSet = new HashSet<Vector3>();  // explored positions.
        List<pathNode> fringe = new  List<pathNode>();// q to explore
        pathNode startNode = new pathNode(startpos, null, guesstimate(startpos, endpos), 0);
        fringe.Add(startNode);
        int depth = 0;
        while (fringe.Count != 0)
        {
            depth++;
            if (depth >= depthlimit)
            {
                //abort
                Debug.Log("hit depth lim");
                return new List<Vector3>();
            }
            pathNode current = getClosest(fringe);
            // if we have pathed close enough to target position, build a path to send
            if (Vector3.Magnitude(current.position - endpos) <= 5)
            {
                List<Vector3> returnpath = new List<Vector3>();
                while (current.previous != null)
                {
                    returnpath.Insert(0, current.position);
                    current = current.previous;
                }
                return returnpath;
            }
            //transfer node to closed set
            closedSet.Add(current.position);
            //adjacency check eight nearby potential nodes.
            foreach (Vector3 i in pathNode.adjacents)
            {
                if (closedSet.Contains(current.position + i)) continue;
                bool alreadyinfringe = false;
                float guessfscore = guesstimate(current.position + i, endpos);
                foreach (pathNode j in fringe)
                {
                    if (j.position == current.position + i)
                    {
                        alreadyinfringe = true;
                        break;
                    }  
                }
                // add to fringe if theres nothing there, and position is not already in fringe
                if (!alreadyinfringe&&isClear(current.position,i))
                {
                    pathNode newnode = new pathNode(current.position + i, current, guessfscore, current.partialcost + i.magnitude);
                    //identify appropriate 
                    fringe.Add(newnode);
                }
            }
        }
        return null;
    }
    //checks if a 5 u wide entity can path through without collisions
    private static bool isClear(Vector3 startposition, Vector3 direction)
    {
        return !(Physics.Raycast(startposition + Vector3.up, direction, 7.2f,layerMask)||
                Physics.Raycast(startposition+ 2.5f*Vector3.Normalize(Vector3.Cross(startposition,Vector3.up)) + Vector3.up, direction, 7.2f , layerMask) ||
                Physics.Raycast(startposition + -2.5f* Vector3.Normalize(Vector3.Cross(startposition, Vector3.up)) + Vector3.up, direction, 7.2f, layerMask)
            );
    }

    private static float guesstimate(Vector3 start, Vector3 end)
    {
        return Vector3.Magnitude(end - start);
    }

    private static pathNode getClosest(List<pathNode> fringe)
    {
        if (fringe.Count < 1) return null;
        float closest = int.MaxValue;
        pathNode retnode = fringe[0];
        foreach (pathNode p in fringe)
        {
            if (p.fScore < closest)
            {
                closest = p.fScore;
                retnode = p;
            }
        }
        fringe.Remove(retnode);
        return retnode; 

    }
}
