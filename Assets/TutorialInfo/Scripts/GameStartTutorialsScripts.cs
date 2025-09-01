using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialTip : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI uiText;
    [TextArea] public string message;
    public float displayDuration = 5f; 
    public bool autoHide = true;      
    public KeyCode[] triggerKeys;     
    public TutorialTip nextTip;     

    private bool inputReady = false;

    public void TriggerTip()
    {
        ShowTip();

        if (autoHide)
        {
            StartCoroutine(HideAfterDelay());
        }
        else
        {
            StartCoroutine(EnableInputNextFrame());
        }
    }

    void ShowTip()
    {
        if (uiText != null)
        {
            uiText.text = message;
            uiText.enabled = true;
        }
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        HideTip();
    }

    void HideTip()
    {
        if (uiText != null)
            uiText.enabled = false;

        if (nextTip != null)
            nextTip.TriggerTip();
    }

    IEnumerator EnableInputNextFrame()
    {
        yield return null;
        inputReady = true;
    }

    void Update()
    {
        if (inputReady && triggerKeys.Length > 0)
        {
            foreach (KeyCode key in triggerKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    inputReady = false;
                    HideTip();
                }
            }
        }
    }
}
