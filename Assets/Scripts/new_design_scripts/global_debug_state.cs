using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class global_debug_state
{
    public static bool is_debug = false;
    public static string is_debug_query = "";

    public static void use_debug()
    {
        is_debug = true;
        is_debug_query = "LIMIT 1";
    }
}
