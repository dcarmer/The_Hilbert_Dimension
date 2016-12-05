using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Killfeed : MonoBehaviour {

    private const float FADE_DELAY = 2.0f;
    private const float TIME_TO_LIVE = 10.0f;
    private const int MAXIMUM = 5;

    private float kicknext = -1;
    private Queue<string> kills = new Queue<string>();

	// Use this for initialization
	void Start () {
	
	}
	
    public static void ReportKill(int killer, int victim)
    {
        GameObject.Find("Killfeed").GetComponent<Killfeed>().Kill(killer, victim);
    }

    public void Kill(int killer, int victim)
    {
        if (this.kicknext < 0) this.kicknext = TIME_TO_LIVE;
        string kill = ColorAlgorithm.GetName(killer) + " has killed " + ColorAlgorithm.GetName(victim);
        kills.Enqueue(kill);
        if (kills.Count > MAXIMUM) kills.Dequeue();
        UpdateText();
    }

    void UpdateText()
    {
        Text text = this.gameObject.GetComponent<Text>();
        text.text = "";
        if (kills.Count == 0)
        {
            text.text = "";
            return;
        }
        Queue<string> temp = new Queue<string>();

        int count = kills.Count;
        for(int i = 0; i < count; i++)
        {
            string kill = kills.Dequeue();

            text.text += kill + "\n";

            temp.Enqueue(kill);
        }

        kills = temp;

    }

	// Update is called once per frame
	void Update () {
	    if(kills.Count > 0)
        {
            kicknext -= Time.deltaTime;
            if(kicknext <= 0)
            {
                kills.Dequeue();
                if (kills.Count > 0) kicknext = TIME_TO_LIVE;
                else kicknext = -1;
                UpdateText();
            }
        }
	}
}
