using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseController : MonoBehaviour
{
    [SerializeField]
    private TimeMachineHealth timeMachine;
    private bool ended = false;


   //Determines win lose state based on health of time machine and restarts scene
    private void Update()
    {
        if (ended)
        {
            Restart();
            return;
        }

        float hp = timeMachine.GetHealthPercent();

        if (hp >= 1f)
        {
            ended = true;
            Debug.Log("WIN: Time machine fully repaired. Press R to restart");
        }
        else if (hp <= 0f)
        {
            ended = true;
            Debug.Log("LOSE: Time machine destroyed. Press R to restart");
        }

    }

    void Restart() //restarts scene
    {
        var s = SceneManager.GetActiveScene();
        SceneManager.LoadScene(s.buildIndex);
    }

   
}
