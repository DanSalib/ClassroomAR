using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CalculatorButton : MonoBehaviour {

    [SerializeField]
    private Text input;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
