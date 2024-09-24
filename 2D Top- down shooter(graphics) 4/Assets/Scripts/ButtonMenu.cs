using UnityEngine;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    //public GameObject[] hiddenButtons;

    //private int currentlyVisibleButtonIndex = -1;

    public AudioSource myFx;
    public AudioClip hoverFx;
    public AudioClip clickFx;

    public void HoverSound()
    {
        myFx.PlayOneShot(hoverFx);
    }

    public void ClickSound()
    {
        myFx.PlayOneShot(clickFx);
    }

    /*private void Start()
    {
        // �������� �� ���� �������� ��������
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Button visibleButton = child.GetComponent<Button>();

            if (visibleButton != null)
            {
                // ������ ���������� �������  
                visibleButton.onClick.AddListener(() => {
                    OnVisibleButtonPointerDown(child.GetSiblingIndex());
                });
            }
        }
    }*/


    /*public void OnVisibleButtonPointerDown(int buttonIndex)
    {
        if (currentlyVisibleButtonIndex != -1 && currentlyVisibleButtonIndex != buttonIndex)
        {
            hiddenButtons[currentlyVisibleButtonIndex].SetActive(false);
        }

        if (currentlyVisibleButtonIndex == buttonIndex)
        {
            hiddenButtons[buttonIndex].SetActive(false);
            currentlyVisibleButtonIndex = -1;
        }
        else
        {
            hiddenButtons[buttonIndex].SetActive(true);
            currentlyVisibleButtonIndex = buttonIndex;
        }
    }*/

    
}
