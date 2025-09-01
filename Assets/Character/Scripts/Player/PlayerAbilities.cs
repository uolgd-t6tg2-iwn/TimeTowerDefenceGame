using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private PlayerController playerController;

    [Header("Melee")]
    [SerializeField]
    private float baseMeleeDamage = 20f;
    private float meleeDamage;
    [SerializeField]
    private float meleeRange = 1.5f;
    [SerializeField]
    private float meleeRadius = 1.5f;
    [SerializeField]
    float meleeCooldown = 0.5f;
    float currentMeleeCooldown = 0f;
    [SerializeField]
    private GameObject meleeHitEffect;

    [Header("Ranged")]
    [SerializeField]
    private float baseRangedDamage = 10f;
    private float rangedDamage;
    [SerializeField]
    private float rangedRange = 0f;
    [SerializeField]
    private float rangedRadius = 25f;
    [SerializeField]
    private float damageRadius = 2f;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject projectileHitEffect;
    [SerializeField]
    float rangedCooldown = 5f;
    float currentRangedCooldown = 0f;
    

    [Header("Buff")]
    [SerializeField]
    private float buffDuration = 10f;
    [SerializeField]
    private float damageMultiplier = 2f;
    [SerializeField]
    private float speedMultiplier = 2f;
    [SerializeField]
    private GameObject buffEffect;
    [SerializeField]
    float buffCooldown = 20f;
    float currentBuffCooldown = 0f;
    private bool buffActive = false;
    private float buffEndTime = 0f;

    private AudioSource buffAudioSource;
    private AudioSource jogAudioSource;
    private AudioSource placeAudioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meleeDamage = baseMeleeDamage;
        rangedDamage = baseRangedDamage;
        
        buffAudioSource = GameObject.Find("BuffAudio").GetComponent<AudioSource>();
        // Debug.Log("Buff Audio Source Name = "+buffAudioSource.name);
    }
    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time < currentMeleeCooldown)
            {
                return;
            }
            else
            {
                currentMeleeCooldown = Time.time + meleeCooldown;
                Debug.Log("Melee Attack");

                //detects enemies in melee range of the player
                Vector3 origin = transform.position + transform.forward * meleeRange + Vector3.up * 1f;
                Collider closest = GetClosestEnemy(origin, meleeRadius);

                if (closest == null)
                {
                    Debug.Log("No enemies in melee range");
                    return;
                }
                else
                {
                    Debug.Log("Enemy Hit with Melee Attack");
                    //apply dmg to closest enemy
                    EnemyHealth enemy = closest.GetComponentInParent<EnemyHealth>();
                    enemy.TakeDamage(meleeDamage);


                    //create vfx hit effect
                    Vector3 spawnPos = closest.transform.position + Vector3.up * 0.5f;
                    Instantiate(meleeHitEffect, spawnPos, Quaternion.identity);

                }

            }
        }
    }

    //create gizmo to view attack range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 meleeAttackOrigin = transform.position + transform.forward * meleeRange + Vector3.up * 1f;
        Gizmos.DrawWireSphere(meleeAttackOrigin, meleeRadius);

        Gizmos.color = Color.blue;
        Vector3 rangedAttackOrigin = transform.position + transform.forward * rangedRange + Vector3.up * 1f;
        Gizmos.DrawWireSphere(rangedAttackOrigin, rangedRadius);
    }


    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time < currentRangedCooldown)
            {
                return;
            }
            else
            {
                //Debug.Log("Ranged Attack!");
                

                Vector3 origin = transform.position + transform.forward * rangedRange + Vector3.up * 1f;
                Collider closest = GetClosestEnemy(origin, rangedRadius);

                if (closest == null)
                {
                    //Debug.Log("No enemies in ranged attack range");
                    return;
                }
                else
                {
                    currentRangedCooldown = Time.time + rangedCooldown;
                    //Debug.Log("Enemy Hit with Ranged Attack");

                    Vector3 impactPos = closest.transform.position + Vector3.up * 0.5f;

                    //Aoe dmg at point of impact
                    Collider[] splashDamage = Physics.OverlapSphere(impactPos, damageRadius, enemyLayer, QueryTriggerInteraction.Collide);
                    for (int i = 0; i < splashDamage.Length; i++)
                    {
                        var enemyHealth = splashDamage[i].GetComponentInParent<EnemyHealth>();
                        enemyHealth.TakeDamage(rangedDamage);
                    }

                    //fire a visual projectile
                    GameObject projectileInstance = Instantiate(projectile, firePoint.position, Quaternion.identity);
                    var p = projectileInstance.GetComponent<Projectile>();
                    p.projectileSetup(firePoint.position, impactPos, projectileHitEffect);

                }
            }
        }
    }

    public void OnBuff(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time < currentBuffCooldown)
            {
                return;
            }
            else
            {
                //Debug.Log("Buff Activated!");
                currentBuffCooldown = Time.time + buffCooldown;

                buffActive = true;
                buffEndTime = Time.time + buffDuration;

                meleeDamage *= damageMultiplier;
                rangedDamage *= damageMultiplier;
                playerController.SetSpeedMultiplier(speedMultiplier);

                //create vfx buff effect
                Vector3 origin = transform.position + Vector3.up * 1.5f;
                Instantiate(buffEffect, origin, Quaternion.identity);

                //play buff sound
                buffAudioSource.Play();
            }
        }
    }

    private Collider GetClosestEnemy(Vector3 origin, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(origin, radius, enemyLayer, QueryTriggerInteraction.Collide);
        if (hits.Length == 0)
        {
            return null;
        }
        //straight line distance
        Collider closest = hits[0];
        float closestDist = (hits[0].transform.position - transform.position).magnitude;
        for (int i = 1; i < hits.Length; i++)
        {
            float currentDist = (hits[i].transform.position - transform.position).magnitude;
            if (currentDist < closestDist)
            {
                closestDist = currentDist;
                closest = hits[i];
            }
        }
        return closest;
    }

    void Update()
    {
        //ends buff after it runs out
        if (buffActive && Time.time >= buffEndTime)
        {
            buffActive = false;
            meleeDamage = baseMeleeDamage;
            rangedDamage = baseRangedDamage;
            playerController.ResetSpeed();
            //Debug.Log("Buff Ended!");
        }

    }
}
