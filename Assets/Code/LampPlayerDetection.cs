using UnityEngine;
using System;

public class LampPlayerDetection : MonoBehaviour
{
    private float healingRange = 1.5f;
    
    private float sectorAngle = -0.7854f;
    private float addAngle = -0.2618f;
    

    
    private Vector2[] vectors = new Vector2[7];

    void Start()
    {
        for (int i = 0; i < vectors.Length; i++) {
            vectors[i] = calculateUnitVector(sectorAngle);
            sectorAngle += addAngle;
        }
    
    }

    void Update()
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            Debug.DrawRay(transform.position, vectors[i]*healingRange, Color.red);
        }


        searchForPlayer();
        
    }

    Vector2 calculateUnitVector(float angle) { 
        return new Vector2(
            (float) Mathf.Cos(angle),   
            (float) Mathf.Sin(angle) 
        );
    }

        bool searchForPlayer()
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, vectors[i], healingRange);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                //Debug.Log("Player detected");
                hit.collider.gameObject.GetComponent<PlantV2>().heal(1);
                return true;
            }
        }
        return false;
    }
}
