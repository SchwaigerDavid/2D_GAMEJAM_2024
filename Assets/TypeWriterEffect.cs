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

    public int secondIMGPo = 7;
    public int secondIMGPlacement = 2; 

    private int pos = 0;

    private bool finished = false;
    public bool isJustText = false;
    private bool finish = false;
    public string nextScene = "Game"; 
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
            finish = false;
            ContinueStory();
        }
        else if (Input.GetMouseButtonUp(0)&&finished) {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }

    }
    public void ContinueStory() {
   
        if (displayLineCorutine !=null)
        {
            StopCoroutine(displayLineCorutine);
            dialogueText.text = text[pos - 1];
            finish = true;
            displayLineCorutine = null; 
        }
        Debug.Log(finish); 

        if (!finish)
        {
            if (!isJustText)
            {
                image.sprite = images[pos];
            }

            displayLineCorutine = StartCoroutine(DisplayLine(text[pos]));
            pos++;
            if (pos >= text.Length)
            {
                finished = true;

            }
        }

       
    }

    private IEnumerator DisplayLine(string line) {

        dialogueText.text = "";
        int counter = 0;
        
        foreach (char letter in line.ToCharArray()) {
            dialogueText.text += letter;
            if (pos == secondIMGPlacement &&counter == line.Length/2&&!isJustText)
            {
                image.sprite = images[secondIMGPo];
            }
            counter++;
            yield return new WaitForSeconds(typingSpeed); 
        }

    }

    public void Menu() {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        Debug.Log("Accessed");
        
    }
}
