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
    public TextMeshProUGUI healthtext;

    public RawImage CM;

    public RawImage DM;

    public RespawnManager respawnManager;
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
        player = respawnManager.Player;
        //int healthy = player.GetComponent<Health>().GetCurrentHealth();
        //healthtext.text = healthy.ToString();

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        string realscore = score.ToString();
        //string boostcooldown = HoverTankController.BoostCooldownTime.ToString();


        float velocityMagnitude = 180.0f - player.GetComponent<Rigidbody>().velocity.magnitude * 10.0f;
        Vector3 needlerot = needle.rectTransform.rotation.eulerAngles;
        needlerot.z = velocityMagnitude;
        needle.rectTransform.rotation = Quaternion.Euler(needlerot);

        Vector3 none = new Vector3(0, 0, 0);
        
        if (player.GetComponent<HoverTankController>().Mode() == TankMode.DRIVE)
        {
            SoundManager.Instance?.PlaySound(SoundAsset.DriveMode, none);
            Texture2D newTexture = LoadTextureFromFile("drive mode a.png");
            Texture2D cTexture = LoadTextureFromFile("combat mode.png");
            DM.texture = newTexture;
            CM.texture = cTexture;
        }
        else
        {
            SoundManager.Instance?.PlaySound(SoundAsset.CombatMode, none);
            Texture2D newTexture = LoadTextureFromFile("drive mode.png");
            Texture2D cTexture = LoadTextureFromFile("combat mode a.png");
            DM.texture = newTexture;
            CM.texture = cTexture;
        }

        if (score == 0)
        {
            realscore = "000";
        }
        scoretext.text = realscore;
        timerText.text = minutes + ":" + seconds;
        //boosttext.text = boostcooldown;
    }

    Texture2D LoadTextureFromFile(string filePath)
    {
        Texture2D texture = new Texture2D(2, 2);
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
        texture.LoadImage(bytes);
        return texture;
    }
}