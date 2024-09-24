using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static bool gamePaused = false;

    public static bool IsGamePaused
    {
        get { return gamePaused; }
    }

    public static void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
    }
}
