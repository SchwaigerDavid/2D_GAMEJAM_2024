using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverSoundPlayer : MonoBehaviour, IPointerEnterHandler
{
    private Button button;
    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.playRandom("ui_hover");
    }

    public void OnButtonClick() {
        SoundManager.Instance.playRandom("ui_click");
    }
}
