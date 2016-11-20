using UnityEngine;
using System.Collections;
/// <summary>
/// A simple tuple of two ints.
/// </summary>
public struct IntVector {
    public int x { get; set; }
    public int y { get; set; }
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="x">x value</param>
    /// <param name="y">y value</param>
    public IntVector(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    /// <summary>
    /// returns the strictly cartesian distance to a target IntVector. 
    /// distance from (4,4) to (5,5) is 2.
    /// distance from (4,4) to (5,4) is 1.
    /// </summary>
    /// <param name="target"> target vector to compare against</param>
    /// <returns> integer abs distance.</returns>
    public int cartesianDistanceto(IntVector target)
    {
        int xdist = target.x - x;
        if (xdist < 0) xdist *= -1;
        int ydist = target.y - y;
        if (ydist < 0) ydist *= -1;
        return xdist+ydist;
    }

}
