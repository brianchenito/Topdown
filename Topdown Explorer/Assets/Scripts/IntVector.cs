using UnityEngine;
using System.Collections;
/// <summary>
/// A simple tuple of two ints.
/// </summary>
public struct IntVector {
    public int x { get; set; }
    public int y { get; set; }
    public float magnitude
    {
        get { return Mathf.Sqrt(x * x + y * y); }
    }

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
    public override bool Equals(object obj)
    {
        if (!(obj is IntVector))return false;
        IntVector castobj = (IntVector)obj;

        return (x == castobj.x && y == castobj.y);
    }
    public override int GetHashCode()
    {
        unchecked 
        {
            int hash = 17;
            hash = hash * 23 + x.GetHashCode();
            hash = hash * 23 + y.GetHashCode();
            return hash;
        }
    }

    public static IntVector operator -(IntVector one, IntVector two)
    {
        return new IntVector(one.x - two.x, one.y - two.y);
    }
    public static IntVector operator +(IntVector one, IntVector two)
    {
        return new IntVector(one.x + two.x, one.y + two.y);
    }
    public static IntVector operator *(int one, IntVector two)
    {
        return new IntVector(one * two.x, one * two.y);
    }
    public static IntVector operator *(IntVector one, int two)
    {
        return new IntVector(one.x * two, one.y * two);

    }
    public static IntVector operator /(IntVector one, int two)
    {
        return new IntVector(one.x / two, one.y / two);
    }
    public static bool operator !=(IntVector one, IntVector two)
    {
        return (one.x != two.x || one.y != two.y);
    }
    public static bool operator ==(IntVector one, IntVector two)
    {
        return (one.x == two.x && one.y == two.y);
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
