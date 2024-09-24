using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using MyGame;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
		[SerializeField] float speed;
		[SerializeField] int health;
		[HideInInspector] public int maxHealth;

		Rigidbody2D rb;
		Vector2 moveVelocity;

		Animator anim;
		SpriteRenderer spR;

		[SerializeField] TextMeshProUGUI text;

		public static Player Instance;

		[SerializeField] GameObject hitEffect;

		[SerializeField] float dashForce, timeBtwDash, dashTime;
		float dashTimer;
		bool isDashing = false;

		[SerializeField] Slider healthSlider;
		[SerializeField] Slider dashSlider;
		[SerializeField] ParticleSystem footParticle;
		[SerializeField] GameObject deathPanel;
		[SerializeField] GameObject dronePrefab;
		public GameObject droneInstance { get; private set; }

		bool canBeDamaged = true;

		[SerializeField] AudioClip heartClip, deathClip, dashSound;
		[SerializeField] AudioClip[] footClips;
		AudioSource audS;

		Vector2 moveInput;
		[HideInInspector] public int currentMoney;
		[SerializeField] TextMeshProUGUI coinsText;

		// Input System
		private PlayerControls controls;
		private InputAction moveAction;
		private InputAction dashAction;
		

		private void Awake()
		{
				Instance = this;
				controls = new PlayerControls();
				moveAction = controls.Player.move;
				dashAction = controls.Player.dash;

				dashAction.performed += _ => TryDash();
		}

		private void OnEnable()
		{
				controls.Enable();
		}

		private void OnDisable()
		{
				controls.Disable();
		}

		void Start()
		{
				rb = GetComponent<Rigidbody2D>();
				anim = GetComponent<Animator>();
				spR = GetComponent<SpriteRenderer>();
				audS = GetComponent<AudioSource>();

				dashTimer = timeBtwDash;
				maxHealth = health;

				UpdateHealthUI();
				droneInstance = Instantiate(dronePrefab, transform.position, Quaternion.identity);
				droneInstance.SetActive(false);
		}

		void Update()
		{
				dashTimer += Time.deltaTime;
				dashSlider.value = dashTimer / timeBtwDash;
		}

		private void FixedUpdate()
		{
				Move();
				if (isDashing) Dash();
		}

		void TryDash()
		{
				if (dashTimer >= timeBtwDash)
				{
						dashTimer = 0;
						ActivateDash();
				}
		}

		void Dash()
		{
				rb.AddForce(moveInput * Time.fixedDeltaTime * dashForce * 100);
		}

		void ActivateDash()
		{
				isDashing = true;
				canBeDamaged = false;
				SoundManager.Instance.PlayerSound(dashSound);
				Invoke(nameof(DeActivateDash), dashTime);
		}

		void DeActivateDash()
		{
				isDashing = false;
				canBeDamaged = true;
		}

		void Move()
		{
				moveInput = moveAction.ReadValue<Vector2>();

				if (moveInput != Vector2.zero)
				{
						anim.SetBool("run", true);
						footParticle.Pause();
						footParticle.Play();

						var emission = footParticle.emission;
						emission.rateOverTime = 10;

						if (!audS.isPlaying)
						{
								audS.clip = footClips[Random.Range(0, footClips.Length)];
								audS.Play();
						}
				}
				else
				{
						anim.SetBool("run", false);
						var emission = footParticle.emission;
						emission.rateOverTime = 0;
				}

				ScalePlayer(moveInput.x);
				moveVelocity = moveInput.normalized * speed;
				rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
		}

		void ScalePlayer(float x)
		{
				if (x > 0)
						spR.flipX = false;
				else if (x < 0)
						spR.flipX = true;
		}

		public void Damage(int damage)
		{
				if (!canBeDamaged) return;

				health -= damage;
				Instantiate(hitEffect, transform.position, Quaternion.identity);
				CameraFollow.Instance.CamShake();
				SoundManager.Instance.PlayerSound(heartClip);
				UpdateHealthUI();

				if (health <= 0 && deathPanel.activeInHierarchy == false)
				{
						PauseManager.PauseGame();
						SoundManager.Instance.PlayerSound(deathClip);
						deathPanel.SetActive(true);
						gameObject.SetActive(false);
				}
		}

		public void AddHealth(int value)
		{
				if (health <= 0) health = 0;
				health += value;

				if (health > maxHealth) health = maxHealth;
				UpdateHealthUI();
		}

		void UpdateHealthUI()
		{
				healthSlider.value = (float)health / maxHealth;
		}

		public void AddMoney(int value)
		{
				currentMoney += value;

				LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Coins").Completed += op =>
				{
						if (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
						{
								coinsText.text = op.Result + ": " + currentMoney.ToString();
						}
				};
		}
}
