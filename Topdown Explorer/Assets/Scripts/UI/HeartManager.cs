using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HeartManager : MonoBehaviour {

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public Sprite s0;
    public Sprite s1;
    public Sprite s2;
    public Sprite s3;
    public Sprite s4;
    private Dictionary<int, Sprite>bindings;
    private bool pulse;


    static Color maxcolor = new Color(0.5f, 0.05f, 0.05f, 1f);
    static Color mincolor = new Color(0.4f, 0.3f, 0.3f, 0f);
    // Use this for initialization
    void Start () {
        bindings = new Dictionary<int, Sprite>
    {
        {0,s0 },
        {1,s1 },
        {2,s2 },
        {3,s3 },
        {4,s4 },
    };
        int random = (int)Mathf.Round(Random.Range(0, 12));
        Debug.Log(random);
        SetHp(random);

    }
	
	// Update is called once per frame
	void Update () {

        if (pulse)
        {
            Color newcolor = Color.Lerp(maxcolor, mincolor, Mathf.Sin(8 * Time.realtimeSinceStartup));
            heart1.GetComponent<Outline>().effectColor = newcolor;
            heart2.GetComponent<Outline>().effectColor = newcolor;
            heart3.GetComponent<Outline>().effectColor = newcolor;
            heart1.transform.localScale = Vector3.one * (1 + 0.02f * Mathf.Sin(8 * Time.realtimeSinceStartup));
            heart2.transform.localScale = Vector3.one * (1 + 0.02f * Mathf.Sin(8 * Time.realtimeSinceStartup));
            heart3.transform.localScale = Vector3.one * (1 + 0.02f * Mathf.Sin(8 * Time.realtimeSinceStartup));

        }
        else
        {
            heart1.GetComponent<Outline>().effectColor = mincolor;
            heart2.GetComponent<Outline>().effectColor = mincolor;
            heart3.GetComponent<Outline>().effectColor = mincolor;
            heart1.transform.localScale = Vector3.one;
            heart2.transform.localScale = Vector3.one;
            heart3.transform.localScale = Vector3.one;
        }


    }
    public void SetHp(int hp)
    {
        if (hp >= 12)
        {
            heart3.GetComponent<Image>().sprite = s4;
        }
        else { heart3.GetComponent<Image>().sprite = bindings[hp % 4]; }
        if (hp >= 8)
        {
            heart2.GetComponent<Image>().sprite = s4;
        }
        else {
            heart3.GetComponent<Image>().sprite = s0;
            heart2.GetComponent<Image>().sprite = bindings[hp % 4];
        }
        if (hp > 6)
        {
            pulse = false;
        }
        else
        {
            pulse = true;
        }
        if (hp >= 4)
        {
            heart1.GetComponent<Image>().sprite = s4;
        }
        else {
            heart2.GetComponent<Image>().sprite = s0;
            heart1.GetComponent<Image>().sprite = bindings[hp%4];
        }


    }
}
