using UnityEngine;
using BayatGames.SaveGameFree;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetProgress : MonoBehaviour 
{
    public void ResetAllProgress() 
    {
        // Очистка PlayerPrefs 
        PlayerPrefs.DeleteAll(); 
        PlayerPrefs.Save();  

        // Очистка BayatGames сохранений 
        SaveGame.DeleteAll(); 
        Debug.Log("All progress reset");  

        // Запуск корутины с задержкой перезагрузки сцены 
        StartCoroutine(ReloadSceneWithDelay()); 
    }  

    private IEnumerator ReloadSceneWithDelay() 
    {
        // Ожидание 1 секунды перед перезагрузкой 
        yield return new WaitForSeconds(1f);  

        // Асинхронная загрузка сцены
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Ожидаем полной загрузки сцены
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}