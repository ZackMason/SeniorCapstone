using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public RawImage needle;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestTime;
    public TextMeshProUGUI scoretext;
    public GameObject player;
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

        

        float velocityMagnitude = 180.0f - player.GetComponent<Rigidbody>().velocity.magnitude * 10.0f;
        Vector3 needlerot = needle.rectTransform.rotation.eulerAngles;
        needlerot.z = velocityMagnitude;
        needle.rectTransform.rotation = Quaternion.Euler(needlerot);

        if (score == 0)
        {
            realscore = "000";
        }
        scoretext.text = realscore;
        timerText.text = minutes + ":" + seconds;
        //boosttext.text = boostcooldown;
    }
}