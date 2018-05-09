using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CalculatorButton : MonoBehaviour {

    [SerializeField]
    private Text input;

    public void OnClick()
    {
        Text buttonText = this.gameObject.GetComponentInChildren<Text>();
        if(buttonText == null)
        {
            return;
        }

        this.input.text += buttonText.text;
    }
}
