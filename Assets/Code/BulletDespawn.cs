using UnityEngine;

public class BulletDespawn : MonoBehaviour
{
    private double timeToDespawn = 3.0;
    private double timeLeft;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLeft = timeToDespawn;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0)
        {
            Destroy(gameObject);
        }
        timeLeft -= Time.deltaTime;
    }
}
