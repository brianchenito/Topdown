using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class QuoteGenerator  {
    static List<string> firstNames= new List<string>()
    {
        "Butts",
        "Bill",
        "Lancel",
        "Bernard",
        "Dolores",
        "Emily",
        "Rodrigo",
        "Coral",
        "Carl",
        "Salsa",
        "Lysa",
        "Clifton",
        "Elsie",
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
    static List<string> LoadPhrases = new List<string>()
    {
        "Loading Game",
        "Summoning Skeltals",
        "Pouring Gasoline on Swords",
        "Failing CSE-175",
        "Spamming Fox's Shine",
        "Procedurally Generating Tiles",
        "Populating Map",
        "Loading Potatoey Placeholder Art"
    };
    public static string GenerateName()
    {
        int r1 = Random.Range(0, firstNames.Count);
        int r2 = Random.Range(0, lastNames.Count);
        return (firstNames[r1] + lastNames[r2]);
    }
    public static string LoadingPhrase()
    {
        int r1 = Random.Range(0, LoadPhrases.Count);
        return (LoadPhrases[r1]);
    }
}
