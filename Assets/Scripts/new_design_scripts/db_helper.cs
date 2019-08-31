using cmn_infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public static class db_helper
{
    public static List<string> intro_story_line()
    {
        DataTable storyline = sqlite_db_helper.GetTable("SELECT * from storyline where scene='intro' order by [order] ASC " +global_debug_state.is_debug_query);
        List<string> res = new List<string>();

        foreach (DataRow r in storyline.Select()) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            res.Add(next_text);
        }
        return res;
    }

    public static List<string> intro_hero_text()
    {
        DataTable storyline = sqlite_db_helper.GetTable("SELECT * from storyline where scene='intro_hero' order by [order] ASC "+ global_debug_state.is_debug_query);
        List<string> res = new List<string>();

        foreach (DataRow r in storyline.Select()) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            res.Add(next_text);
        }
        return res;
    }

}

public static class db_helper_menu
{
    public class table_line
    {
        public string pos;
        public string player;
        public string score;
        public table_line(string pos_, string player_, string score_)
        {
            pos = pos_;
            player = player_;
            score = score_;
        }
    }
    public static List<table_line> load_ladder()
    {
        DataTable storyline = sqlite_db_helper.GetTable("SELECT  ROW_NUMBER () OVER ( ORDER BY scores desc) RowNum, * from ladder order by scores desc limit 10");
        List<table_line> res = new List<table_line>();

        foreach (DataRow r in storyline.Select())
        {
            table_line de = new table_line(r["RowNum"].ToString(), r["name"].ToString(), r["scores"].ToString());
            res.Add(de);
        }
        return res;

    }

    public static void write_scores(string player_name, int scores)
    {
        sqlite_db_helper.ExecuteQueryWithoutAnswer("INSERT INTO LADDER (NAME, SCORES) VALUES ('" + player_name + "'," + scores + ")");
    }
}

public static class db_helper_questions
{

    public static void add_some_questions_to_db(int number, string compexity)
    {
        List<question> qlist = questions_provider.get_questions(number, questions_provider.diff_level.easy);


        //string comp_digit = "1";
        //if c

        foreach (var q in qlist)
        {
            string json = serializer_helper.json_serialize_object_to_string(q);
            if (sqlite_db_helper.GetTable("SELECT ID FROM QUESTIONS_JSON WHERE JSON='" + json.Replace("'", "") + "'").Rows.Count==0) //if this question not in db
            {
                sqlite_db_helper.ExecuteQueryWithoutAnswer("INSERT INTO QUESTIONS_JSON (COMPLEXITY, JSON) VALUES ('"+compexity+"','" + json.Replace("'", "") + "')");

            }
        }
    }

    public static List<question> load_questions(string level, int qty)
    {
        List<question> qlist = new List<question>();

        if (level.ToLower()==levelnames.West)
        {
            for (int i = 0; i < qty; i++)
            {
                question q = math_question_builder.create_math_question(10 + i);
                if (global_debug_state.is_debug)
                {
                    q.question_text += ":" + q.answers[0].txt; //we will show right answer in question for debug reasons
                }
                qlist.Add(q);
            }
            qlist.Reverse();
        }
        else if ((level.ToLower() == levelnames.East)||(level.ToLower() == levelnames.North))
        {

            string complexity = level.ToLower() == levelnames.North ? questions_provider.diff_level.medium : questions_provider.diff_level.easy;
            string query = "SELECT JSON FROM QUESTIONS_JSON WHERE ID IN (SELECT id FROM QUESTIONS_JSON WHERE COMPLEXITY='" + complexity + "' ORDER BY RANDOM() LIMIT " + qty.ToString() + ")";
            DataTable t = sqlite_db_helper.GetTable(query);
            foreach (var r in t.Select())
            {
                question q = serializer_helper.json_deserialize_object_from_string<question>(r["JSON"].ToString());
                if (global_debug_state.is_debug) //add right answer to query if debugging
                {
                    q.question_text += " (" + q.get_correct_answer().txt + ")";
                    q.shuffle_answers();
                }
                qlist.Add(q);
            }

            //SELECT * FROM table WHERE id IN (SELECT id FROM table ORDER BY RANDOM() LIMIT x)
        }
        else
        { }


        return qlist;
    }

    
}

public static class db_helper_west
{
    public class dialog_entry
    {
        public string txt;
        public string char_t;
        public dialog_entry(string text_, string char_t_)
        {
            txt = text_;
            char_t = char_t_;
        }
    }
    public static List<dialog_entry> level_west_intro_talk() //load an intro dialog for level
    {
        DataTable storyline = sqlite_db_helper.GetTable("SELECT * from storyline where scene = 'west' order by [order] ASC ");
        List<dialog_entry> res = new List<dialog_entry>();

        foreach (DataRow r in storyline.Select()) 
        {
            dialog_entry de = new dialog_entry(r["ctext"].ToString(), r["char_talks"].ToString());
            res.Add(de);
        }
        return res;
    }

    /*public static List<string> intro_hero_text(string limit)
    {
        DataTable storyline = sqlite_db_helper.GetTable("SELECT * from storyline where scene='intro_hero' order by [order] ASC " + limit);
        List<string> res = new List<string>();

        foreach (DataRow r in storyline.Select()) //we starting from story text in narrator box, foreach row in table we display text with delay
        {
            string next_text = r["ctext"].ToString(); //getting a text record from table (field = ctext)
            res.Add(next_text);
        }
        return res;
    }*/

}