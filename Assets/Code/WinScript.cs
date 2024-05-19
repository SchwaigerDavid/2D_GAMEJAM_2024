using UnityEngine;

public class WinScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        var player = collision.gameObject;
        if ((player == null))
        {
            return;
        }

        PlantV2 script = player.GetComponent<PlantV2>();
        if(script == null)
        {
            return;
        }

        var WIN = script.WIN;
        WIN.SetActive(true);
        WIN.transform.Find("WinText").GetComponent<TypeWriterEffect>().ContinueStory();
    }
}
