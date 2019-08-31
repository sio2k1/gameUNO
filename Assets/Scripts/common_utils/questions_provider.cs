using cmn_infrastructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Web;
using System;

public class opentdbcom_json_parser
{
    public List<opentdbcom_json_entry> results = new List<opentdbcom_json_entry>();
}
public class opentdbcom_json_entry
{
    public string correct_answer;
    public string difficulty;
    public string question;
    public List<string> incorrect_answers = new List<string>();
}
public static class questions_provider
{
    public static class diff_level
    {
        public static string medium = "medium";
        public static string easy = "easy";
        public static string hard = "hard";
    }
   public static List<question> get_questions(int count, string difficulty)
    {
        List<question> qlist = new List<question>();
        try
        {
            WebClient client = new WebClient(); //connect to api and get json
            string jsonstr = client.DownloadString("https://opentdb.com/api.php?amount=" + count.ToString() + "&difficulty="+ difficulty + "&type=multiple");
            opentdbcom_json_parser obj = serializer_helper.json_deserialize_object_from_string<opentdbcom_json_parser>(jsonstr); //deserialize

            foreach (opentdbcom_json_entry en in obj.results) // converting api format to our format
            {
                question q = new question();
                q.question_text = HttpUtility.HtmlDecode(en.question);
                answer correct = new answer(true, HttpUtility.HtmlDecode(en.correct_answer));
                q.answers.Add(correct);

                foreach (string s in en.incorrect_answers)
                {
                    answer incorrect = new answer(false, HttpUtility.HtmlDecode(s));
                    q.answers.Add(incorrect);
                }
                q.shuffle_answers();

                /*if (en.correct_answer.Contains(" ")) //if not contains " " mean answer is one word, write this as typable answer
                {
                    foreach (string s in en.incorrect_answers)
                    {
                        answer incorrect = new answer(false, HttpUtility.HtmlDecode(s));
                        q.answers.Add(incorrect);
                    }
                    q.shuffle_answers();
                }*/
                qlist.Add(q);
            }
        } catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        return qlist;
    }
}
