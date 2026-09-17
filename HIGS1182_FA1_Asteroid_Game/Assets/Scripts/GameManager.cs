using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Over Screen")]
    [Tooltip("Only used by GameOverScene. Leave these empty in the other scenes.")]
    [SerializeField] private TMP_Text outcomeText;
    [SerializeField] private TMP_Text amountCollectedText;

    private static int scrapCollected;
    private static int scrapTotal;

    private static bool CollectedEverything => scrapTotal > 0 && scrapCollected >= scrapTotal;

    private void Start()
    {
        if (outcomeText != null)
        {
            outcomeText.text = CollectedEverything ? "You won!" : "You lost!";
        }

        if (amountCollectedText != null)
        {
            amountCollectedText.text = $"You picked up {scrapCollected}/{scrapTotal} pieces of scrap!";
        }
    }

    public void PlayGame() => SceneManager.LoadScene("MainScene");

    public void ShowHowToPlay() => SceneManager.LoadScene("HowToPlayScene");

    public void ShowMainMenu() => SceneManager.LoadScene("MainMenuScene");

    public void QuitGame() => Application.Quit();

    public static void ReportScrap(int collected, int total)
    {
        scrapCollected = collected;
        scrapTotal = total;
    }

    public static void EndRun()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("GameOverScene");
    }
}
