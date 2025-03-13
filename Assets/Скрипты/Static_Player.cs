using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class Static_Player
{
    // Start is called before the first frame update
    private static int Collected_Coins = 0;
    private static int Collected_Stars = 0;

    internal static void Collect_Star()
    {
        
    }

    internal static void Collect_Coin()
    {
        Collected_Coins++;
    }
}
