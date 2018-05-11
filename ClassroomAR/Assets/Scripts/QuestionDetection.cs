using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDetection : MonoBehaviour
{

    //Keyword consts
    private const string whoKey = "who";
    private const string whatKey = "what";
    private const string whereKey = "where";
    private const string whenKey = "when";
    private const string whyKey = "why";
    private const string howKey = "how";

    //Wolfram consts
    private const string googleUrl = "https://www.google.ca/search?source=hp&ei=dAT1Wtz7OuGYjwTH06_AAg&q=";

    public List<string> questionKeys = new List<string> { whoKey, whatKey, whereKey, whenKey, whyKey, howKey };

    public string currentUrl = "";

    public bool IsQuestion(string text)
    {
        string[] words = text.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (this.questionKeys.Contains(words[i].ToLower()))
            {
                return true;
            }
        }

        return false;
    }

    public void GenerateGoogleLink(string text)
    {
        this.currentUrl = "";
        string link = googleUrl;

        if (!this.IsQuestion(text))
        {
            return;
        }

        string[] words = text.Split(' ');
        int i = 0;
        for (i = 0; i < words.Length; i++)
        {
            if (this.questionKeys.Contains(words[i].ToLower()))
            {
                link += words[i];
                break;
            }
        }

        for(int j = i + 1 ; j < words.Length && j < 10 ; j++)
        {
            if(words[j] == null)
            {
                break;
            }

            link += "+" + words[j];
        }

        this.currentUrl = link;
    }

    public void OpenGoogleLink()
    {
        Application.OpenURL(this.currentUrl);
    }
}
