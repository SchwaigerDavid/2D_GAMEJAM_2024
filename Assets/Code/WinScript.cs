using UnityEngine;
using UnityEngine.SceneManagement; 

public class WinScript : MonoBehaviour
{
    private Camera camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
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

        AudioSource audio = camera.GetComponent<AudioSource>();
        audio.Stop();
        audio.loop = false;
        audio.clip = (AudioClip)Resources.Load("jingle_victory");
        audio.Play();
    }

    public void Menu() {

        SceneManager.LoadScene(0, LoadSceneMode.Single); 

    }
}
