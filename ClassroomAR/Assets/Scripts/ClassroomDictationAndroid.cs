using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Diagnostics;
using AUP;

public class ClassroomDictationAndroid : MonoBehaviour
{

    private const string TAG = "[SpeechRecognizerDemo]: ";

    private SpeechPlugin speechPlugin;
    public Text resultText;
    //public Text partialResultText;
    //public Text statusText;

    [SerializeField]
    private Button wolframButton;

    [SerializeField]
    private MathDetection mathDetector;

    [SerializeField]
    private ScrollRect scrollView;

    private Dispatcher dispatcher;
    private UtilsPlugin utilsPlugin;

    private RectTransform scrollRect;

    private Coroutine curCoroutine;

    // Use this for initialization
    void Start()
    {
        dispatcher = Dispatcher.GetInstance();
        // for accessing audio
        utilsPlugin = UtilsPlugin.GetInstance();
        utilsPlugin.SetDebug(0);

        speechPlugin = SpeechPlugin.GetInstance();
        speechPlugin.SetDebug(0);
        speechPlugin.Init();

        // set the calling package this is optional 
        // you can use this if your app is for children or kids
        speechPlugin.SetCallingPackage("com.mycoolcompany.mygame");

        AddSpeechPluginListener();
        this.StartListeningNoBeep();
    }

    private void Awake()
    {
        this.scrollRect = this.scrollView.content.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        AddSpeechPluginListener();
    }

    private void AddSpeechPluginListener()
    {
        if (speechPlugin != null)
        {
            //add speech recognizer listener
            speechPlugin.onReadyForSpeech += onReadyForSp
                eech;
            speechPlugin.onBeginningOfSpeech += onBeginningOfSpeech;
            speechPlugin.onEndOfSpeech += onEndOfSpeech;
            speechPlugin.onError += onError;
            speechPlugin.onResults += onResults;
            speechPlugin.onPartialResults += onPartialResults;
        }
    }

    private void RemoveSpeechPluginListener()
    {
        if (speechPlugin != null)
        {
            //remove speech recognizer listener
            speechPlugin.onReadyForSpeech -= onReadyForSpeech;
            speechPlugin.onBeginningOfSpeech -= onBeginningOfSpeech;
            speechPlugin.onEndOfSpeech -= onEndOfSpeech;
            speechPlugin.onError -= onError;
            speechPlugin.onResults -= onResults;
            speechPlugin.onPartialResults -= onPartialResults;
        }
    }

    private void OnApplicationPause(bool val)
    {
        if (speechPlugin != null)
        {
            if (val)
            {
                RemoveSpeechPluginListener();
            }
            else
            {
                AddSpeechPluginListener();
            }
        }
    }

    public void StartListening()
    {
        bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();

        if (isSupported)
        {
            //number of possible results
            //Note: sometimes even you put 5 numberOfResults, there's a chance that it will be only 3 or 2
            //it is not constant.

            // unmute beep
            utilsPlugin.UnMuteBeep();

            // enable offline
            //speechPlugin.EnableOffline(true);

            // enable partial Results
            speechPlugin.EnablePartialResult(true);

            int numberOfResults = 5;
            speechPlugin.StartListening(numberOfResults);

            //by activating this, the Speech Recognizer will start and you can start Speaking or saying something 
            //speech listener will stop automatically especially when you stop speaking or when you are speaking 
            //for a long time
        }
        else
        {
            UnityEngine.Debug.Log(TAG + "Speech Recognizer not supported by this Android device ");
        }
    }

    public void StartListeningNoBeep()
    {
        this.resultText.text += "start \n";
        this.scrollRect.sizeDelta += new Vector2(0, 22);
        bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();

        if (isSupported)
        {
            //number of possible results
            //Note: sometimes even you put 5 numberOfResults, there's a chance that it will be only 3 or 2
            //it is not constant.

            // mute beep
            utilsPlugin.MuteBeep();

            // enable offline
            //speechPlugin.EnableOffline(true);

            // enable partial Results
            //speechPlugin.EnablePartialResult(true);

            int numberOfResults = 5;
            speechPlugin.StartListening(numberOfResults);
            ///speechPlugin.StartListeningNoBeep(numberOfResults,true);
            if(curCoroutine != null)
            {
                StopCoroutine(this.curCoroutine);
            }

            this.curCoroutine = StartCoroutine("RestartSpeech");

            //by activating this, the Speech Recognizer will start and you can start Speaking or saying something 
            //speech listener will stop automatically especially when you stop speaking or when you are speaking 
            //for a long time
        }
        else
        {
            UnityEngine.Debug.Log(TAG + "Speech Recognizer not supported by this Android device ");
        }
    }

    private IEnumerator RestartSpeech()
    {
        float restartTime = 5.5f;
        while(restartTime > 0)
        {
            yield return new WaitForSeconds(1);
            restartTime--;
        }
        this.CancelSpeech();
        yield return new WaitForSeconds(0.5f);
        this.StartListeningNoBeep();
    }

    //cancel speech
    public void CancelSpeech()
    {
        this.resultText.text += "cancel \n";
        this.scrollRect.sizeDelta += new Vector2(0, 22);

        if (speechPlugin != null)
        {
            bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();

            if (isSupported)
            {
                speechPlugin.Cancel();
            }
        }

        UnityEngine.Debug.Log(TAG + " call CancelSpeech..  ");
    }

    public void StopListening()
    {
        if (speechPlugin != null)
        {
            speechPlugin.StopListening();
        }
        UnityEngine.Debug.Log(TAG + " StopListening...  ");

        this.StartListeningNoBeep();
    }

    public void StopCancel()
    {
        if (speechPlugin != null)
        {
            speechPlugin.StopCancel();
        }
        UnityEngine.Debug.Log(TAG + " StopCancel...  ");
        this.StartListeningNoBeep();

    }

    private void DelayUnMute()
    {
        utilsPlugin.UnMuteBeep();
    }

    private void OnDestroy()
    {
        RemoveSpeechPluginListener();
        speechPlugin.StopListening();
    }

    private void UpdateStatus(string status)
    {
        //if (statusText != null)
        //{
        //    statusText.text = String.Format("Status: {0}", status);
        //}
    }

    //SpeechRecognizer Events
    private void onReadyForSpeech(string data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                if (speechPlugin != null)
                {
                    //Disables modal
                    speechPlugin.EnableModal(false);
                }

                UpdateStatus(data.ToString());
            }
        );
    }

    private void onBeginningOfSpeech(string data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                UpdateStatus(data.ToString());
            }
        );
    }

    private void onEndOfSpeech(string data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                UpdateStatus(data.ToString());
            }
        );
    }

    private void onError(int data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                // unmute beep
                CancelInvoke("DelayUnMute");
                Invoke("DelayUnMute", 0.3f);

                SpeechRecognizerError error = (SpeechRecognizerError)data;
                UpdateStatus(error.ToString());

                if (resultText != null)
                {
                    //resultText.text = "Result: Waiting for result...";
                }
                this.StartListeningNoBeep();
            }
        );
    }

    private void onResults(string data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                if (resultText != null)
                {
                    // unmute beep
                    CancelInvoke("DelayUnMute");
                    Invoke("DelayUnMute", 0.3f);

                    string[] results = data.Split(',');
                    UnityEngine.Debug.Log(TAG + " result length " + results.Length);

                    //when you set morethan 1 results index zero is always the closest to the words the you said
                    //but it's not always the case so if you are not happy with index zero result you can always 
                    //check the other index

                    //sample on checking other results
                    foreach (string possibleResults in results)
                    {
                        UnityEngine.Debug.Log(TAG + " possibleResults " + possibleResults);
                    }

                    //sample showing the nearest result
                    string whatToSay = results.GetValue(0).ToString();

                    if (this.mathDetector.isMath(whatToSay))
                    {
                        this.mathDetector.GenerateWolframLink(whatToSay);
                        if (this.mathDetector.currentEquationUrl != "")
                        {
                            this.wolframButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            this.wolframButton.gameObject.SetActive(false);
                        }
                    }

                    this.resultText.text += whatToSay + "\n";
                    this.scrollRect.sizeDelta += new Vector2(0, 22);
                    this.scrollView.verticalScrollbar.value = 0f;
                }
                this.StartListeningNoBeep();
            }
        );
    }

    private void onPartialResults(string data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                //if (partialResultText != null)
                //{
                //    string[] results = data.Split(',');
                //    Debug.Log(TAG + " partial result length " + results.Length);

                //    //when you set morethan 1 results index zero is always the closest to the words the you said
                //    //but it's not always the case so if you are not happy with index zero result you can always 
                //    //check the other index

                //    //sample on checking other results
                //    foreach (string possibleResults in results)
                //    {
                //        Debug.Log(TAG + "partial possibleResults " + possibleResults);
                //    }

                //    //sample showing the nearest result
                //    string whatToSay = results.GetValue(0).ToString();
                //    partialResultText.text = string.Format("Partial Result: {0}", whatToSay);
                //}
            }
        );
    }

    //SpeechRecognizer Events
}