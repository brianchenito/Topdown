using UnityEngine;
using System.Collections;

public class PickupStats  {
    public int index { get; set;}
    public string name { get; set; }
    public IntVector GCoords { get; set; }
    public Vector2 LCoord { get; set; }
    public float exp { get; set; }
    public PickupStats(int _index, string _name, IntVector _GCoords, Vector2 _LCoord, float _exp)
    {
        index = _index;
        name = _name;
        GCoords = _GCoords;
        LCoord = _LCoord;
        exp     = _exp;
    }
}
