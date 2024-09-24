using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;
using UnityEngine;

public class LevelSelectionMenuManager : MonoBehaviour
{
    public LevelObject[] levelObjects;
    public static int currLevel;
    public static int unlockedLevels;

    public void onClickLevel(int levelNum) 
    {
        currLevel = levelNum;
        SceneManager.LoadScene("Level" + (levelNum + 1));
    }

    void Start()
    {
        // Загружаем количество разблокированных уровней с помощью Save Game Free
        unlockedLevels = SaveGame.Load<int>("unlockedLevels", 0);

        for (int i = 0; i < levelObjects.Length; i++)
        {
            if(unlockedLevels >= i)
            {
                levelObjects[i].levelButton.interactable = true;
            }
        }
    }

    // Добавим метод для сохранения прогресса
    public static void SaveProgress()
    {
        SaveGame.Save<int>("unlockedLevels", unlockedLevels);
    }

    // Update is called once per frame
    void Update()
    {
        // Оставляем пустым, если нет необходимости в постоянном обновлении
    }
}