using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class back_to_menu : MonoBehaviour
{
    public void btn_scores_ok_click()
    {
        SceneManager.LoadScene(0);
    }
}
