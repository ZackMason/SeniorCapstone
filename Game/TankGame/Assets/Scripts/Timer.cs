using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    //public Image headhealth;
    //public Image turrethealth;
    public Image bodyhealth;

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

    public TimeSpan zone1;
    public TimeSpan zone2;
    public TimeSpan zone3;
    bool isFadingOut = false;
    public static int zone = 0;

    private int bossCount = 2;

    private bool _isDone = false;
    public void Finish() {
        bossCount -= 1;
        _isDone = bossCount == 0;
    }
    private Texture2D _combatModeTexture;
    private Texture2D _driveModeTexture;

    [SerializeField] private ModeIndicator _combatModeIndicator;
    [SerializeField] private ModeIndicator _driveModeIndicator;

    void Start()
    {
        SoundManager.Instance?.PlaySound(SoundAsset.theme1, Vector3.zero);
        timeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDone) {
            timeCounter += Time.deltaTime;
        }
        player = respawnManager.Player;
        var tankBody = player.GetComponent<HoverTankController>().GetBody();
        //int healthy = player.GetComponent<Health>().TankBody.GetCurrentHealth();
        if (tankBody != null) 
        {
            var tankHealth = tankBody.GetComponent<Health>().GetHealth();

            healthtext.text = tankHealth.ToString("f0");
            bodyhealth.fillAmount = 1.0f - (tankHealth) / tankBody.GetComponent<Health>().MaxHealth;
        }
        else
        {
            healthtext.text = "DEAD";
            bodyhealth.fillAmount = 1.0f;
        }
        //turrethealth.fillAmount = player.GetComponent<HoverTankController>().GetTurret().GetComponent<Health>().GetHealth() / 100.0f;
        //headhealth.fillAmount = player.GetComponent<HoverTankController>().GetHead().GetComponent<Health>().GetHealth() / 100.0f;
       

        

        boosty = player.GetComponent<HoverTankController>()._boostTimer;
        if (boosty > 0)
        {
            //BoostAudio.Play();
            CDT.text = boosty.ToString("f0");
            BCD.fillAmount = 0.2f * boosty;
        }
        else
        {
           // BoostAudio.Play();
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
            _combatModeIndicator.SetState(false);
            if (_driveModeIndicator.SetState(true)) {
                SoundManager.Instance?.PlaySound(SoundAsset.DriveMode, Vector3.zero);
            }
        }
        else
        {
            _driveModeIndicator.SetState(false);
            if (_combatModeIndicator.SetState(true)) {
                SoundManager.Instance?.PlaySound(SoundAsset.DriveMode, Vector3.zero);
            }
        }

        if (score == 0)
        {
            realscore = "000";
        }
        scoretext.text = realscore;
        timePlaying = TimeSpan.FromSeconds(timeCounter);
        string timeDisplayed = timePlaying.ToString("mm':'ss");
        timerText.text = timeDisplayed;
        if (zone == 1)
        {
            bestTime.text = "Zone 1: " + timeDisplayed;
            zone1 = timePlaying;
            score += 1000 - Convert.ToInt32(timePlaying.TotalSeconds * 3);
            bestTime.gameObject.SetActive(true);
            zone += 1;
        }
        else if (zone == 3)
        {
            bestTime.text = "Zone 2: " + timeDisplayed;
            zone2 = timePlaying;
            score += 1000 - Convert.ToInt32(timePlaying.TotalSeconds * 3) + Convert.ToInt32(zone1.TotalSeconds);
            bestTime.gameObject.SetActive(true);
            zone += 1;
        }
        else if (zone == 5)
        {
            bestTime.text = "Zone 3: " + timeDisplayed;
            zone2 = timePlaying;
            score += 1000 - Convert.ToInt32(timePlaying.TotalSeconds * 3) + Convert.ToInt32(zone2.TotalSeconds);
            bestTime.gameObject.SetActive(true);
            zone += 1;
        }
        else if (!isFadingOut && bestTime.gameObject.activeSelf)
        {
            StartCoroutine(FadeOutBestTimeText());
        }
    }

    IEnumerator FadeOutBestTimeText()
    {
        isFadingOut = true;

        float alpha = 1f;
        while (alpha > 0f)
        {
            // Reduce transparency gradually
            alpha -= Time.deltaTime / 3.0f;
            bestTime.alpha = alpha;

            yield return null;
        }

        bestTime.gameObject.SetActive(false);
        bestTime.alpha = 1f;

        isFadingOut = false;
    }

}