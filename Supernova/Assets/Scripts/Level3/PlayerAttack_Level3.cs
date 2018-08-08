﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack_Level3 : MonoBehaviour {

	public KeyCode shootKey;
	private Animator anim;
	private bool isShoot = false; //表示是否射击
	private float shootTimer = 0.0f;// 射击的计时器
	public float shootTime = 0.47f;// 表示射击的CD时间
	private FaceDirection faceDir;
	private bool fireCommand;

	public GameObject dragonBullet, knightBullet, magicBullet, assassinBullet, bossBullet;
	private GameObject bullet;

	public GameObject networkManager;
	private TcpClient_Level3 tcpClient;

	public AudioClip dragonAttack, knightAttack, magicAttack, assassinAttack, bossAttack;
	private AudioSource audioSource;
	private AudioSource[] audioSources;
	private Vector2 firePosition;

	public void SetFireCommand(Vector2 pos)
	{
		fireCommand = true;
		firePosition = pos;
	}

	void Start()
	{
		anim = GetComponent<Animator>();
		tcpClient = networkManager.GetComponent<TcpClient_Level3>();
		audioSources = GetComponents<AudioSource>();
		audioSource = audioSources[0];
		firePosition = transform.position;
	}

	void FixedUpdate()
	{
		if (isShoot)
		{
			shootTimer += Time.deltaTime;
			if(shootTimer >= shootTime)
			{
				shootTimer = 0.0f;
				isShoot = false;
			}
		}
		else
		{
			Shoot();
		}
	}

	void Shoot()
	{
		if ( (gameObject.CompareTag("Player1") && PlayerStatusControl_Level3._instance.playerIdentity == 1)
			|| (gameObject.CompareTag("Player2") && PlayerStatusControl_Level3._instance.playerIdentity == 2)
			|| (gameObject.CompareTag("Player3") && PlayerStatusControl_Level3._instance.playerIdentity == 3) )
		{
			ControlShoot();
		}
		else
		{
			EnemyShoot();
		}
	}

	void ControlShoot()
	{
		if(ETCInput.GetButton("Button_Level2") || Input.GetKeyDown(shootKey))
		{
			tcpClient.SendFireCommand(transform.position);
			Fire();
		}
	}
	/* 
	void EnemyShoot()
	{
		if(currentCommand == "F")
		{
			Fire();
			currentCommand = "";
		}
	}
	*/

	void EnemyShoot()
	{
		if(fireCommand)
		{
			EnemyFire();
			fireCommand = false;
			firePosition = transform.position;
		}
	}

	void SetBulletTag(GameObject bullet)
	{
		Bullet_Level3 bullet_1 = bullet.GetComponent<Bullet_Level3>();
		bullet_1.role =
		bullet_1.parentTag =
	}

	void SelectAccordingToPower(PlayerPower_Level3 power)
	{
		switch(power)
		{
			case PlayerPower_Level3.DragonPower1:
				bullet = dragonBullet;
				audioSource.clip = dragonAttack;
				break;
			case PlayerPower_Level3.DragonPower2:
				bullet = dragonBullet;
				audioSource.clip = dragonAttack;
				break;
			case PlayerPower_Level3.DragonPower3:
				bullet = dragonBullet;
				audioSource.clip = dragonAttack;
				break;
			case PlayerPower_Level3.KnightPower1:
				bullet = knightBullet;
				audioSource.clip = knightAttack;
				break;
			case PlayerPower_Level3.KnightPower2:
				bullet = knightBullet;
				audioSource.clip = knightAttack;
				break;
			case PlayerPower_Level3.MagicPower1:
				bullet = magicBullet;
				audioSource.clip = magicAttack;
				break;
			case PlayerPower_Level3.MagicPower2:
				bullet = magicBullet;
				audioSource.clip = magicAttack;
				break;
			case PlayerPower_Level3.MagicPower3:
				bullet = magicBullet;
				audioSource.clip = magicAttack;
				break;
			case PlayerPower_Level3.AssassinPower1:
				bullet = assassinBullet;
				audioSource.clip = assassinAttack;
				break;
			case PlayerPower_Level3.AssassinPower2:
				bullet = assassinBullet;
				audioSource.clip = assassinAttack;
				break;
			case PlayerPower_Level3.BossPower:
				bullet = bossBullet;
				audioSource.clip = bossAttack;
				break;
			case PlayerPower_Level3.Default:
				bullet = null;
				audioSource.clip = null;
				break;
		}
	}

	void SelectBullet()
	{
		if(gameObject.CompareTag("Player1"))
		{
			switch(Player1Status_Level3._instance.playerSkill)
			{
				case PlayerSkill_Level3.ShieldSkill:
					break;
				case PlayerSkill_Level3.GunSkill:
					break;
				case PlayerSkill_Level3.IceSkill:
					break;
				case PlayerSkill_Level3.Default:
					SelectAccordingToPower(Player1Status_Level3._instance.playerPower);
					break;
			}
		}
		else if(gameObject.CompareTag("Player2"))
		{
			switch(Player2Status_Level3._instance.playerSkill)
			{
				case PlayerSkill_Level3.ShieldSkill:
					break;
				case PlayerSkill_Level3.GunSkill:
					break;
				case PlayerSkill_Level3.IceSkill:
					break;
				case PlayerSkill_Level3.Default:
					SelectAccordingToPower(Player2Status_Level3._instance.playerPower);
					break;
			}
		}
		else if(gameObject.CompareTag("Player3"))
		{
			switch(Player3Status_Level3._instance.playerSkill)
			{
				case PlayerSkill_Level3.ShieldSkill:
					break;
				case PlayerSkill_Level3.GunSkill:
					break;
				case PlayerSkill_Level3.IceSkill:
					break;
				case PlayerSkill_Level3.Default:
					SelectAccordingToPower(Player3Status_Level3._instance.playerPower);
					break;
			}
		}
	}


	void Fire()
	{
		isShoot = true;
		faceDir = gameObject.GetComponent<PlayerMove_Level3>().dir;

		SelectBullet();
		audioSource.Play();
		switch (faceDir)
		{
			case FaceDirection.Up:
				anim.SetTrigger("UpAttack");
				if(bullet)
				{	
					if((gameObject.CompareTag("Player1") && Player1Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower) ||
						(gameObject.CompareTag("Player2") && Player2Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower) ||
						(gameObject.CompareTag("Player3") && Player3Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower))
					{
						UpAttack();
						LeftAttack();
						RightAttack();
					}
					else
					{
						UpAttack();
					}
				}
				break;

			case FaceDirection.Down:
				anim.SetTrigger("DownAttack");
				if(bullet)
				{	
					if((gameObject.CompareTag("Player1") && Player1Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower) ||
						(gameObject.CompareTag("Player2") && Player2Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower) ||
						(gameObject.CompareTag("Player3") && Player3Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower))
					{
						DownAttack();
						LeftAttack();
						RightAttack();
					}
					else
					{
						DownAttack();
					}
				}
				break;

			case FaceDirection.Left:
				anim.SetTrigger("LeftAttack");
				if(bullet)
				{
					if((gameObject.CompareTag("Player1") && Player1Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower) ||
						(gameObject.CompareTag("Player2") && Player2Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower) ||
						(gameObject.CompareTag("Player3") && Player3Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower))
					{
						LeftAttack();
						UpAttack();
						DownAttack();
					}
					else
					{
						LeftAttack();
					}
				}
				break;

			case FaceDirection.Right:
				anim.SetTrigger("RightAttack");
				if(bullet)
				{
					if((gameObject.CompareTag("Player1") && Player1Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower) ||
						(gameObject.CompareTag("Player2") && Player2Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower) ||
						(gameObject.CompareTag("Player3") && Player3Status_Level3._instance.playerPower == PlayerPower_Level3.BossPower))
					{
						RightAttack();
						UpAttack();
						DownAttack();
					}
					else
					{
						RightAttack();
					}
				}
				break;
		}
		
	}

	void EnemyFire()
	{
		isShoot = true;
		faceDir = gameObject.GetComponent<PlayerMove_Level3>().dir;
		SelectBullet();
		audioSource.Play();
		switch (faceDir)
		{
			case FaceDirection.Up:
				anim.SetTrigger("UpAttack");
				if(bullet)
				{	
					if((gameObject.CompareTag("Player1") && Player1Status_Level2._instance.playerPower == PlayerPower_Level2.BossPower) ||
						(gameObject.CompareTag("Player2") && Player2Status_Level2._instance.playerPower == PlayerPower_Level2.BossPower))
					{
						EnemyUpAttack();
						EnemyLeftAttack();
						EnemyRightAttack();
					}
					else
					{
						EnemyUpAttack();
					}
				}
				break;

			case FaceDirection.Down:
				anim.SetTrigger("DownAttack");
				if(bullet)
				{	
					if((gameObject.CompareTag("Player1") && Player1Status_Level2._instance.playerPower == PlayerPower_Level2.BossPower) ||
						(gameObject.CompareTag("Player2") && Player2Status_Level2._instance.playerPower == PlayerPower_Level2.BossPower))
					{
						EnemyDownAttack();
						EnemyLeftAttack();
						EnemyRightAttack();
					}
					else
					{
						EnemyDownAttack();
					}
				}
				break;

			case FaceDirection.Left:
				anim.SetTrigger("LeftAttack");
				if(bullet)
				{
					if((gameObject.CompareTag("Player1") && Player1Status_Level2._instance.playerPower == PlayerPower_Level2.BossPower) ||
						(gameObject.CompareTag("Player2") && Player2Status_Level2._instance.playerPower == PlayerPower_Level2.BossPower))
					{
						EnemyLeftAttack();
						EnemyUpAttack();
						EnemyDownAttack();
					}
					else
					{
						EnemyLeftAttack();
					}
				}
				break;

			case FaceDirection.Right:
				anim.SetTrigger("RightAttack");
				if(bullet)
				{
					if((gameObject.CompareTag("Player1") && Player1Status_Level2._instance.playerPower == PlayerPower_Level2.BossPower) ||
						(gameObject.CompareTag("Player2") && Player2Status_Level2._instance.playerPower == PlayerPower_Level2.BossPower))
					{
						EnemyRightAttack();
						EnemyUpAttack();
						EnemyDownAttack();
					}
					else
					{
						EnemyRightAttack();
					}
				}
				break;
		}
		
	}

	void UpAttack()
	{
		GameObject go = (GameObject)Instantiate(bullet, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.Euler(0f,0f,90f), this.transform);
		SetBulletTag(go);
	}

	void DownAttack()
	{
		GameObject go = (GameObject)Instantiate(bullet, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.Euler(0f,0f,-90f), this.transform);
		SetBulletTag(go);
	}

	void LeftAttack()
	{
		GameObject go = (GameObject)Instantiate(bullet, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.Euler(0f,0f,-180f), this.transform);
		SetBulletTag(go);
	}

	void RightAttack()
	{
		GameObject go = (GameObject)Instantiate(bullet, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity, this.transform);
		SetBulletTag(go);
	}


	void EnemyUpAttack()
	{
		GameObject go = (GameObject)Instantiate(bullet, new Vector2(firePosition.x, firePosition.y), Quaternion.Euler(0f,0f,90f));
		SetBulletTag(go);
	}

	void EnemyDownAttack()
	{
		GameObject go = (GameObject)Instantiate(bullet, new Vector2(firePosition.x, firePosition.y), Quaternion.Euler(0f,0f,-90f));
		SetBulletTag(go);
	}

	void EnemyLeftAttack()
	{
		GameObject go = (GameObject)Instantiate(bullet, new Vector2(firePosition.x, firePosition.y), Quaternion.Euler(0f,0f,-180f));
		SetBulletTag(go);
	}

	void EnemyRightAttack()
	{
		GameObject go = (GameObject)Instantiate(bullet, new Vector2(firePosition.x, firePosition.y), Quaternion.identity);
		SetBulletTag(go);
	}
}


			