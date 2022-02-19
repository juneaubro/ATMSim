using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTextClear : MonoBehaviour {
    public Text text;
    private void Update() {
        text = GameObject.Find("txtDebug").GetComponent<Text>();
        string[] str = text.text.Split('\n');
        print(str.Length);
        if (str.Length > 40) {
            text.text = "Debugging: ";
        }
    }
}
