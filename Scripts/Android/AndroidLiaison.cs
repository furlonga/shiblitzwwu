using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidLiaison : MonoBehaviour
{
    Text textBoxText;
    AndroidJavaObject intent;
    bool hasExtra;
    AndroidJavaObject extras;
    string arguments;

    // Start is called before the first frame update
    void Start()
    {
        textBoxText = gameObject.GetComponent<Text>();
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        if(currentActivity != null)
            intent = currentActivity.Call<AndroidJavaObject>("getIntent");
        if(intent != null)
            hasExtra = intent.Call<bool>("hasExtra", "arguments");
        Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {
        if(hasExtra)
        {
            Debug.Log("has extra!");
            extras = intent.Call<AndroidJavaObject>("getExtras");
            arguments = extras.Call<string>("getString", "arguments");
            textBoxText.text = arguments;
            Debug.Log(arguments);
        }
        else
        {
            textBoxText.text = "No Extras came in from android";
            Debug.Log("does not have extra OTL");
        }
    }
}
