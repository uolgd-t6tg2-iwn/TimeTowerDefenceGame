using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSystem : MonoBehaviour
{
    // hardcoded config
    [SerializeField]
    private float startAmount = 50f;
    [SerializeField]
    private float gainPerSecond = 2f;

    [SerializeField] private TextMeshProUGUI resourceText;
    
    
    private float logInterval = 5f;
    private float current;
    private float nextLogTime;

    private void Start()
    {
        current = startAmount;

        nextLogTime = Time.time + logInterval;

        Debug.Log("Resource: " + current);
        resourceText.text = "Resources - " + current.ToString();
    }

    private void Update()
    {
        current = current + (gainPerSecond * Time.deltaTime);

        //temp code to not spam console
        if (Time.time >= nextLogTime)
        {
            Debug.Log("Resource: " + current);
            nextLogTime = Time.time + logInterval;
        }
            resourceText.text = "Resources - " + current.ToString("F0");
    }

    public bool TrySpend(float amount)
    {
        //returns true if you have enough money otherwise return false
        if (current >= amount)
        {
            current = current - amount;
            return true;
        }
        else
        {
            return false;
        }
    }

}
