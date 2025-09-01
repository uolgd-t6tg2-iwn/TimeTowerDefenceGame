using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TutorialTip firstTip;

    void Start()
    {
        
    }
    public void StartTutorial()
    {
        if (firstTip != null)
            firstTip.TriggerTip();
    }
}
