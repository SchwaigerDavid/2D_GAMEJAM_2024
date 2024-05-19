using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Credits : MonoBehaviour
{
    public Animator antimator;
    public Animator legendanimator;

    private bool hasbeenpushed = false;
    public void OpenCredits() {
        if (hasbeenpushed) {
            legendanimator.SetBool("Legend", true);
      }

        hasbeenpushed = true;
        antimator.SetBool("Credits",true);
    }
    public void Quit()
    {
        Application.Quit();
    }
   
    public void Play() {
        StartCoroutine(GameObject.FindAnyObjectByType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.Out, "CutScene1"));
    }



}
