using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class TypeWriterEffect : MonoBehaviour
{

    public string[] text;

    public TextMeshProUGUI dialogueText;
    public float typingSpeed = .1f;

    private int pos = 0;

    private bool finished = false;

    public Sprite[] images;
    public Image image;
    public float FadeRate = 0.2f; 


    private Coroutine displayLineCorutine; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ContinueStory();

    }

    private void Update()
    {

        if (Input.GetMouseButtonUp(0) && !finished)
        {
            Debug.Log("Down");
            ContinueStory();
        }
        else if (Input.GetMouseButtonUp(0)&&finished) {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

    }
    public void ContinueStory() {
   
        if (displayLineCorutine !=null)
        {
            StopCoroutine(displayLineCorutine);
        }
        
    
        image.sprite = images[pos];
        displayLineCorutine = StartCoroutine(DisplayLine(text[pos]));
        pos++;
        if (pos >= text.Length) {
            finished = true; 

        }
    }

    private IEnumerator DisplayLine(string line) {

        dialogueText.text = "";

        foreach (char letter in line.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed); 
        }

    }

    public void Menu() {

        
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        
    }
}
