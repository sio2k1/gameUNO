using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class vis_scoreboarg_game_entry : MonoBehaviour
{
    public Text game_id;
    public GameObject panel;

    public GameObject cell;
    public void add_player(string text)
    {
        cell.GetComponent<Text>().text = text;
        var tl = Instantiate(cell, new Vector3(0, 0, 0), Quaternion.identity, panel.transform); // place on panel to autodistribution with "grid layout group"

    }
}
