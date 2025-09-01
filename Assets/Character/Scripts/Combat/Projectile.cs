using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float travelTime = 0.15f;
    private Vector3 startPos;
    private Vector3 endPos;
    private GameObject impactVFX;
    private float timeTravelled = 0f;
    private bool created = false;


    public void projectileSetup(Vector3 start, Vector3 end, GameObject impactEffect)
    {
        startPos = start;
        endPos = end;
        impactVFX = impactEffect;
        transform.position = startPos;
        created = true;
    }

    private void Update()
    {
        if (created == true)
        {
            //how far we are
            timeTravelled += Time.deltaTime / travelTime;

            //clamp max to 1
            if (timeTravelled > 1f)
            {
                timeTravelled = 1f;
            }

            //move  and rotate projectile 
            Vector3 nextPos = Vector3.Lerp(startPos, endPos, timeTravelled);
            Vector3 dir = nextPos - transform.position;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.position = nextPos;
   
            //when end is reached
            if (timeTravelled == 1f)
            {
                //impact vfx
                Instantiate(impactVFX, endPos, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}
