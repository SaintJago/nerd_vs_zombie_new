using UnityEngine;
using BayatGames.SaveGameFree;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetProgress : MonoBehaviour 
{
    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SaveGame.DeleteAll();
        
        Debug.Log("All progress reset");
        
        // Immediate scene reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
