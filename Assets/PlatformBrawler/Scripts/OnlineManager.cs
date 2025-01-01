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

    public float gameDuration = 60f;
    private float countdown = 3f;

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

        //Disables player movement control scripts
        EndGame();
    }

    private void EndGame()
    {
        countdownText.text = "Finish!";
        countdownText.gameObject.SetActive(true);

        Player1Controller.enabled = false;
        Player2Controller.enabled = false;
        Player1RemoteController.enabled = false;
        Player2RemoteController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
