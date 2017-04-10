using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour {

    private const float m_cameraTransitionSpeed = 3.0f;
    private static string TAG = "MainMenu";

    public GameObject levelButtonPrefab;
    public GameObject levelContainerButton;
    public GameObject skinButtonPrefab;
    public GameObject skinContainerButton;
    public GameObject successButtonPrefab;
    public GameObject successContainerButton;

    public Text currencyText;

    public Material playerMaterial;

    private Transform cameraTransform;
    private Transform cameraDesiredLookAt;

    private bool nextLevelLocked = false;

    private int[] costTab =    { 0, 150, 150, 150,
                                300, 300, 300, 300,
                                1000, 1000, 1000, 1000,
                                2000, 2000, 2500, 3000 };

	// Use this for initialization
	void Start () {
        changePlayerSkin(GameManager.Instance.CurrentSkinIndex);
        currencyText.text = "Currency : " + GameManager.Instance.Currency.ToString();
        cameraTransform = Camera.main.transform;

        Sprite[] thumbnails = Resources.LoadAll<Sprite>("LevelIcons");
        foreach(Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;
            container.transform.SetParent(levelContainerButton.transform, false);

            string sceneName = thumbnail.name;

            LevelData level = new LevelData(sceneName);

            string minutes = ((int)level.BestTime / 60).ToString("00");
            string secondes = (level.BestTime % 60).ToString("00.00");

            container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (level.BestTime != 0.0f) ? minutes + " : " + secondes : "00 : 00";
            container.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = level.NumberStars.ToString();

            container.transform.GetChild(1).gameObject.SetActive(nextLevelLocked);
            container.GetComponent<Button>().interactable = !nextLevelLocked;

            if (level.BestTime == 0.0f)
                nextLevelLocked = true;

            container.GetComponent<Button>().onClick.AddListener(() => loadLevel(sceneName));
        }

        int skinIndex = 0;
        Sprite[] skins = Resources.LoadAll<Sprite>("SkinsPlayer");
        foreach (Sprite skin in skins)
        {
            GameObject container = Instantiate(skinButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = skin;
            container.transform.SetParent(skinContainerButton.transform, false);

            int index = skinIndex;
            container.GetComponent<Button>().onClick.AddListener(() => changePlayerSkin(index));
            container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = costTab[index].ToString();

            if ((GameManager.Instance.SkinAvailability & 1 << index) == 1 << index)
            {
                container.transform.GetChild(0).gameObject.SetActive(false);
            }

            skinIndex++;
        }

        int successIndex = 0;
        Sprite[] successTab = Resources.LoadAll<Sprite>("SuccessIcons");
        foreach (Sprite success in successTab)
        {
            GameObject container = Instantiate(successButtonPrefab) as GameObject;
            container.transform.GetChild(0).GetComponent<Image>().sprite = success;
            container.transform.SetParent(successContainerButton.transform, false);

            int index = successIndex;

            if ((GameManager.Instance.SuccessDone & 1 << index) == 1 << index)
            {
                container.transform.GetChild(1).gameObject.SetActive(true);
            }

            successIndex++;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(cameraDesiredLookAt != null)
        {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesiredLookAt.rotation, m_cameraTransitionSpeed * Time.deltaTime);
        }
		
	}

    private void loadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void lookAtMenu(Transform menuTransform)
    {
        cameraDesiredLookAt = menuTransform;
    }

    private void changePlayerSkin(int index)
    {
        if ((GameManager.Instance.SkinAvailability & 1 << index) == 1 << index)
        {
            float x = (index % 4) * 0.25f;
            float y = ((int)index / 4) * 0.25f;

            if (y == 0.0f)
                y = 0.75f;
            else if (y == 0.75f)
                y = 0.0f;
            else if (y == 0.75f)
                y = 0.0f;
            else if (y == 0.75f)
                y = 0.0f;

            playerMaterial.SetTextureOffset("_MainTex", new Vector2(x, y));
            GameManager.Instance.CurrentSkinIndex = index;
            GameManager.Instance.Save();
        }
        else
        {
            // Skin not available
            int cost = costTab[index];

            if (GameManager.Instance.Currency >= cost)
            {
                GameManager.Instance.Currency -= cost;
                GameManager.Instance.SkinAvailability += 1 << index;
                GameManager.Instance.Save();

                currencyText.text = "Currency : " + GameManager.Instance.Currency.ToString();

                skinContainerButton.transform.GetChild(index).GetChild(0).gameObject.SetActive(false);
                changePlayerSkin(index);
            }
        }
    }

    public void deletePlayerPref()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}

public class LevelData
{

    public LevelData(string levelName)
    {
        string dataDuration = PlayerPrefs.GetString(levelName + "_Duration");
        int dataStars = PlayerPrefs.GetInt(levelName + "_Stars");

        if (dataDuration == "")
            return;

        string[] allDataDurations = dataDuration.Split('&');
        BestTime = float.Parse(allDataDurations[0]);
        BronzeTime = float.Parse(allDataDurations[1]);
        SilverTime = float.Parse(allDataDurations[2]);
        GoldTime = float.Parse(allDataDurations[3]);

        if (dataStars == 0)
            return;

        NumberStars = dataStars;
    }

    public float BestTime { get; set; }
    public float BronzeTime { get; set; }
    public float SilverTime { get; set; }
    public float GoldTime { get; set; }
    public int NumberStars { get; set; }

}