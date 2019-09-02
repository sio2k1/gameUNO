using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//this script is used to display text as typing in bubbles.
public class type_text_effect : MonoBehaviour
{

    public float delay = 0.1f; //delay on typing
    private string txt_to_display;
    private string current_text = "";


    public void type_text(string text) // call this to display something
    {
        txt_to_display = text;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText() // show letters one by one
    {
        for (int i = 0; i <= txt_to_display.Length; i++)
        {
            current_text = txt_to_display.Substring(0, i);
            this.GetComponent<Text>().text = current_text;
            yield return new WaitForSeconds(delay);
        }
    }
}
