using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class type_text_effect : MonoBehaviour
{

    public float delay = 0.1f;
    private string txt_to_display;
    private string current_text = "";


    public void type_text(string text)
    {
        txt_to_display = text;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= txt_to_display.Length; i++)
        {
            current_text = txt_to_display.Substring(0, i);
            this.GetComponent<Text>().text = current_text;
            yield return new WaitForSeconds(delay);
        }
    }
}
