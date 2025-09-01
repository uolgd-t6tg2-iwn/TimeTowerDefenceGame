using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class UIBehaviour : MonoBehaviour
{
    public Animator mainMenuController;
    public Animator settingsMenuController;

    public AudioMixer audioMixer;

    public EventSystem eventSystem;

    public GameObject[] gameObjectsToActivateOnGameStart;

    public GameObject playerCharacterAndCamera;

    public GameObject menus;
    // public Animator avSettingsMenuController;
    // public Animator displaySettingsMenuController;

    public Text gameStateButtonText;

    private bool gameStarted;
    private bool gamePaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameStarted = false;
        gamePaused = true;

        mainMenuController.SetBool("mainMenuIsOffscreen", false);
        settingsMenuController.SetBool("settingsMenuIsOffscreen", true);
        settingsMenuController.SetBool("settingsMenuSlideToDisplay", false);
    }

    private void Update()
    {
        //throw new NotImplementedException();
        // If escape is pressed while game not paused, open menu
        if (Input.GetKeyDown("escape"))
        {
            if (gameStarted & !gamePaused & !menus.activeSelf)
            {
                Debug.Log("Pausing game");
                PauseGame();
            }
            // else if (gameStarted & gamePaused & mainMenuController.GetBool("mainMenuIsOffscreen") &
            //          (settingsMenuController.GetBool("settingsMenuIsOffscreen") == false))
            // {
            //     Debug.Log("Going back to main menu");
            //     GoToMainMenu();
            //     OpenMainMenu();
            //     GoToMainMenu();
            // }
            // else if (gameStarted & gamePaused & (mainMenuController.GetBool("mainMenuIsOffscreen") == false) &
            //          settingsMenuController.GetBool("settingsMenuIsOffscreen"))
            // {
            //     Debug.Log("Continuing game");
            //     ContinueGame();
            // }
        }
    }

    public void OpenMainMenu()
    {
        menus.SetActive(true);
        GoToMainMenu();
    }

    public void CloseMainMenu()
    {
        GoToMainMenu();
        menus.SetActive(false);
    }

    public void GoToMainMenu()
    {
        eventSystem.SetSelectedGameObject(GameObject.Find("GameStateButton"));
        mainMenuController.SetBool("mainMenuIsOffscreen", false);
        settingsMenuController.SetBool("settingsMenuIsOffscreen", true);
        settingsMenuController.SetBool("settingsMenuSlideToDisplay", false);
        Debug.Log("Sliding to main menu");
    }

    public void GoToSettingsMenu()
    {
        eventSystem.SetSelectedGameObject(GameObject.Find("AudioSettingsButton"));
        mainMenuController.SetBool("mainMenuIsOffscreen", true);
        settingsMenuController.SetBool("settingsMenuIsOffscreen", false);
        settingsMenuController.SetBool("settingsMenuSlideToDisplay", false);
        Debug.Log("Sliding to settings menu");
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            gamePaused = false;

            // Set inactive objects active
            for (int idx = 0; idx < gameObjectsToActivateOnGameStart.Length; idx++)
            {
                gameObjectsToActivateOnGameStart[idx].SetActive(true);
            }

            playerCharacterAndCamera.SetActive(true);

            // Update text in 'Game State Button' for 'pause' functionality
            gameStateButtonText.text = "Continue Game";

            CloseMainMenu();
        }
    }

    // Pause game
    public void PauseGame()
    {
        if (gameStarted & !gamePaused)
        {
            Debug.Log("Pausing the game");
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            gamePaused = true;
            OpenMainMenu();
            // GoToMainMenu();
        }
    }

    // Continue game
    public void ContinueGame()
    {
        if (gameStarted & gamePaused)
        {
            Debug.Log("Continuing the game");
            gamePaused = false;
            // GoToMainMenu();
            CloseMainMenu();
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void QuitGame()
    {
        // Quit game, hopefully gracefully
        Application.Quit();
    }

    public void SettingsMenuSlideToDisplay()
    {
        mainMenuController.SetBool("mainMenuIsOffscreen", true);
        settingsMenuController.SetBool("settingsMenuIsOffscreen", false);
        settingsMenuController.SetBool("settingsMenuSlideToDisplay", true);
        Debug.Log("Sliding to display");
        Debug.Log("mainMenuIsOffscreen=" + mainMenuController.GetBool("mainMenuIsOffscreen"));
        Debug.Log("settingsMenuIsOffscreen=" + settingsMenuController.GetBool("settingsMenuIsOffscreen"));
        Debug.Log("settingsMenuSlideToDisplay=" + settingsMenuController.GetBool("settingsMenuSlideToDisplay"));
    }

    public void SettingsMenuSlideToAudio()
    {
        mainMenuController.SetBool("mainMenuIsOffscreen", true);
        settingsMenuController.SetBool("settingsMenuIsOffscreen", false);
        settingsMenuController.SetBool("settingsMenuSlideToDisplay", false);
        Debug.Log("Sliding to audio");
    }

    public void ChangeMasterVolume(float value)
    {
        audioMixer.SetFloat("mxrMasterVolume", SliderValueToVolume(value));
        Debug.Log("Master volume changed to " + value + "i.e." + SliderValueToVolume(value) + "db");
    }

    public void ChangeSFXVolume(float value)
    {
        audioMixer.SetFloat("mxrSFXVolume", SliderValueToVolume(value));
        Debug.Log("SFX volume changed to " + value + "i.e." + SliderValueToVolume(value) + "db");
    }

    public void ChangeSoundtrackVolume(float value)
    {
        audioMixer.SetFloat("mxrSoundtrackVolume", SliderValueToVolume(value));
        Debug.Log("Soundtrack volume changed to " + value + "i.e." + SliderValueToVolume(value) + "db");
    }

    public void ChangeSpeechVolume(float value)
    {
        audioMixer.SetFloat("mxrSpeechVolume", SliderValueToVolume(value));
        Debug.Log("Speech volume changed to " + value + "i.e." + SliderValueToVolume(value) + "db");
    }

    private float SliderValueToVolume(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 1e-6f, 1f)) * 13f; // Assumes max of 0dB
    }
}