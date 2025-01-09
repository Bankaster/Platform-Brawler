using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleMenu : MonoBehaviour
{
    public TextMeshProUGUI pressEnterText;
    public GameObject title;
    public GameObject nextMenu;

    public float scrollSpeed = 10f;
    public Vector2 offScreenPosition = new Vector2(250, -600);
    public Vector2 onScreenPosition = new Vector2(250, 300);

    private bool menuActive = false;
    private bool isAnimating = false;

    void Start()
    {
        title.transform.position = onScreenPosition;
        nextMenu.transform.position = offScreenPosition;
        nextMenu.SetActive(false);
    }

    void Update()
    {
        //If the Enter Key is pressed the Main Menu is shown
        if (Input.GetKeyDown(KeyCode.Return) && !isAnimating)
        {
            StartCoroutine(ShowMenu());
        }

        //If the Esc Key is pressed the Main Menu hides
        if (Input.GetKeyDown(KeyCode.Escape) && menuActive && !isAnimating)
        {
            StartCoroutine(HideMenu());
        }
    }

    IEnumerator ShowMenu()
    {
        isAnimating = true;
        menuActive = true;

        //Scrolls up the title off screen
        StartCoroutine(ScrollUp(title, onScreenPosition, offScreenPosition));

        //Enbales and shows the Main Menu from behind
        nextMenu.SetActive(true);
        yield return StartCoroutine(ScrollUp(nextMenu, offScreenPosition, onScreenPosition));

        //Ends the animation
        isAnimating = false;
    }

    IEnumerator HideMenu()
    {
        isAnimating = true;
        menuActive = false;

        //Hides the Main Menu behind the visible screen
        yield return StartCoroutine(ScrollDown(nextMenu, onScreenPosition, offScreenPosition));
        nextMenu.gameObject.SetActive(false);

        //Restores the Title and text form above
        StartCoroutine(ScrollDown(title, offScreenPosition, onScreenPosition));

        //Ends the animation
        isAnimating = false;
    }

    IEnumerator ScrollUp(GameObject element, Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0f;
        Vector3 currentPosition = startPosition;

        // Asegúrate de que el objeto está activo
        element.SetActive(true);

        while (currentPosition.y < endPosition.y)
        {
            elapsedTime += Time.deltaTime;
            currentPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime * scrollSpeed / Mathf.Abs(endPosition.y - startPosition.y));
            element.transform.position = currentPosition;
            yield return null;
        }

        element.transform.position = endPosition;
    }

    IEnumerator ScrollDown(GameObject element, Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0f;
        Vector3 currentPosition = startPosition;

        while (currentPosition.y > endPosition.y)
        {
            elapsedTime += Time.deltaTime;
            currentPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime * scrollSpeed / Mathf.Abs(startPosition.y - endPosition.y));
            element.transform.position = currentPosition;
            yield return null;
        }

        element.transform.position = endPosition;
    }
}
