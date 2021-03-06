﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float firingRate = 0.2f;

	public float speed = 15.0f;
	public float padding = 1f;
	public float health = 250f;

	public AudioClip fireSound;

	float xMin;
	float xMax;

	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xMin = leftMost.x + padding;
		xMax = rightMost.x - padding;
	}

	void Fire () {
		Vector3 offset = new Vector3(0,1,0);
		GameObject beam = Instantiate(projectile, transform.position + offset,Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0,projectileSpeed,0);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) { 
			InvokeRepeating ("Fire", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Fire");
		}

		if(Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.RightArrow)) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		}

		// restrict the player to the game space
		float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
		transform.position = new Vector3(newX, transform.position.y,transform.position.z);
	}

	void OnTriggerEnter2D (Collider2D collider) { 
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			Debug.Log("Player collided with missile");
			health -= missile.GetDamage();
			missile.Hit();
			if(health <=0) { 
				Die();
			}
		}
	}

	void Die () {
		LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		man.LoadLevel("Win Screen");
		Destroy(gameObject);
	}

}
