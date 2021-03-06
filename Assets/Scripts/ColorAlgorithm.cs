﻿using UnityEngine;

public static class ColorAlgorithm
{
    private const int MAXIMUM_PLAYERS = 9;
    private static Color[] colors;
    private static string[] colorNames = new string[] {"Red", "Green", "Blue", "Yellow", "Cyan", "Purple", "White", "Black"};
    private static int nextPlayer = 0;

    public static int getPlayerID()
    {
        int o = nextPlayer;
        nextPlayer++;
        return o;
    }

    static ColorAlgorithm()
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

            colors[i] = new Color(((value - 1) * r) / 255, ((value - 1) * g) / 255, ((value - 1) * b) / 255, 1);
        }
    }

    public static string GetName(int i)
    {
        return colorNames[i];
    }

    public static Color GetColor(int i)
    {
       // Debug.Log(i);
        return colors[i];
    }
}
