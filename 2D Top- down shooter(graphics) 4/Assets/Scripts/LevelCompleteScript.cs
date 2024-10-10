using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class LevelCompleteScript : MonoBehaviour
{
    public void OnLevelComplete()
    {
        // Возобновить игру
        PauseManager.ResumeGame();

        // Если уровень является последним разблокированным уровнем, увеличиваем количество разблокированных уровней и сохраняем это
        if (LevelSelectionMenuManager.currLevel == LevelSelectionMenuManager.unlockedLevels)
        {
            LevelSelectionMenuManager.unlockedLevels++;
            SaveGame.Save<int>("unlockedLevels", LevelSelectionMenuManager.unlockedLevels);
        }

        // Загружаем сцену меню
        SceneManager.LoadScene("Menu");

        // Добавляем действие, которое будет выполнено после загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            // Находим и активируем панель выбора уровней
            GameObject levelSelectMenu = GameObject.Find("LevelSelectMenu");
            if (levelSelectMenu != null)
            {
                levelSelectMenu.SetActive(true);
            }

            // Отписываемся от события, чтобы оно не вызывалось повторно
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}