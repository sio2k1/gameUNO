using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ladder_board : MonoBehaviour
{
    public GameObject table_line;
    public GameObject ladder_panel;
    // Start is called before the first frame update
    void Start()
    {
        //load_scores();
    }

    public void clear_scores()
    {
        var tagged = GameObject.FindGameObjectsWithTag("ladder_to_del");
        foreach (GameObject go in tagged)
        {
            Destroy(go);
        }
    }
    public void load_scores()
    {
        clear_scores();
        List<db_helper_menu.table_line> lines = db_helper_menu.load_ladder();

        foreach (var l in lines)
        {
            var tl = Instantiate(table_line, new Vector3(0, 0, 0), Quaternion.identity, ladder_panel.transform);
            tl.tag = "ladder_to_del";
            foreach (var t in tl.GetComponentsInChildren<Text>())
            {

                if (t.name.ToLower() == "position")
                {
                    t.text = l.pos;
                }
                if (t.name.ToLower() == "player")
                {
                    t.text = l.player;
                }
                if (t.name.ToLower() == "score")
                {
                    t.text = l.score;
                }
            }
        }


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
