using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    public UI_InGame inGameUI { get; private set; }
    public UI_WeaponSelection weaponSelectionUI { get; private set; }
    public GameObject pauseUI;
    public UI_GameOver gameOverUI { get; private set; }
    public UICreditsRoll uICreditsRoll { get; private set; }
    private string message = "GameOver!! YOU LOSE";

    [SerializeField] private GameObject[] uiElements;
    [Header("Fade in here")]
    [SerializeField] private Image fadeImage;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        weaponSelectionUI = GetComponentInChildren<UI_WeaponSelection>(true);
        gameOverUI = GetComponentInChildren<UI_GameOver>(true);
        uICreditsRoll = GetComponentInChildren<UICreditsRoll>(true);
    }

    void Start()
    {
        StartCoroutine(FadeImageAlpha(0, 2, null));

    }

    public void ShowGameOverUI()
    {
        SwitchTo(gameOverUI.gameObject);
        gameOverUI.ShowGameOverMessage(message);
    }

    public void ShowGameWinner()
    {
        SwitchTo(uICreditsRoll.gameObject);
        StartCoroutine(RestartAfterCredits());
    }

    private IEnumerator RestartAfterCredits()
    {
        // Show credits for 5 seconds (adjust as needed)
        yield return new WaitForSeconds(10f);
        RestartGame();
    }
    public void SwitchTo(GameObject uiToSwitchOn)
    {
        foreach (GameObject go in uiElements)
        {
            go.SetActive(false);
        }

        uiToSwitchOn.SetActive(true);
    }

    public void StartTheGame()
    {
        Debug.Log("inGameUI: " + (inGameUI != null));
        Debug.Log("GameManager.instance: " + (GameManager.instance != null));
        StartCoroutine(StartGameSequence());
    }
    public void QuitApplication() => Application.Quit();

    public void RestartGame()
    {
        StartCoroutine(FadeImageAlpha(1, 1f, GameManager.instance.RestartScene));
    }

    public void PauseSwitch()
    {
        bool gamePaused = pauseUI.activeSelf;
        if (gamePaused)
        {
            SwitchTo(inGameUI.gameObject);
            Time.timeScale = 1;
        }
        else
        {
            SwitchTo(pauseUI);
            Time.timeScale = 0;

        }
    }



    // Credit to Megaman Tutorial for this fading function : 
    //Links YOUSTUBE: https://www.youtube.com/watch?v=pARyrvmz4Bo&list=PLfKMG2CcLNaLVod7WS3Yg28CJis_7L2PQ&ab_channel=GameDevwithTony
    private IEnumerator FadeImageAlpha(float targetAlpha, float duration, System.Action onComplete)
    {
        float timer = 0;
        Color currentColor = fadeImage.color;
        float startAlpha = currentColor.a;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);

            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

            yield return null;
        }

        fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);


        //call the oncomplet method ifit exits
        onComplete?.Invoke();
    }


    private IEnumerator StartGameSequence()
    {
        StartCoroutine(FadeImageAlpha(1, 1, null));
        yield return new WaitForSeconds(1);
        SwitchTo(inGameUI.gameObject);
        GameManager.instance.GameStart();
        StartCoroutine(FadeImageAlpha(0, 1, null));
    }
}
