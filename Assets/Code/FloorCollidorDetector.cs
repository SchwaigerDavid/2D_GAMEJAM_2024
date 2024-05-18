using UnityEngine;

public class FloorCollidorDetector : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        if (other.gameObject.CompareTag("Floor"))
        {
            PlantV2 plant = other.gameObject.GetComponentInParent<PlantV2>();
            plant.grounded = true;
            plant.doubleJump = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Left " + other.gameObject.name);
        if (other.gameObject.CompareTag("Floor"))
        {
            PlantV2 plant = other.gameObject.GetComponentInParent<PlantV2>();
            plant.grounded = false;
        }
    }
}
