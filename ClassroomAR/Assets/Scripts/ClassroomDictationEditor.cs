using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEngine.Windows.Speech;
#endif


public class ClassroomDictationEditor : MonoBehaviour {
#if UNITY_EDITOR
    [SerializeField]
    private Text m_Hypotheses;

    [SerializeField]
    private Text m_Recognitions;

    [SerializeField]
    private Button wolframButton;

    [SerializeField]
    private MathDetection mathDetector;

    [SerializeField]
    private Button googleButton;

    [SerializeField]
    private QuestionDetection questionDetector;

    [SerializeField]
    private ScrollRect scrollView;

    private DictationRecognizer m_DictationRecognizer;

    void Start()
    {
        m_DictationRecognizer = new DictationRecognizer();
    
        RectTransform rt = this.scrollView.content.GetComponent<RectTransform>();

        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            UnityEngine.Debug.LogFormat("Dictation result: {0}", text);
            this.MathDetectorCheck(text);
            this.QuestionDetectorCheck(text);

            m_Recognitions.text += text + "\n";
            rt.sizeDelta += new Vector2(0, 22);
            this.scrollView.verticalScrollbar.value = 0f;
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

    private void MathDetectorCheck(string whatToSay)
    {
        this.mathDetector.GenerateWolframLink(whatToSay);
        if (this.mathDetector.currentEquationUrl != "")
        {
            this.wolframButton.gameObject.SetActive(true);
        }
        else
        {
            //this.wolframButton.gameObject.SetActive(false);
        }
    }

    private void QuestionDetectorCheck(string whatToSay)
    {
        this.questionDetector.GenerateGoogleLink(whatToSay);
        if (this.questionDetector.currentUrl != "")
        {
            this.googleButton.gameObject.SetActive(true);
        }
        else
        {
            //this.googleButton.gameObject.SetActive(false);
        }
    }
#endif
}
