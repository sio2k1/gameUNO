using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is for defining global debug state, mainly used to lower game duration by cut some dialogs.
public static class global_debug_state
{
    public static bool is_debug = false;
    public static string is_debug_query = "";
    public static int questions_per_level = 4; // how many questions per level, this "4" should be defined in somekind of global vars sic!

    public static void use_debug()
    {
        is_debug = true;
        questions_per_level = 2;
        is_debug_query = "LIMIT 1"; // we add this limit to sql queries where we want to display just one dext message instead of all (for narration in game start for example)
    }
}
