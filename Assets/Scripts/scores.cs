using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DELETE OR IMPLEMENT THIS
public static class OLD_score_calculator
{
    public static int points=0;
    public static float time=0f;
    public static void add_right_answer(object level_)
    {
        if (level_.GetType() == typeof(game_level_logic))
        {
            points += 2;
        }
    }
    public static void add_time(float time_)
    {
        time += time_;
    }

    public static int calc_score()
    {
        return Mathf.RoundToInt((points*1000)/time);
    }
}
