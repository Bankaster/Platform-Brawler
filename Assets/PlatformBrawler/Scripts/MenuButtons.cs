using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite oldSprite;
    public Image targetSprite;
    public Sprite newSprite;

    private void Start()
    {
        if (targetSprite != null)
        {
            oldSprite = targetSprite.sprite;
        }
    }

    //Change the image when the mouse is on it
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetSprite != null && newSprite != null)
        {
            targetSprite.sprite = newSprite;
        }
    }

    //Restore the Image to his original sprite
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetSprite != null && oldSprite != null)
        {
            targetSprite.sprite = oldSprite;
        }
    }

    //Close the Game
    public void ExitButton()
    {
        Application.Quit();
    }

    //Load the Main Menu Scene
    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
