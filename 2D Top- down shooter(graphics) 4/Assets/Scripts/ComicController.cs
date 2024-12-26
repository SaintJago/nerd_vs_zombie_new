using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ComicController : MonoBehaviour
{
		[SerializeField] private VideoPlayer videoPlayer;
		[SerializeField] private string menuSceneName = "Menu";
		[SerializeField] private string firstLevelSceneName = "FirstLevel";
		[SerializeField] private Button skipButton;
		[SerializeField] private bool autoPlayOnStart = true;

		[Header("Настройки показа катсцены")]
		[SerializeField] private bool usePlayerPrefsLogic = true;

		[Header("Настройки кнопки пропуска")]
		[SerializeField] private float skipButtonDelay = 3f;              // Задержка перед появлением кнопки
		[SerializeField] private float skipButtonFadeDuration = 1f;       // Длительность анимации появления
		[SerializeField] private bool showSkipButtonInstantly = false;    // Показывать кнопку мгновенно
		[SerializeField] private bool hideSkipButtonOnStart = true;       // Скрывать ли кнопку при старте

		private const string FirstLaunchKey = "IsFirstLaunch";
		private const string ReplayModeKey = "IsReplayMode";
		private CanvasGroup skipButtonCanvasGroup;

		private void Start()
		{
				if (videoPlayer == null)
				{
						Debug.LogError("VideoPlayer component is not assigned!");
						return;
				}

				InitializeSkipButton();

				videoPlayer.loopPointReached += OnVideoFinished;
				if (skipButton != null)
				{
						skipButton.onClick.AddListener(SkipVideo);
				}
				else
				{
						Debug.LogWarning("Skip Button is not assigned. Skip functionality will be unavailable.");
				}

				if (ShouldPlayCutscene())
				{
						if (autoPlayOnStart)
						{
								PlayVideo();
						}
				}
				else
				{
						LoadMenuScene();
				}
		}

		private void InitializeSkipButton()
		{
				if (skipButton != null)
				{
						// Получаем или добавляем компонент CanvasGroup
						skipButtonCanvasGroup = skipButton.GetComponent<CanvasGroup>();
						if (skipButtonCanvasGroup == null)
						{
								skipButtonCanvasGroup = skipButton.gameObject.AddComponent<CanvasGroup>();
						}

						// Настраиваем начальное состояние
						if (hideSkipButtonOnStart)
						{
								skipButtonCanvasGroup.alpha = 0f;
								skipButtonCanvasGroup.interactable = false;
								skipButtonCanvasGroup.blocksRaycasts = false;
						}

						// Запускаем корутину для показа кнопки
						StartCoroutine(ShowSkipButtonWithDelay());
				}
		}

		private IEnumerator ShowSkipButtonWithDelay()
		{
				if (skipButtonDelay > 0 && !showSkipButtonInstantly)
				{
						yield return new WaitForSeconds(skipButtonDelay);
				}

				if (showSkipButtonInstantly)
				{
						skipButtonCanvasGroup.alpha = 1f;
						skipButtonCanvasGroup.interactable = true;
						skipButtonCanvasGroup.blocksRaycasts = true;
				}
				else
				{
						float elapsedTime = 0f;
						while (elapsedTime < skipButtonFadeDuration)
						{
								elapsedTime += Time.deltaTime;
								float normalizedTime = elapsedTime / skipButtonFadeDuration;

								skipButtonCanvasGroup.alpha = Mathf.Lerp(0f, 1f, normalizedTime);
								if (normalizedTime >= 0.5f) // Включаем интерактивность на половине анимации
								{
										skipButtonCanvasGroup.interactable = true;
										skipButtonCanvasGroup.blocksRaycasts = true;
								}

								yield return null;
						}

						skipButtonCanvasGroup.alpha = 1f;
				}
		}

		private bool ShouldPlayCutscene()
		{
				if (!usePlayerPrefsLogic)
				{
						return true;
				}
				return IsFirstLaunch() || IsReplayMode();
		}

		private bool IsFirstLaunch()
		{
				if (!PlayerPrefs.HasKey(FirstLaunchKey))
				{
						PlayerPrefs.SetInt(FirstLaunchKey, 1);
						PlayerPrefs.Save();
						return true;
				}
				return false;
		}

		private bool IsReplayMode()
		{
				bool isReplay = PlayerPrefs.GetInt(ReplayModeKey, 0) == 1;
				PlayerPrefs.SetInt(ReplayModeKey, 0);
				PlayerPrefs.Save();
				return isReplay;
		}
		
		private void OnVideoFinished(VideoPlayer vp)
		{
				StopAudio();
				if (IsFirstLaunch())
				{
						SceneManager.LoadScene(firstLevelSceneName);
				}
				else
				{
						LoadMenuScene();
				}
		}

		private void SkipVideo()
		{
				if (videoPlayer.isPlaying)
				{
						videoPlayer.Stop();
						StopAudio();
						LoadMenuScene();
				}
		}

		private void LoadMenuScene()
		{
				SceneManager.LoadScene(menuSceneName);
		}

		private void StopAudio()
		{
				AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
				foreach (AudioSource audioSource in allAudioSources)
				{
						audioSource.Stop();
				}
		}

		private void OnDestroy()
		{
				if (skipButton != null)
				{
						skipButton.onClick.RemoveListener(SkipVideo);
				}
		}

		public void PlayVideo()
		{
				if (videoPlayer != null)
				{
						videoPlayer.Stop();
						videoPlayer.Play();
				}
		}

		public static void SetReplayMode()
		{
				PlayerPrefs.SetInt(ReplayModeKey, 1);
				PlayerPrefs.Save();
		}
}