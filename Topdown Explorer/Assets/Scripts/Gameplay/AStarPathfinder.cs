using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// A node for generating paths.
/// </summary>
public class pathNode
{
    public static  float sqrt2= Mathf.Sqrt(2);
    public static List<Vector3> adjacents = new List<Vector3>()
    {
        // the eight directions neighbor nodes can be in 
        new Vector3(2.5f,0,0),
        new Vector3(-2.5f,0,0),
        new Vector3(0,0,2.5f),
        new Vector3(0,0,-2.5f),
        new Vector3(2.5f,0,2.5f),
        new Vector3(2.5f,0,-2.5f),
        new Vector3(-2.5f,0,2.5f),
        new Vector3(-2.5f,0,-2.5f),
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

/// <summary>
///  a statically implemented class for generating paths using the a* algo
/// </summary>
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
        pathNode startNode = new pathNode(startpos, null, guesstimate(startpos,startpos, endpos), 0);
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
                float guessfscore = guesstimate(startpos,current.position + i, endpos);
                float guessgscore = current.partialcost + i.magnitude;
                pathNode foundinFringe=null;
                foreach (pathNode j in fringe)
                {
                    if (j.position == current.position + i)
                    {
                        foundinFringe = j;
                        break;
                    }  
                }
                // add to fringe if theres nothing there, and position is not already in fringe
                if (foundinFringe == null && isClear(current.position, i))
                {
                    pathNode newnode = new pathNode(current.position + i, current, guessfscore, guessgscore);
                    fringe.Add(newnode);
                }
                //update fringe node if a better route to this node is found
                else if (foundinFringe != null && guessgscore < foundinFringe.partialcost)
                {
                    foundinFringe.previous = current;
                    foundinFringe.partialcost = guessgscore;
                    foundinFringe.heuristic = guessfscore;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// checks if a 5u wide entity can fit in a partiular location
    /// </summary>
    /// <param name="startposition"> the position to move from</param>
    /// <param name="direction">the vector to move in</param>
    /// <returns></returns>
    private static bool isClear(Vector3 startposition, Vector3 direction)
    {
        return !(Physics.Raycast(startposition + Vector3.up, direction, 7.2f,layerMask)||
                Physics.Raycast(startposition+ 2.5f*Vector3.Normalize(Vector3.Cross(startposition,Vector3.up)) + Vector3.up, direction, 7.2f , layerMask) ||
                Physics.Raycast(startposition + -2.5f* Vector3.Normalize(Vector3.Cross(startposition, Vector3.up)) + Vector3.up, direction, 7.2f, layerMask)
            );
    }
    /// <summary>
    /// heuristic function for a* prediction
    /// </summary>
    /// <param name="start"> origin point</param>
    /// <param name="end"> destination point</param>
    /// <returns></returns>
    private static float guesstimate(Vector3 start, Vector3 current, Vector3 end)
    {
        float dx1 = start.x - end.x;
        float dz1 = start.z - end.z;
        float dx2 = current.x - end.x;
        float dz2 = current.z - end.z;
        float cross = Mathf.Abs(dx1 * dz2 - dx2 * dz1);
        return (Mathf.Abs(dx2)+Mathf.Abs(dz2)-(pathNode.sqrt2-2)*Mathf.Min(Mathf.Abs(dx2),Mathf.Abs(dz2)))+cross*0.1f;


    }
    /// <summary>
    /// terrible O(n) retrieval of the node with the lowest fScore.
    /// </summary>
    /// <param name="fringe"></param>
    /// <returns></returns>
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
