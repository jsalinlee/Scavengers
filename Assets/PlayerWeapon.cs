using System.Collections;
using System.Collections.Generic;
using UnityEngine;	
public enum WeaponStages { one, two }
public class PlayerWeapon : MonoBehaviour
{
    PlayerController player;
	public GameObject projectile; // prefab for projectile
    public AudioClip fireSound;
	public AudioClip secondStage;
	public float stageDelay;
	public WeaponStages stages;
	public float fireRate;
	bool fireDisabled;
    // Use this for initialization
    void Start()
    {
		fireDisabled = false;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !fireDisabled )
        {
            Fire();
			fireDisabled = true;
			StartCoroutine("FireRate");
			if(stages == WeaponStages.two) StartCoroutine("SecondStage");
        }
    }
	IEnumerator FireRate()
	{
		yield return new WaitForSeconds(fireRate);
		fireDisabled = false;
	}
	IEnumerator SecondStage()
	{
		yield return new WaitForSeconds(stageDelay);
		PlaySound(secondStage);
	}
    void Fire()
    {
		Projectile proj = Instantiate(projectile).GetComponent<Projectile>();
		proj.transform.position = transform.position;
		proj.SetDirection(player.facing);
        PlaySound(fireSound);
    }
    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            GameObject audioObj = new GameObject();
            AudioSource src = audioObj.AddComponent<AudioSource>();
            src.clip = clip;
            src.Play();
            Destroy(audioObj, clip.length);
        }

    }
}
