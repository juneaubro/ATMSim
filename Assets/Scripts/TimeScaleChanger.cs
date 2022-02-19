using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleChanger : MonoBehaviour {
    public Text text;
    public Slider slider;
    private float fixedDeltaTime;
    private void Awake() {
        text = GameObject.Find("TimeScale Text").GetComponent<Text>();
        slider = GameObject.Find("TimeScale Bar").GetComponent<Slider>();
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }
    private void Update() {
        text.text = $"{slider.value}x speed ({slider.value*60}frames/second)";

        Time.timeScale = slider.value;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }
}
