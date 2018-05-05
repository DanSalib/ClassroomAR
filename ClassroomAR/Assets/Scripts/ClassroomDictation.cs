using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class ClassroomDictation : MonoBehaviour {

    [SerializeField]
    private Text m_Hypotheses;

    [SerializeField]
    private Text m_Recognitions;

    [SerializeField]
    private Button wolframButton;

    [SerializeField]
    private MathDetection mathDetector;

    private DictationRecognizer m_DictationRecognizer;

    void Start()
    {
        m_DictationRecognizer = new DictationRecognizer();

        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            UnityEngine.Debug.LogFormat("Dictation result: {0}", text);
            if(this.mathDetector.isMath(text))
            {
                this.mathDetector.GenerateWolframLink(text);
                if (this.mathDetector.currentEquationUrl != "")
                {
                    this.wolframButton.gameObject.SetActive(true);
                }
                else
                {
                    this.wolframButton.gameObject.SetActive(false);
                }
            }

            m_Recognitions.text += text + "\n";

        };

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {
            UnityEngine.Debug.LogFormat("Dictation hypothesis: {0}", text);
            m_Hypotheses.text += text;
        };

        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
                UnityEngine.Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
        };

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            UnityEngine.Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        m_DictationRecognizer.Start();
    }
}
