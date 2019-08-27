using cmn_infrastructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Web;



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
    public static List<question> get_questions(int count)
    {

        WebClient client = new WebClient();
        string jsonstr = client.DownloadString("https://opentdb.com/api.php?amount="+count.ToString()+"&difficulty=easy&type=multiple");
        opentdbcom_json_parser obj = serializer_helper.json_deserialize_object_from_string<opentdbcom_json_parser>(jsonstr);

        List<question> qlist = new List<question>();
        foreach (opentdbcom_json_entry en in obj.results)
        {
            question q = new question();
            
            q.question_text = HttpUtility.HtmlDecode(en.question);
            answer correct = new answer(true, HttpUtility.HtmlDecode(en.correct_answer));
            q.answers.Add(correct);

            


            if (en.correct_answer.Contains(" ")) //if not contains " " mean answer is one word, write this as typable answer
            {
                foreach (string s in en.incorrect_answers)
                {
                    answer incorrect = new answer(false, HttpUtility.HtmlDecode(s));
                    q.answers.Add(incorrect);
                }
                q.shuffle_answers();
            }

            qlist.Add(q);

        }
        return qlist;
    }


}
