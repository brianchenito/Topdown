using UnityEngine;
using System.Collections;

public class CharacterStats {
    public string name { get; set; }
    public IntVector GCoords { get; set; }
    public Vector2 LCoord { get; set; }
    public float exp { get; set; }
    public float health { get; set; }
    public float damage { get; set; }
    public CharacterStats(string _Name, IntVector _GCoords, Vector2 _LCoord, float _exp, float _dmg)
    {
        name = _Name;
        GCoords = _GCoords;
        LCoord = _LCoord;
        exp = _exp;
        damage = _dmg;
    }
}
