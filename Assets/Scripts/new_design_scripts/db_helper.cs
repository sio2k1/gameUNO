using cmn_infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public static class db_helper
{
    public static List<string> intro_story_line(string limit)
    {
        DataTable storyline = sqlite_db_helper.GetTable("SELECT * from storyline where scene='intro' order by [order] ASC " +limit);
        List<string> res = new List<string>();

        foreach (DataRow r in storyline.Select()) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            res.Add(next_text);
        }
        return res;
    }

    public static List<string> intro_hero_text(string limit)
    {
        DataTable storyline = sqlite_db_helper.GetTable("SELECT * from storyline where scene='intro_hero' order by [order] ASC "+limit);
        List<string> res = new List<string>();

        foreach (DataRow r in storyline.Select()) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            res.Add(next_text);
        }
        return res;
    }

}
