using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestTime;
    public TextMeshProUGUI scoretext;
    //public TextMeshProUGUI boosttext;
    public static int score = 0;
    public float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        string realscore = score.ToString();
        //string boostcooldown = HoverTankController.BoostCooldownTime.ToString();
        if (score == 0)
        {
            realscore = "000";
        }
        scoretext.text = realscore;
        timerText.text = minutes + ":" + seconds;
        //boosttext.text = boostcooldown;
    }
}