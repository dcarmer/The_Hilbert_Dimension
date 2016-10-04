using UnityEngine;
using System.Collections;

public class ColorAlgorithm : MonoBehaviour {
    private const int MAXIMUM_PLAYERS = 9;
    //private static bool initialized = false;
    private static Color[] colors;
    public int player_id;
    private int real_id;


	// Use this for initialization
	void Start () {
        if (colors == null) init();
        real_id = player_id;
        GetComponent<Renderer>().material.color = colors[player_id];
	}
	
    private void init()
    {
        int value = 256;
        colors = new Color[MAXIMUM_PLAYERS];


        for (int i = 0; i < MAXIMUM_PLAYERS; i++)
        {
            int r = 0;
            int g = 0;
            int b = 0;
            if (i % 7 == 6)
            {
                r = 1;
                g = 1;
                b = 1;
            }
            else
            {
                if (i % 7 == 0 || i % 7 == 3 || i % 7 == 5) r = 1;
                if (i % 7 == 1 || i % 7 == 3 || i % 7 == 4) g = 1;
                if (i % 7 == 2 || i % 7 == 4 || i % 7 == 5) b = 1;
            }
            if (i % 7 == 0 && i > 0) value /= 2;

            colors[i] = new Color(((value - 1) * r)/255, ((value - 1) * g)/255, ((value - 1) * b)/255, 1);
        }
    }

	// Update is called once per frame
	void Update () {
	    if(player_id != real_id)
        {
            real_id = player_id;
            GetComponent<Renderer>().material.color = colors[player_id];
        }
	}
}
