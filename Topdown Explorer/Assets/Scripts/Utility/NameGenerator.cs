using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NameGenerator  {
    static List<string> firstNames= new List<string>()
    {
        "Butts",
        "Brian",
        "Matt",
        "Eggbert",
        "Bolbo",
        "Butters",
        "Eugene",
        "Adolf",
        "Salty Sam",
        "Tokesmaster",
        "Flexington"
    };
    static List<string> lastNames = new List<string>()
    {
        " McButts",
        " Chen",
        " Pinson",
        " Saladson",
        " Boggins",
        " McFlubbers",
        ", The 3rd",
        ", First of his Name",
        " Eggman",
        " Bobobobo",
        " McFlexesaLot"

    };
    public string GenerateName()
    {
        int r1 = Random.Range(0, firstNames.Count);
        int r2 = Random.Range(0, lastNames.Count);
        return (firstNames[r1] + lastNames[r2]);
    }
}
