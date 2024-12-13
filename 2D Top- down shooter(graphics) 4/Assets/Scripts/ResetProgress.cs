using UnityEngine;
using BayatGames.SaveGameFree;

public class ResetProgress : MonoBehaviour
{
		public void ResetAllProgress()
		{
				// Очистка PlayerPrefs
				PlayerPrefs.DeleteAll();
				PlayerPrefs.Save();

				// Очистка BayatGames сохранений
				SaveGame.DeleteAll();
				// Перезагрузка текущей сцены для применения изменений
				//UnityEngine.SceneManagement.SceneManager.LoadScene(
						//UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
}