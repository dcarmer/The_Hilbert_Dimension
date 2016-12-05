using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Leaderboards : MonoBehaviour {

    private Dictionary<int, int> kills = new Dictionary<int, int>();
    private Dictionary<int, int> deaths = new Dictionary<int, int>();
    private int yourKills = 0;
    public static int id;
    private int[] leaderboards;

    
    public static void ReportKill(int killer, int victim)
    {
        GameObject.Find("Killfeed").GetComponent<Leaderboards>().Kill(killer, victim);
        Killfeed.ReportKill(killer, victim);
    }

    public void Kill(int killer, int victim)
    {
        if (killer == id) yourKills++;
        if (!kills.ContainsKey(killer))
        {
            this.Add(killer);
        }
        int val;
        kills.TryGetValue(killer, out val);
        kills.Add(killer, val + 1);
        this.UpdateLeaderboards();
    }

    public void Sort()
    {
        int length = leaderboards.Length;
        for(int i = 0; i < length - 1; i++)
        {
            int max = -1;
            int maxi = i;
            for(int k = i + 1; k < length; k++)
            {
                int val;
                kills.TryGetValue(leaderboards[k], out val);
                if (val > max)
                {
                    max = val;
                    maxi = k;
                }
            }
            int swap = leaderboards[i];
            leaderboards[i] = leaderboards[maxi];
            leaderboards[maxi] = swap;
        }
    }

    private void Add(int id)
    {
        int[] temp = new int[leaderboards.Length + 1];

        for(int i = 0; i < leaderboards.Length; i++)
        {
            temp[i] = leaderboards[i];
        }

        temp[leaderboards.Length] = id;
        leaderboards = temp;
    }

    private void UpdateLeaderboards()
    {
        this.Sort();
        Text text = this.gameObject.GetComponent<Text>();

        text.text = "Kills: " + yourKills + "\n";
        text.text += "-----------------------\n";

        for (int i = 0; i < leaderboards.Length; i++)
        {
            string name = ColorAlgorithm.GetName(leaderboards[i]);
            int val;
            kills.TryGetValue(leaderboards[i], out val);
            text.text += (i + 1) + " | " + name + " | " + val;
        }
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
