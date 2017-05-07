using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFunction : MonoBehaviour
{
    private String text = String.Empty;
    public Text functionText;

	// Use this for initialization
	void Start () {

    }

    public void AddFunction(string function)
    {
        if(text.Length>0) text += "\n";
        text += function;
        functionText.text = text;
    }

    public void Clear()
    {
        text = String.Empty;
        functionText.text = text;
    }
}
