using UnityEngine;

public class FaceManager : MonoBehaviour
{

    public static FaceManager Instance;

    public Sprite selectedFace;

    public Sprite[] faces = new Sprite[2];

    public GameObject faceObject;
    public int pos = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    public void Forward() {
        pos++;
        if (pos >= faces.Length) {

            pos = 0;

        }

        selectedFace = faces[pos];

        faceObject.GetComponent<SpriteRenderer>().sprite = selectedFace;

    }

    public void Back()
    {
        pos--;
        if (pos<0)
        {

            pos = faces.Length-1;

        }

        selectedFace = faces[pos];

        faceObject.GetComponent<SpriteRenderer>().sprite = selectedFace;



    }
}
