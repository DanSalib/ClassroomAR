using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class CalculatorController : MonoBehaviour
{
    [SerializeField]
    private Text inputField;

    [SerializeField]
    private GameObject calculator;

    private float result;

    private Dictionary<char, int> numbers = new Dictionary<char, int> { { '0', 0 }, { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 } };
    private List<char> operators = new List<char> { '+', '-', '/', '*', '^' };

    public void ToggleButtonPress()
    {
        this.calculator.gameObject.SetActive(!this.calculator.gameObject.activeSelf);
    }

    public void ClearButtonPress()
    {
        this.inputField.text = string.Empty;
    }

    public void Evaluate()
    {
        string curNumber = string.Empty;
        char curOperator = ' ';
        foreach(char c in inputField.text)
        {
            if (numbers.ContainsKey(c) || c == '.')
            {
                curNumber += c;
            }
            if(operators.Contains(c))
            {
                float num = 0;
                float.TryParse(curNumber, out num);

                if (curOperator == ' ')
                {
                    this.result = num;
                } else
                {
                    float num1 = this.result;
                    this.result = PerformOperator(num1, num, curOperator);
                }
                curOperator = c;
                curNumber = string.Empty;
            }
        }
        float finalNum = 0;
        float.TryParse(curNumber, out finalNum);

        this.result = PerformOperator(this.result, finalNum, curOperator);

        this.inputField.text = result.ToString();
    }

    private float PerformOperator(float num1, float num2, char curOperator)
    {
        switch(curOperator)
        {
            case '+':
                return num1 + num2;
            case '-':
                return num1 - num2;
            case '*':
                return num1 * num2;
            case '/':
                return num1 / num2;
        }
        return num1;
    }

}
