using UnityEngine;

public class TimeMachineHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private float regenPerSecond = 2f;
    [SerializeField]
    [Range(0f, 1f)]
    private float startPercent = 0.25f; //% of max health time machine starts at
    [SerializeField]
    [Range(0f, 1f)]
    private float repairedModelPercent = 0.5f; //% of max health time machine changes to repaired model at
    [SerializeField]
    private GameObject damagedModel;
    [SerializeField]
    private GameObject repairedModel;
    [SerializeField]
    private GameObject damageVfxPrefab;
    private GameObject activeDamageVfx;

    private float currentHealth;

    private bool isFixed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentHealth = maxHealth * startPercent;

        isFixed = false;
        damagedModel.SetActive(true);
        repairedModel.SetActive(false);

    }

    private void Update()
    {
        //regen time machine health over time
        if (currentHealth < maxHealth)
        {
            currentHealth = currentHealth + regenPerSecond * Time.deltaTime;

        }

        if (isFixed == false)
        {
            //changes time machine health when health is over 
            if (currentHealth >= (maxHealth * repairedModelPercent))
            {
                isFixed = true;

                damagedModel.SetActive(false);
                repairedModel.SetActive(true);

            }
        }
    }
    
    public float GetHealthPercent()
{
    return currentHealth / maxHealth;
}

    public void TakeDamage(float amount)
    {
        currentHealth = currentHealth - amount;

        //lowest health possible is 0
        if (currentHealth < 0f)
        {
            currentHealth = 0f;
        }

        if (isFixed == true)
        {
            if (currentHealth < (maxHealth * repairedModelPercent))
            {
                isFixed = false;

                damagedModel.SetActive(true);
                repairedModel.SetActive(false);
            }
        }

        //vfx plays when time machine takes damage
        if (activeDamageVfx == null)
        {
            activeDamageVfx = Instantiate(damageVfxPrefab, transform.position, Quaternion.identity, transform);
        }
    }
}
