using UnityEngine;

public class SmokePlayer : MonoBehaviour
{
    private Beetle beetle;
    void Start()
    {
        beetle = GetComponentInParent<Beetle>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            beetle.attack(player);
        }
    }
}
