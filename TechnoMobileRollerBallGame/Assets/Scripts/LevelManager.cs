using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private static string TAG = "LevelManager";

    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    public Text timerText;
    float lvlDuration = 0;

    private float startTime;
    public float bronzeTime;
    public float silverTime;
    public float goldTime;
    private int numberStars = 0;

    private float startTime2 = 0;
    private const float timeBeforeStart = 2.0f;

    public Transform respawnPlayer;
    private GameObject player;

    public GameObject endingMenu;
    public Text bestTime;
    public Text bronzeTimeText;
    public Text silverTimeText;
    public Text goldTimeText;

    public GameObject tutoJoystickPanel;
    public GameObject tutoJoystickImage;

    public GameObject tutoBoostPanel;
    public GameObject tutoBoostImage;

    public GameObject tutoSwipePanel;
    public GameObject tutoSwipeLeftImage;
    public GameObject tutoSwipeRightImage;

    // Use this for initialization
    void Start () {
        instance = this;
        startTime2 = Time.time;

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = respawnPlayer.position;

        endingMenu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.time - startTime2 < timeBeforeStart)
        {
            startTimer();
            return;
        }

        if (GameManager.Instance.IsPlayed != 0)
        {
            lvlDuration = Time.time - startTime;

            string minutes = ((int)lvlDuration / 60).ToString("00");
            string secondes = (lvlDuration % 60).ToString("00.00");

            timerText.text = minutes + " : " + secondes;

            if (player.transform.position.y < -5.0f)
                OnDead();
        }
        else
        {
            if (Time.time - startTime2 > timeBeforeStart && Time.time - startTime2 < timeBeforeStart + 0.25f)
            {
                tutoJoystickPanel.SetActive(true);
                tutoJoystickImage.SetActive(true);
                tutoBoostPanel.SetActive(false);
                tutoBoostImage.SetActive(false);
                tutoSwipePanel.SetActive(false);
                tutoSwipeLeftImage.SetActive(false);
                tutoSwipeRightImage.SetActive(false);
                Debug.Log("ok");
            }
        }
    }

    public void OnWin()
    {
        foreach(Transform t in endingMenu.transform.parent)
        {
            t.gameObject.SetActive(false);
        }

        endingMenu.SetActive(true);

        if (lvlDuration < goldTime)
        {
            numberStars = 3;
            GameManager.Instance.Currency += 150;

            if (SceneManager.GetActiveScene().name == "1_Training" && ((GameManager.Instance.SuccessDone & 1 << 2) != 1 << 2))
                GameManager.Instance.SuccessDone += 1 << 2;
        }
        else if (silverTime > lvlDuration && lvlDuration > goldTime)
        {
            numberStars = 2;
            GameManager.Instance.Currency += 100;
        }
        else if (bronzeTime > lvlDuration && lvlDuration > silverTime)
        {
            numberStars = 1;
            GameManager.Instance.Currency += 50;
        }
        else
        {
            numberStars = 0;
            Debug.Log(TAG + " ---> OnWin : 0 stars.");
        }

        if (SceneManager.GetActiveScene().name == "1_Training" && ((GameManager.Instance.SuccessDone & 1 << 0) != 1 << 0) && ((GameManager.Instance.SuccessDone & 1 << 1) != 1 << 1))
        {
            GameManager.Instance.SuccessDone += 1 << 0;
            GameManager.Instance.SuccessDone += 1 << 1;
        }

        LevelData level = new LevelData(SceneManager.GetActiveScene().name);

        string saveAllDurations = "";
        saveAllDurations += (level.BestTime > lvlDuration || level.BestTime == 0.0f) ? lvlDuration : level.BestTime;
        saveAllDurations += "&";
        saveAllDurations += bronzeTime;
        saveAllDurations += "&";
        saveAllDurations += silverTime;
        saveAllDurations += "&";
        saveAllDurations += goldTime;
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_Duration", saveAllDurations);

        if (level.NumberStars > numberStars || level.NumberStars == 0)
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_Stars",  numberStars);

        GameManager.Instance.Save();

        string minutes = ((int)lvlDuration / 60).ToString("00");
        string secondes = (lvlDuration % 60).ToString("00.00");

        bestTime.text = minutes + " : " + secondes;

        minutes = ((int)bronzeTime / 60).ToString("00");
        secondes = (bronzeTime % 60).ToString("00.00");

        bronzeTimeText.text = minutes + " : " + secondes;

        minutes = ((int)silverTime / 60).ToString("00");
        secondes = (silverTime % 60).ToString("00.00");

        silverTimeText.text = minutes + " : " + secondes;

        minutes = ((int)goldTime / 60).ToString("00");
        secondes = (goldTime % 60).ToString("00.00");

        goldTimeText.text = minutes + " : " + secondes;

        Time.timeScale = 0;
    }

    public void OnDead()
    {
        player.transform.position = respawnPlayer.position;
        Rigidbody rigidBodyPlayer = player.GetComponent<Rigidbody>();
        rigidBodyPlayer.velocity = Vector3.zero;
        rigidBodyPlayer.angularVelocity = Vector3.zero;
    }

    public void displayTutoBoost()
    {
        tutoJoystickPanel.SetActive(false);
        tutoJoystickImage.SetActive(false);
        tutoBoostPanel.SetActive(true);
        tutoBoostImage.SetActive(true);
        tutoSwipePanel.SetActive(false);
        tutoSwipeLeftImage.SetActive(false);
        tutoSwipeRightImage.SetActive(false);
    }

    public void displayTutoSwipe()
    {
        tutoJoystickPanel.SetActive(false);
        tutoJoystickImage.SetActive(false);
        tutoBoostPanel.SetActive(false);
        tutoBoostImage.SetActive(false);
        tutoSwipePanel.SetActive(true);
        tutoSwipeLeftImage.SetActive(true);
        tutoSwipeRightImage.SetActive(true);
    }

    public void startGame()
    {
        tutoJoystickPanel.SetActive(false);
        tutoJoystickImage.SetActive(false);
        tutoBoostPanel.SetActive(false);
        tutoBoostImage.SetActive(false);
        tutoSwipePanel.SetActive(false);
        tutoSwipeLeftImage.SetActive(false);
        tutoSwipeRightImage.SetActive(false);

        GameManager.Instance.IsPlayed = 1;
        startTimer();
    }

    private void startTimer()
    {
        startTime = Time.time;
    }
}
