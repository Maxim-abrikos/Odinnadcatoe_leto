using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using TMPro;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;





#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	internal class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		private float MoveSpeed = 5.5f;
		[Tooltip("Sprint speed of the character in m/s")]
		private float SprintSpeed = 8.0f;
		[Tooltip("Rotation speed of the character")]
		private float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		private float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		private float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;


#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		//private GameObject _mainCamera;
		private Camera _mainCamera;

		private const float _threshold = 0.01f;
		internal int[] Collected_Coins = new int[] { 0, 0, 0, 0 };
		internal int[] Collected_Stars_T = new int[4] { 0, 0,0,0 };
        internal int[] Collected_Stars_G = new int[4] { 0, 0,0,0 };
        internal int[,] Collected_Stars_In_Disciplines = new int[4, 10] { {0,0,0,0,0,0,0,0,0,0 },{0,0,0,0,0,0,0,0,0,0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };



		internal bool Key_T = false, Key_G = false;
		//[SerializeField]
		//private string NameOfLoad_S_T, NameOfLoad_S_G, NameOfLoad_C;
		//[SerializeField]
  //      private string NameOfLoad_S_T_, NameOfLoad_S_G_, NameOfLoad_C_;
        //public Image PauseMenu;


		private Dictionary<string, string> SomeWords = new Dictionary<string, string>() { 
	    { "Startovaya","Мне нужно попасть в лабиринт. Логично предположить, что он находится за теми воротами, но ничего страшного не произойдёт, если я задержусь тут на несколько минут."},
			{"GlavnZal", "Здесь собраны задания различных дисциплин первого курса НГТУ. Уже не терпится начать свой путь в науку!" },
			{"GlavnZal1","Будет непросто, но оно того стоит. Я сам выбрал этот путь и я должен дойти до конца... А ещё я хочу печенье." },
			{"GlavnZal2","Эмблема в виде абрикоса должна что-то значить. Я обязан выяснить, что к чему. Жду не дождусь получить свою первую звезду и первую монетку." },
        {"GlavnZal3","Естественные науки - мой конёк. Я даже не сомневаюсь, что смогу достойно проявить себя в направлениях физики, химии и биологии" }};
		

        private Vector3 playerPosition;
        private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			//if (_mainCamera == null)
			//{
			//	_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			//}

		}
		//public VectorValue pos;
		private void Start()
		{
			//transform.position = pos.initialValue;
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			if (PlayerPrefs.HasKey("ЗвёздыТ1"))
				Collected_Stars_T[0] = PlayerPrefs.GetInt("ЗвёздыТ1");
            if (PlayerPrefs.HasKey("ЗвёздыТ2"))
                Collected_Stars_T[1] = PlayerPrefs.GetInt("ЗвёздыТ2");
            if (PlayerPrefs.HasKey("ЗвёздыГ1"))
                Collected_Stars_G[0] = PlayerPrefs.GetInt("ЗвёздыГ1");
            if (PlayerPrefs.HasKey("ЗвёздыГ2"))
                Collected_Stars_G[1] = PlayerPrefs.GetInt("ЗвёздыГ2");
            if (PlayerPrefs.HasKey("ЗвёздыТ3"))
                Collected_Stars_T[2] = PlayerPrefs.GetInt("ЗвёздыТ3");
            if (PlayerPrefs.HasKey("ЗвёздыГ3"))
                Collected_Stars_G[2] = PlayerPrefs.GetInt("ЗвёздыГ3");
            if (PlayerPrefs.HasKey("ЗвёздыТ4"))
                Collected_Stars_T[3] = PlayerPrefs.GetInt("ЗвёздыТ4");
            if (PlayerPrefs.HasKey("ЗвёздыГ4"))
                Collected_Stars_G[3] = PlayerPrefs.GetInt("ЗвёздыГ4");

            if (PlayerPrefs.HasKey("Монетки1"))
				Collected_Coins[0] = PlayerPrefs.GetInt("Монетки1");
            if (PlayerPrefs.HasKey("Монетки2"))
                Collected_Coins[1] = PlayerPrefs.GetInt("Монетки2");
            if (PlayerPrefs.HasKey("Монетки3"))
                Collected_Coins[2] = PlayerPrefs.GetInt("Монетки3");
            if (PlayerPrefs.HasKey("Монетки3"))
                Collected_Coins[3] = PlayerPrefs.GetInt("Монетки4");

            if (PlayerPrefs.HasKey("ТВиМС"))
				Collected_Stars_In_Disciplines[0, 0] = PlayerPrefs.GetInt("ТВиМС");
			if (PlayerPrefs.HasKey("Химия"))
				Collected_Stars_In_Disciplines[0, 1] = PlayerPrefs.GetInt("Химия");
			if (PlayerPrefs.HasKey("МатАнализ"))
				Collected_Stars_In_Disciplines[0, 2] = PlayerPrefs.GetInt("МатАнализ");
			if (PlayerPrefs.HasKey("Физика"))
				Collected_Stars_In_Disciplines[0, 3] = PlayerPrefs.GetInt("Физика");
			if (PlayerPrefs.HasKey("Программирование"))
				Collected_Stars_In_Disciplines[0, 4] = PlayerPrefs.GetInt("Программирование");
            if (PlayerPrefs.HasKey("История"))
                Collected_Stars_In_Disciplines[0, 5] = PlayerPrefs.GetInt("История");
            if (PlayerPrefs.HasKey("Английский"))
                Collected_Stars_In_Disciplines[0, 6] = PlayerPrefs.GetInt("Английский");
            if (PlayerPrefs.HasKey("Экономика"))
                Collected_Stars_In_Disciplines[0, 7] = PlayerPrefs.GetInt("Экономика");
            if (PlayerPrefs.HasKey("ОПД"))
                Collected_Stars_In_Disciplines[0, 8] = PlayerPrefs.GetInt("ОПД");
            if (PlayerPrefs.HasKey("ОЛКК"))
                Collected_Stars_In_Disciplines[0, 9] = PlayerPrefs.GetInt("ОЛКК");



            if (PlayerPrefs.HasKey("Линал"))
                Collected_Stars_In_Disciplines[1, 0] = PlayerPrefs.GetInt("Линал");
            if (PlayerPrefs.HasKey("Графика"))
                Collected_Stars_In_Disciplines[1, 1] = PlayerPrefs.GetInt("Графика");
            if (PlayerPrefs.HasKey("Дискретка"))
                Collected_Stars_In_Disciplines[1, 2] = PlayerPrefs.GetInt("Дискретка");
            if (PlayerPrefs.HasKey("Физика+"))
                Collected_Stars_In_Disciplines[1, 3] = PlayerPrefs.GetInt("Физика+");
            if (PlayerPrefs.HasKey("Программирование+"))
                Collected_Stars_In_Disciplines[1, 4] = PlayerPrefs.GetInt("Программирование+");
            if (PlayerPrefs.HasKey("История+"))
                Collected_Stars_In_Disciplines[1, 5] = PlayerPrefs.GetInt("История+");
            if (PlayerPrefs.HasKey("История_мира"))
                Collected_Stars_In_Disciplines[1, 6] = PlayerPrefs.GetInt("История_мира");
            if (PlayerPrefs.HasKey("Экономика+"))
                Collected_Stars_In_Disciplines[1, 7] = PlayerPrefs.GetInt("Экономика+");
            if (PlayerPrefs.HasKey("Право"))
                Collected_Stars_In_Disciplines[1, 8] = PlayerPrefs.GetInt("Право");
            if (PlayerPrefs.HasKey("Литература"))
                Collected_Stars_In_Disciplines[1, 9] = PlayerPrefs.GetInt("Литература");

            if (PlayerPrefs.HasKey("20"))
                Collected_Stars_In_Disciplines[2, 0] = PlayerPrefs.GetInt("20");
            if (PlayerPrefs.HasKey("21"))
                Collected_Stars_In_Disciplines[2, 1] = PlayerPrefs.GetInt("21");
            if (PlayerPrefs.HasKey("22"))
                Collected_Stars_In_Disciplines[2, 2] = PlayerPrefs.GetInt("22");
            if (PlayerPrefs.HasKey("23"))
                Collected_Stars_In_Disciplines[2, 3] = PlayerPrefs.GetInt("23");
            if (PlayerPrefs.HasKey("24"))
                Collected_Stars_In_Disciplines[2, 4] = PlayerPrefs.GetInt("24");
            if (PlayerPrefs.HasKey("25"))
                Collected_Stars_In_Disciplines[2, 5] = PlayerPrefs.GetInt("25");
            if (PlayerPrefs.HasKey("26"))
                Collected_Stars_In_Disciplines[2, 6] = PlayerPrefs.GetInt("26");
            if (PlayerPrefs.HasKey("27"))
                Collected_Stars_In_Disciplines[2, 7] = PlayerPrefs.GetInt("27");
            if (PlayerPrefs.HasKey("28"))
                Collected_Stars_In_Disciplines[2, 8] = PlayerPrefs.GetInt("28");
            if (PlayerPrefs.HasKey("29"))
                Collected_Stars_In_Disciplines[2, 9] = PlayerPrefs.GetInt("29");



            if (PlayerPrefs.HasKey("30"))
                Collected_Stars_In_Disciplines[3, 0] = PlayerPrefs.GetInt("30");
            if (PlayerPrefs.HasKey("31"))
                Collected_Stars_In_Disciplines[3, 1] = PlayerPrefs.GetInt("31");
            if (PlayerPrefs.HasKey("32"))
                Collected_Stars_In_Disciplines[3, 2] = PlayerPrefs.GetInt("32");
            if (PlayerPrefs.HasKey("33"))
                Collected_Stars_In_Disciplines[3, 3] = PlayerPrefs.GetInt("33");
            if (PlayerPrefs.HasKey("34"))
                Collected_Stars_In_Disciplines[3, 4] = PlayerPrefs.GetInt("34");
            if (PlayerPrefs.HasKey("35"))
                Collected_Stars_In_Disciplines[3, 5] = PlayerPrefs.GetInt("35");
            if (PlayerPrefs.HasKey("36"))
                Collected_Stars_In_Disciplines[3, 6] = PlayerPrefs.GetInt("36");
            if (PlayerPrefs.HasKey("37"))
                Collected_Stars_In_Disciplines[3, 7] = PlayerPrefs.GetInt("37");
            if (PlayerPrefs.HasKey("38"))
                Collected_Stars_In_Disciplines[3, 8] = PlayerPrefs.GetInt("38");
            if (PlayerPrefs.HasKey("39"))
                Collected_Stars_In_Disciplines[3, 9] = PlayerPrefs.GetInt("39");



            if (PlayerPrefs.HasKey("Level"))
				LEVEL = PlayerPrefs.GetInt("Level");


			string s = SceneManager.GetActiveScene().name;


			//PlayerPrefs.DeleteAll();
			StartCoroutine(ShowStartDialoge());

        }

		[SerializeField]
		private TMP_Text TMT;

		IEnumerator ShowStartDialoge()
		{
			string Smth = SomeWords[SceneManager.GetActiveScene().name.ToString()];
			string CurText = "";
			TMT.text = CurText;
			Image Im = TMT.GetComponentInChildren<Image>();
			Im.enabled = true;
			for (int i = 0; i < Smth.Length; i++)
			{
                CurText = Smth.Substring(0, i + 1);
				TMT.text = CurText;
				yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.8f);
            Im.enabled = false;
			TMT.text = "";
			over = true;
			yield break;
		}


        [SerializeField]
        private TMP_Text CoinsSrarsPanel;

		internal int LEVEL = 0;

		bool over = false;
        private void Update()
		{

				JumpAndGravity();
				GroundedCheck();
				Move();

			if (over)
				CoinsSrarsPanel.text = "Монетки: " + Collected_Coins[LEVEL].ToString() + "\n" + "Звёзды: " + (Enumerable.Range(0, Collected_Stars_In_Disciplines.GetLength(1))
						 .Select(j => Collected_Stars_In_Disciplines[LEVEL, j])
						 .Sum()).ToString();
			//Get_Paused();
		}

		

		internal void ChangeRS(bool TF) 
		{
			if (TF == false)
				this.RotationSpeed = 1.0f;
			else
				this.RotationSpeed = 0.0f;
		}


		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime

				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				
				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

        private void OnDisable()
        {
			PlayerPrefs.SetInt("ЗвёздыТ1", Collected_Stars_T[0]);
            PlayerPrefs.SetInt("ЗвёздыГ1", Collected_Stars_G[0]);
            PlayerPrefs.SetInt("ЗвёздыТ2", Collected_Stars_T[1]);
            PlayerPrefs.SetInt("ЗвёздыГ2", Collected_Stars_G[1]);
            PlayerPrefs.SetInt("ЗвёздыТ3", Collected_Stars_T[2]);
            PlayerPrefs.SetInt("ЗвёздыГ3", Collected_Stars_G[2]);
            PlayerPrefs.SetInt("ЗвёздыТ4", Collected_Stars_T[3]);
            PlayerPrefs.SetInt("ЗвёздыГ4", Collected_Stars_G[3]);

            PlayerPrefs.SetInt("Монетки1", Collected_Coins[0]);
            PlayerPrefs.SetInt("Монетки2", Collected_Coins[1]);
            PlayerPrefs.SetInt("Монетки3", Collected_Coins[2]);
            PlayerPrefs.SetInt("Монетки4", Collected_Coins[3]);

            PlayerPrefs.SetInt("ТВиМС", Collected_Stars_In_Disciplines[0, 0]);
			PlayerPrefs.SetInt("Химия", Collected_Stars_In_Disciplines[0, 1]);
			PlayerPrefs.SetInt("МатАнализ", Collected_Stars_In_Disciplines[0, 2]);
			PlayerPrefs.SetInt("Физика", Collected_Stars_In_Disciplines[0, 3]);
			PlayerPrefs.SetInt("Программирование", Collected_Stars_In_Disciplines[0, 4]);
            PlayerPrefs.SetInt("История", Collected_Stars_In_Disciplines[0, 5]);
            PlayerPrefs.SetInt("Английский", Collected_Stars_In_Disciplines[0, 6]);
            PlayerPrefs.SetInt("Экономика", Collected_Stars_In_Disciplines[0, 7]);
            PlayerPrefs.SetInt("ОПД", Collected_Stars_In_Disciplines[0, 8]);
            PlayerPrefs.SetInt("ОЛКК", Collected_Stars_In_Disciplines[0, 9]);


            PlayerPrefs.SetInt("Линал", Collected_Stars_In_Disciplines[1, 0]);
            PlayerPrefs.SetInt("Графика", Collected_Stars_In_Disciplines[1, 1]);
            PlayerPrefs.SetInt("Дискретка", Collected_Stars_In_Disciplines[1, 2]);
            PlayerPrefs.SetInt("Физика+", Collected_Stars_In_Disciplines[1, 3]);
            PlayerPrefs.SetInt("Программирование+", Collected_Stars_In_Disciplines[1, 4]);
            PlayerPrefs.SetInt("История+", Collected_Stars_In_Disciplines[1, 5]);
            PlayerPrefs.SetInt("История_мира", Collected_Stars_In_Disciplines[1, 6]);
            PlayerPrefs.SetInt("Экономика+", Collected_Stars_In_Disciplines[1, 7]);
            PlayerPrefs.SetInt("Право", Collected_Stars_In_Disciplines[1, 8]);
            PlayerPrefs.SetInt("Литература", Collected_Stars_In_Disciplines[1, 9]);

            PlayerPrefs.SetInt("20", Collected_Stars_In_Disciplines[2, 0]);
            PlayerPrefs.SetInt("21", Collected_Stars_In_Disciplines[2, 1]);
            PlayerPrefs.SetInt("22", Collected_Stars_In_Disciplines[2, 2]);
            PlayerPrefs.SetInt("23", Collected_Stars_In_Disciplines[2, 3]);
            PlayerPrefs.SetInt("24", Collected_Stars_In_Disciplines[2, 4]);
            PlayerPrefs.SetInt("25", Collected_Stars_In_Disciplines[2, 5]);
            PlayerPrefs.SetInt("26", Collected_Stars_In_Disciplines[2, 6]);
            PlayerPrefs.SetInt("27+", Collected_Stars_In_Disciplines[2, 7]);
            PlayerPrefs.SetInt("28", Collected_Stars_In_Disciplines[2, 8]);
            PlayerPrefs.SetInt("29", Collected_Stars_In_Disciplines[2, 9]);


            PlayerPrefs.SetInt("30", Collected_Stars_In_Disciplines[3, 0]);
            PlayerPrefs.SetInt("31", Collected_Stars_In_Disciplines[3, 1]);
            PlayerPrefs.SetInt("32", Collected_Stars_In_Disciplines[3, 2]);
            PlayerPrefs.SetInt("33", Collected_Stars_In_Disciplines[3, 3]);
            PlayerPrefs.SetInt("34", Collected_Stars_In_Disciplines[3, 4]);
            PlayerPrefs.SetInt("35", Collected_Stars_In_Disciplines[3, 5]);
            PlayerPrefs.SetInt("36", Collected_Stars_In_Disciplines[3, 6]);
            PlayerPrefs.SetInt("37", Collected_Stars_In_Disciplines[3, 7]);
            PlayerPrefs.SetInt("38", Collected_Stars_In_Disciplines[3, 8]);
            PlayerPrefs.SetInt("39", Collected_Stars_In_Disciplines[3, 9]);


            PlayerPrefs.SetInt("Level", LEVEL);
            PlayerPrefs.Save();
		}

        private void OnDestroy()
        {
            PlayerPrefs.SetInt("ЗвёздыТ1", Collected_Stars_T[0]);
            PlayerPrefs.SetInt("ЗвёздыГ1", Collected_Stars_G[0]);
            PlayerPrefs.SetInt("ЗвёздыТ2", Collected_Stars_T[1]);
            PlayerPrefs.SetInt("ЗвёздыГ2", Collected_Stars_G[1]);
            PlayerPrefs.SetInt("ЗвёздыТ3", Collected_Stars_T[2]);
            PlayerPrefs.SetInt("ЗвёздыГ3", Collected_Stars_G[2]);
            PlayerPrefs.SetInt("ЗвёздыТ4", Collected_Stars_T[3]);
            PlayerPrefs.SetInt("ЗвёздыГ4", Collected_Stars_G[3]);


            PlayerPrefs.SetInt("Монетки1", Collected_Coins[0]);
            PlayerPrefs.SetInt("Монетки2", Collected_Coins[1]);
            PlayerPrefs.SetInt("Монетки3", Collected_Coins[2]);
            PlayerPrefs.SetInt("Монетки4", Collected_Coins[3]);


            PlayerPrefs.SetInt("ТВиМС", Collected_Stars_In_Disciplines[0, 0]);
			PlayerPrefs.SetInt("Химия", Collected_Stars_In_Disciplines[0, 1]);
			PlayerPrefs.SetInt("МатАнализ", Collected_Stars_In_Disciplines[0, 2]);
			PlayerPrefs.SetInt("Физика", Collected_Stars_In_Disciplines[0, 3]);
			PlayerPrefs.SetInt("Программирование", Collected_Stars_In_Disciplines[0, 4]);
            PlayerPrefs.SetInt("История", Collected_Stars_In_Disciplines[0, 5]);
            PlayerPrefs.SetInt("Английский", Collected_Stars_In_Disciplines[0, 6]);
            PlayerPrefs.SetInt("Экономика", Collected_Stars_In_Disciplines[0, 7]);
            PlayerPrefs.SetInt("ОПД", Collected_Stars_In_Disciplines[0, 8]);
            PlayerPrefs.SetInt("ОЛКК", Collected_Stars_In_Disciplines[0, 9]);


            PlayerPrefs.SetInt("Линал", Collected_Stars_In_Disciplines[1, 0]);
            PlayerPrefs.SetInt("Графика", Collected_Stars_In_Disciplines[1, 1]);
            PlayerPrefs.SetInt("Дискретка", Collected_Stars_In_Disciplines[1, 2]);
            PlayerPrefs.SetInt("Физика+", Collected_Stars_In_Disciplines[1, 3]);
            PlayerPrefs.SetInt("Программирование+", Collected_Stars_In_Disciplines[1, 4]);
            PlayerPrefs.SetInt("История+", Collected_Stars_In_Disciplines[1, 5]);
            PlayerPrefs.SetInt("История_мира", Collected_Stars_In_Disciplines[1, 6]);
            PlayerPrefs.SetInt("Экономика+", Collected_Stars_In_Disciplines[1, 7]);
            PlayerPrefs.SetInt("Право", Collected_Stars_In_Disciplines[1, 8]);
            PlayerPrefs.SetInt("Литература", Collected_Stars_In_Disciplines[1, 9]);

            PlayerPrefs.SetInt("20", Collected_Stars_In_Disciplines[2, 0]);
            PlayerPrefs.SetInt("21", Collected_Stars_In_Disciplines[2, 1]);
            PlayerPrefs.SetInt("22", Collected_Stars_In_Disciplines[2, 2]);
            PlayerPrefs.SetInt("23", Collected_Stars_In_Disciplines[2, 3]);
            PlayerPrefs.SetInt("24", Collected_Stars_In_Disciplines[2, 4]);
            PlayerPrefs.SetInt("25", Collected_Stars_In_Disciplines[2, 5]);
            PlayerPrefs.SetInt("26", Collected_Stars_In_Disciplines[2, 6]);
            PlayerPrefs.SetInt("27", Collected_Stars_In_Disciplines[2, 7]);
            PlayerPrefs.SetInt("28", Collected_Stars_In_Disciplines[2, 8]);
            PlayerPrefs.SetInt("29", Collected_Stars_In_Disciplines[2, 9]);


            PlayerPrefs.SetInt("30", Collected_Stars_In_Disciplines[3, 0]);
            PlayerPrefs.SetInt("31", Collected_Stars_In_Disciplines[3, 1]);
            PlayerPrefs.SetInt("32", Collected_Stars_In_Disciplines[3, 2]);
            PlayerPrefs.SetInt("33", Collected_Stars_In_Disciplines[3, 3]);
            PlayerPrefs.SetInt("34", Collected_Stars_In_Disciplines[3, 4]);
            PlayerPrefs.SetInt("35", Collected_Stars_In_Disciplines[3, 5]);
            PlayerPrefs.SetInt("36", Collected_Stars_In_Disciplines[3, 6]);
            PlayerPrefs.SetInt("37", Collected_Stars_In_Disciplines[3, 7]);
            PlayerPrefs.SetInt("38", Collected_Stars_In_Disciplines[3, 8]);
            PlayerPrefs.SetInt("39", Collected_Stars_In_Disciplines[3, 9]);


            PlayerPrefs.SetInt("Level", LEVEL);
            PlayerPrefs.Save();
		}





    }
}