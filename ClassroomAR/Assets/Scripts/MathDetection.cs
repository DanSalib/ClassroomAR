using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathDetection : MonoBehaviour {

    //Keyword consts
    private const string plusKey = "plus";
    private const string minusKey = "minus";
    private const string timesKey = "times";
    private const string multipliedBy = "multiplied by";
    private const string dividedByKey = "divided by";
    private const string overKey = "over";
    private const string pointKey = "point";
    private const string toThePowerOfKey = "to the power of";
    private const string squaredKey = "squared";
    private const string equalsKey = "equals";
    private const string negativeKey = "negative";
    private const string positiveKey = "positive";


    //Symbol consts
    private const string additionSymbol = "+";
    private const string subtractionSymbol = "-";
    private const string multiplicationSymbol = "*";
    private const string divisionSymbol = "/";
    private const string decimalSymbol = ".";
    private const string exponentSymbol = "^";
    private const string equalitySymbol = "=";

    //Wolfram consts
    private const string wolframUrl = "http://www.wolframalpha.com/input/?i=";
    private const string plusUrlParam = "%2B";
    private const string equalsUrlParam = "%3D";
    private const string divisionUrlParam = "%2F";
    private const string exponentUrlParam = "%5E";

    public Dictionary<string, string> mathKeys = new Dictionary<string, string>();

    public string currentEquationUrl = "";

    // Use this for initialization
    void Start() {
        this.InitializeMathKeys();
    }

    public bool isMath(string text)
    {
        bool containsOperation = false;
        int mathScore = 0;
        string[] words = text.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            float num = 0;
            float.TryParse(words[i], out num);
            if (num != 0)
            {
                mathScore++;
                continue;
            }

            if (this.mathKeys.ContainsKey(words[i].ToLower()))
            {
                if (i > 0 && words[i] == pointKey && DecimalParse(words[i - 1], words[i + 1]) != 0)
                {
                    mathScore++;
                    continue;
                }
                containsOperation = true;
                mathScore++;
                continue;
            } 
        }

        if (mathScore > 2 && containsOperation)
        {
            return true;
        }

        return false;
    }

    public float DecimalParse(string preNum, string postNum)
    {
        string fullNum = preNum + '.' + postNum;
        float parsedNum = 0f;
        float.TryParse(fullNum, out parsedNum);

        return parsedNum;
    }

    private void InitializeMathKeys()
    {
        this.mathKeys.Add(additionSymbol, "+" + plusUrlParam + "+");
        this.mathKeys.Add(plusKey, "+" + plusUrlParam + "+");
        this.mathKeys.Add(minusKey, "-");
        this.mathKeys.Add(equalsKey, "+" + equalsUrlParam + "+");
        this.mathKeys.Add(divisionSymbol, "+" + divisionUrlParam);
        this.mathKeys.Add(timesKey, multiplicationSymbol);
        this.mathKeys.Add(multipliedBy, multiplicationSymbol);
        this.mathKeys.Add(equalsKey, "+" + equalitySymbol);
    }

    public void GenerateWolframLink(string text)
    {
        this.currentEquationUrl = "";
        string link = wolframUrl;

        if(!this.isMath(text))
        {
            return;
        }

        string[] words = text.Split(' ');
        for(int i = 0; i < words.Length; i++)
        {
            if(this.mathKeys.ContainsKey(words[i].ToLower()))
            {
                link += this.mathKeys[words[i].ToLower()];
                continue;
            }

            if(words[i].Length > 1 && (words[i][0] == '+' || words[i][0] == '-'))
            {
                link += "+" + plusUrlParam + "+" + words[i];
                continue;
            }

            link += words[i];
            continue;
        }

         this.currentEquationUrl = link;
    }

    public void OpenWolframLink()
    {
        Application.OpenURL(this.currentEquationUrl);
    }
}
