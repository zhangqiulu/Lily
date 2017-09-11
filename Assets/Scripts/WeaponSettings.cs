using UnityEngine;

public class WeaponSettings : MonoBehaviour {

	public float forceAmmo;
	public float fireRate = 25f;
	public float range = 100;

	public bool oneClick = false;
	public Sprite weaponImage;
	public bool useAnimation = true;
	public AudioSource soundFire;
	public Animator animShoot;

	public ParticleSystem psSpark;
	public ParticleSystem psFire;
}
