using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerFPS : MonoBehaviour {
	
	[Header("FPS Settings")]
	[Range(1,5)]public float speedMouse = 2;
	[Range(5,30)]public float smoothMouse = 15f;
	private float angle;

	[Header("Player Settings")]
	[Range(3,13)]public float speedMove = 10;
	[Range(0.1f,0.3f)]public float smoothMove = 0.1f;
	private Vector3 movement;
	private Vector3 refMove;


	[Header("Shooting Setting")]
	private float timeleft;
	private Vector3 originlPostionWeapon;
	public Transform pointShoot;
	[Range(0,5)] public float marginAccuracy = 2f;

	[Header("Weapon Settings")]
	[Range(0,20)] public float w_maxRotateY = 5;
	[Range(0,20)] public float w_maxRotateX = 5;
	[Range(0,10)] public float w_speedWeapon = 5;
	
	private int curWeapons;
	public WeaponSettings[] weapons;
	private WeaponSettings weapon;
	public Transform weaponEffectMove;


	[Header("Animations")]
	public Animator animMove;

	[Header("UI Setting")]
	public Image aim;
	public Image hit;
	public Image weaponImage;
	private float deltaScale = 0;
	private float alpthHit;

	private Camera cam;
	private Rigidbody rb;

	private Quaternion q_camera;
	private Quaternion q_player;

	void Start () {
	
		weapon = weapons [curWeapons];
		

		cam = Camera.main;
		rb = GetComponent<Rigidbody> ();

		timeleft = GetTimeShoot ();
		originlPostionWeapon = weaponEffectMove.localPosition;
		alpthHit = 0;


		q_player = transform.rotation;
		q_camera = cam.transform.localRotation;

		RefrshWeaponImage();
	}
	
	void Update () {
	
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
		PlayerMovement ();
		CameraFPS ();
		ShootBullet ();
		HitInfo ();
		timeleft -= Time.deltaTime;

		if(Input.GetKeyDown("e"))
		{
			if(curWeapons < weapons.Length - 1)
				curWeapons ++;
			else
				curWeapons = 0;

		}

		if(Input.GetKeyDown("q"))
		{
			if(curWeapons > 0)
				curWeapons --;
			else
				curWeapons = weapons.Length - 1;

		}

		foreach(WeaponSettings w in weapons)
		{
			if(w == weapon)
				w.gameObject.SetActive(true);
			else
				w.gameObject.SetActive(false);

		}

		weapon = weapons [curWeapons];
		RefrshWeaponImage ();

	}

	void RefrshWeaponImage()
	{
		weaponImage.sprite = weapon.weaponImage;
	}

	void FixedUpdate()
	{
		rb.MovePosition (rb.position + movement);
		Debug.DrawRay (pointShoot.position, pointShoot.forward * weapon.range, Color.red);
	}

	void CameraFPS()
	{
		float x = -Input.GetAxis("Mouse Y") * speedMouse;
		float y = Input.GetAxis("Mouse X") * speedMouse;

		angle += x;
		angle = Mathf.Clamp (angle, -60, 60);

		q_player *= Quaternion.Euler(0,y,0);
		q_camera = Quaternion.Euler(angle,0,0);

		transform.rotation = Quaternion.Lerp(transform.rotation, q_player, smoothMouse * Time.deltaTime);
		cam.transform.localRotation = Quaternion.Lerp (cam.transform.localRotation, q_camera, smoothMouse * Time.deltaTime);

		RotateWithValue (weaponEffectMove,x,y ,w_maxRotateX, w_maxRotateY, w_speedWeapon);
		
		
	}

	void PlayerMovement()
	{
		float h = Input.GetAxisRaw ("Horizontal") * speedMove;
		float v = Input.GetAxisRaw ("Vertical") * speedMove;

		Vector3 directionMove = transform.right * h * Time.deltaTime + transform.forward * v * Time.deltaTime;
		Vector3 amountMove = directionMove.normalized * speedMove * Time.deltaTime;

		movement = Vector3.SmoothDamp (movement, amountMove, ref refMove, smoothMove);


		RotateWithValue (weaponEffectMove, -v / 100,h / 2, w_maxRotateX, w_maxRotateY, w_speedWeapon);


		if (v != 0 || h != 0)
			animMove.enabled = true;
		else
			animMove.enabled = false;
	}

	void RotateWithValue(Transform obj, float x, float y, float maxX,float maxY, float speed)
	{
		obj.localRotation = Quaternion.Lerp(obj.localRotation,Quaternion.Euler (Mathf.Clamp(x,-maxX,maxX)* speed, 0, -Mathf.Clamp(y,-maxY,maxY) * speed), Time.deltaTime * 3);
		
	}

	void ShootRay()
	{
		Ray ray = new Ray (pointShoot.position, pointShoot.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, weapon.range))
		{
			if(hit.collider.gameObject.GetComponent<Rigidbody>())
			{
				Rigidbody hitRb = hit.collider.gameObject.GetComponent<Rigidbody>();
				hitRb.AddForce(Vector3.forward - hit.normal * weapon.forceAmmo);

				alpthHit = 1;
			}
			weapon.psSpark.transform.position = hit.point;
			weapon.psSpark.transform.LookAt(transform.position);
			weapon.psSpark.Play();
		}
	}

	
	void ShakePointShoot()
	{
		float randomShack = Random.Range (-marginAccuracy, marginAccuracy);
		pointShoot.localRotation = Quaternion.Euler (randomShack, randomShack, 0);
	}


	void ShootBullet()
	{
		if(timeleft <= 0)
		{
			if(!weapon.oneClick)
			{
				if(Input.GetMouseButton(0))
				{
					ShootInfo();
				}
			}else
			{
				if(Input.GetMouseButtonDown(0))
				{
					ShootInfo();
				}
			}

		}

		if(Input.GetMouseButton(1))
		{
			weaponEffectMove.localPosition = Vector3.Lerp(weaponEffectMove.localPosition, new Vector3(0,weaponEffectMove.localPosition.y, weaponEffectMove.localPosition.z), Time.deltaTime * 5);
			pointShoot.localRotation = Quaternion.identity;
			AimAnimator(0.5f);

			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 50, Time.deltaTime * 5);
			
		}else{
			weaponEffectMove.localPosition = Vector3.Lerp(weaponEffectMove.localPosition, originlPostionWeapon, Time.deltaTime * 5);
			ShakePointShoot();
			AimAnimator();

			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, Time.deltaTime * 5);
			
		}
	}

	void ShootInfo()
	{
		ShootRay();
		weapon.psFire.Play();
		deltaScale += 2f;
		deltaScale = 0;
		
		if(Input.GetMouseButton(1))
			aim.rectTransform.localScale = Vector3.one ;
		else
			aim.rectTransform.localScale = Vector3.one * 2;
		
		if(weapon.soundFire != null)
			weapon.soundFire.Play();
		if(weapon.animShoot != null)
		{
			weapon.animShoot.enabled = weapon.useAnimation;
			weapon.animShoot.Play(0);
		}
		
		timeleft = GetTimeShoot();
	}
	
	void AimAnimator(float scale = 1)
	{
		aim.rectTransform.localScale = Vector3.Lerp (aim.rectTransform.localScale, Vector3.one * scale, Time.deltaTime * 10);
	}

	void HitInfo()
	{
		alpthHit -= Time.deltaTime * 3;
		hit.color = new Color (hit.color.r, hit.color.g, hit.color.b, alpthHit);
	}

	float GetTimeShoot()
	{
		return 1 / weapon.fireRate;
	}
}
