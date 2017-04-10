using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    private int currentSkinIndex = 0;
    private int currency = 0;
    private int skinAvailability = 1;
    private int successDone = 0;
    private int isPlayed = 0;

    #region GETTERS_SETTERS

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public int CurrentSkinIndex
    {
        get
        {
            return currentSkinIndex;
        }
        set
        {
            currentSkinIndex = value;
        }
    }

    public int Currency
    {
        get
        {
            return currency;
        }
        set
        {
            currency = value;
        }
    }

    public int SkinAvailability
    {
        get
        {
            return skinAvailability;
        }
        set
        {
            skinAvailability = value;
        }
    }

    public int SuccessDone
    {
        get
        {
            return successDone;
        }
        set
        {
            successDone = value;
        }
    }

    public int IsPlayed
    {
        get
        {
            return isPlayed;
        }

        set
        {
            isPlayed = value;
        }
    }
    #endregion

    // Use this for initialization
    private void Awake () {
        instance = this;

        DontDestroyOnLoad(gameObject);

        if (PlayerPrefs.HasKey("CurrentSkin") && PlayerPrefs.HasKey("Currency") && PlayerPrefs.HasKey("SkinAvailability") && PlayerPrefs.HasKey("SuccessDone") && PlayerPrefs.HasKey("IsPlayed"))
        {
            //We had previous session

            currentSkinIndex = PlayerPrefs.GetInt("CurrentSkin");
            currency = PlayerPrefs.GetInt("Currency");
            skinAvailability = PlayerPrefs.GetInt("SkinAvailability");
            successDone = PlayerPrefs.GetInt("SuccessDone");
            isPlayed = PlayerPrefs.GetInt("IsPlayed");
        }
        else
        {
            Save();
        }

	}
	
	// Use this for save informations to Preferences
    public void Save()
    {
        PlayerPrefs.SetInt("CurrentSkin", currentSkinIndex);
        PlayerPrefs.SetInt("Currency", currency);
        PlayerPrefs.SetInt("SkinAvailability", skinAvailability);
        PlayerPrefs.SetInt("SuccessDone", successDone);
        PlayerPrefs.SetInt("IsPlayed", isPlayed);
    }
}
