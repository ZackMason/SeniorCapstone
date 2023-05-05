using System;
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
    public TextMeshProUGUI CDT;
    public TextMeshProUGUI FCDT;

    // fire and boost cooldown?
    public Image BCD; 
    public Image FCD;

    public AudioSource DriveAudio;
    public AudioSource BoostAudio;

    public RawImage CM;
    public RawImage DM;

    public RespawnManager respawnManager;
    public GameObject player;
    public static int score = 0;
    public float boosty;
    public float fire;
    public float timeCounter;
    private TimeSpan timePlaying;

    private Texture2D _combatModeTexture;
    private Texture2D _driveModeTexture;

    void Start()
    {
        _driveModeTexture = LoadTextureFromFile("drive mode a.png");
        _combatModeTexture = LoadTextureFromFile("combat mode.png");
        timeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
        player = respawnManager.Player;
        //int healthy = player.GetComponent<Health>().GetCurrentHealth();
        //healthtext.text = healthy.ToString();

        boosty = player.GetComponent<HoverTankController>()._boostTimer;
        if (boosty > 0)
        {
            //BoostAudio.Play();
            CDT.text = boosty.ToString("f0");
            BCD.fillAmount = 0.2f * boosty;
        }
        else
        {
            CDT.text = " ";
            BCD.fillAmount = 0.0f;
        }

        fire = player.GetComponentInChildren<HEWeapon>()._fireTime;
        //healthtext.text = fire.ToString();
        if (fire > 0)
        {
            FCDT.text = fire.ToString("f1");
            FCD.fillAmount = fire;
        }
        else
        {
            FCDT.text = " ";
            FCD.fillAmount = 0.0f;
        }

        // string minutes = ((int)t / 60).ToString();
        // string seconds = (t % 60).ToString("f2");
        string realscore = score.ToString();
        //string boostcooldown = HoverTankController.BoostCooldownTime.ToString();


        float velocityMagnitude = 180.0f - player.GetComponent<Rigidbody>().velocity.magnitude * 10.0f;
        Vector3 needlerot = needle.rectTransform.rotation.eulerAngles;
        needlerot.z = velocityMagnitude;
        needle.rectTransform.rotation = Quaternion.Euler(needlerot);
        DriveAudio.volume = player.GetComponent<Rigidbody>().velocity.magnitude * 0.1f;
        //DriveAudio.Play();


        
        if (player.GetComponent<HoverTankController>().Mode() == TankMode.DRIVE)
        {
            
            Texture2D newTexture = _driveModeTexture;
            Texture2D cTexture = _combatModeTexture;
            if (DM.texture != newTexture)
            {
                SoundManager.Instance?.PlaySound(SoundAsset.DriveMode, Vector3.zero);
            }
            DM.texture = newTexture;
            CM.texture = cTexture;
        }
        else
        {
            Texture2D newTexture = _driveModeTexture;
            Texture2D cTexture = _combatModeTexture;
            if (DM.texture != newTexture)
            {
                SoundManager.Instance?.PlaySound(SoundAsset.CombatMode, Vector3.zero);
            }
            DM.texture = newTexture;
            CM.texture = cTexture;
        }

        if (score == 0)
        {
            realscore = "000";
        }
        scoretext.text = realscore;
        timePlaying = TimeSpan.FromSeconds(timeCounter);
        string timeDisplayed = timePlaying.ToString("mm':'ss");
        timerText.text = timeDisplayed;
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