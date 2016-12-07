using UnityEngine;
using System.Collections;

public class PropStats {
    public int index { get; set; }
    public string name { get; set; }
    public IntVector GCoords { get; set; }
    public Vector2 LCoord { get; set; }
    public Vector2 size { get; set; }
    public PropStats(int _index, string _name, IntVector _GCoords, Vector2 _Lcoords, Vector2 _size)
    {
        index = _index;
        name = _name;
        GCoords = _GCoords;
        LCoord = _Lcoords;
        size = _size;

    }
}
