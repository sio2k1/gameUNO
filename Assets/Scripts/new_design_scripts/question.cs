using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using UnityEngine;

public static class math_question_builder //this class provides us random calculation tasks
{

    private static int last_rnd = -1;
    private static string act_gen()
    {
        string res = "";
        int rnd = -1;
        while ((rnd == last_rnd) || (res == ""))  //this how we deal with equal random values, due to random gen will generate the same numbers at one tick
        {
            rnd = Random.Range(0, 3);
            if (rnd == 0) { res = "+"; }
            if (rnd == 1) { res = "-"; }
            if (rnd == 2) { res = "*"; }
            //  if (rnd == 3) { res = "/"; } // not using division not to make it complex
        }
        last_rnd = rnd;
        return res;
    }
    private static string math_gen(int complexity)
    {
        int highest_number = complexity;
        int length = complexity / 3;
        string s =  Random.Range(0, highest_number).ToString(); 
        for (int i = 0; i < length + 1; i++)
        {
            s += act_gen() + Random.Range(0, highest_number);
        }
        return s;
    }

    public static question create_math_question(int complexity) //this will generate ez to use question with answer and autocalculate the answer
    {
        question q = new question();
        string math_gened=  math_gen(complexity);
        string value = new DataTable().Compute(math_gened, null).ToString(); //this is not how you should deal with this problem in real life, but its one line calculations solver.
        answer a = new answer(true, value);
        q.answers.Add(a);
        q.question_text = "Calculate this: " + math_gened + "=?";
        return q;
    }
}

public class question 
{
    [DataMember]
    public string question_text;
    public List<answer> answers = new List<answer>();

    public string generate_question_text()
    {
        string res = question_text;

        if (answers.Count>1)
        {
            res = question_text + "\r\n";
            int order = 1;
            foreach (answer a in answers)
            {
                res += order.ToString() + "." + a.txt +" ";
                order++;
            }
        }

        return res;
    }

    public answer get_correct_answer()
    {
        //answers.fi
        return answers.Find(a => a.is_correct);
        //return an[0];
    }

    public string get_correct_answer_position()
    {
        return (answers.IndexOf(answers.Find(a => a.is_correct))+1).ToString();
    }

    public void shuffle_answers() //this we use to shuffle answer order
    {
        if (answers.Count>0)
        {
            List<answer> new_order = new List<answer>();
            while (answers.Count>0)
            {
                int item = Random.Range(0, answers.Count);
                new_order.Add(answers[item]);
                answers.RemoveAt(item);
            }
            answers = new_order;
        }
    }
}

[DataContract]
public class answer
{
    [DataMember]
    public bool is_correct;
    [DataMember]
    public string txt;
    public answer(bool iscorrect, string answ_text)
    {
        is_correct = iscorrect;
        txt = answ_text;
    }
}
