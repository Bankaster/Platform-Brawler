using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnlineManager : MonoBehaviour
{
    static public OnlineManager instance;

    [SerializeField] Serialize serialize;

    [SerializeField] BrawlerController Player1Controller;
    [SerializeField] BrawlerController Player2Controller;
    [SerializeField] RemotePlayerController Player1RemoteController;
    [SerializeField] RemotePlayerController Player2RemoteController;

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public GameObject[] players;
    public float gameDuration = 20f;
    private float countdown = 3f;

    public GameObject resultPanel;
    public TextMeshProUGUI winnerText;

    public float blueDeathCount = 0;
    public float redDeathCount = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Start Countdown
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        //Show Countdown in Screen
        while (countdown > 0)
        {
            countdownText.text = Mathf.CeilToInt(countdown).ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "Start!";
        yield return new WaitForSeconds(1f);


        //Hide Countdown and enables player movement control scripts
        countdownText.gameObject.SetActive(false);

        if (GameObject.FindGameObjectWithTag("Server"))
        {
            Player1Controller.enabled = true;
            Player2RemoteController.enabled = true;
            serialize.CharController = Player1Controller.gameObject;
        }
        else
        {
            Player2Controller.enabled = true;
            Player1RemoteController.enabled = true;
            serialize.CharController = Player2Controller.gameObject;
        }

        //Start Timer
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        float timeLeft = gameDuration;

        while (timeLeft > 0)
        {
            timerText.text = Mathf.CeilToInt(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        CheckWinner();

        //Method that ends the game
        EndGame();
    }

    //Disables player movement control scripts
    private void EndGame()
    {
        countdownText.text = "Finish!";
        countdownText.gameObject.SetActive(true);

        Player1Controller.enabled = false;
        Player2Controller.enabled = false;
        Player1RemoteController.enabled = false;
        Player2RemoteController.enabled = false;
    }

    //Compares players death counts to decide the winner
    private void CheckWinner()
    {
        if (redDeathCount > blueDeathCount)
        {
            ShowWinner("Player1");
        }
        else if (blueDeathCount > redDeathCount)
        {
            ShowWinner("Player2");
        }
        else if (blueDeathCount == redDeathCount)
        {
            ShowWinner("Draw");
        }
    }

    //Show the winner on screen
    public void ShowWinner(string winner)
    {
        resultPanel.SetActive(true);

        if (winner == "Player1")
        {
            winnerText.text = "Blue Robot Wins!";
            winnerText.color = Color.blue;
        }
        else if (winner == "Player2")
        {
            winnerText.text = "Red Robot Wins!";
            winnerText.color = Color.red;
        }
        else if (winner == "Draw")
        {
            winnerText.text = "It's a Draw!";
            winnerText.color = Color.green;
        }

        StartCoroutine(ShowResultWithEffect());
    }

    //Add animations to the result panel
    private IEnumerator ShowResultWithEffect()
    {
        //Panel Scaling animation
        resultPanel.transform.localScale = Vector3.zero;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            resultPanel.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsed / duration);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
