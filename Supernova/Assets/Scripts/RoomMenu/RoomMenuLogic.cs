﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomMenuLogic : MonoBehaviour {

	public static RoomMenuLogic _instance;
	public GameObject gameRoom; //游戏大厅的 Sprite

	public GameObject[] playerCharacter; //表示房间里显示的 4 个角色立绘
	private Image[] playerCharacterImage;
	public GameObject[] readyPic;//表示4个角色的Ready图片
	public Sprite[] characterSprites; //4 个角色的立绘图像
	public Sprite[] mapSprites;
	
	public Sprite defaultCharacter;

	public int playerIndex = 0; // 表示本地的该玩家是几号,如果是0表示那么表示第一个进入房间，他就是房主
	private int playerTotal = 0; // 表示当前房间的玩家数
	public int picIndex = 0; // 表示玩家选的是第几个立绘，默认是第一个立绘

	private int readyCount = 0;

	public bool isTeam1 = true;
	private int team1Number = 0;

	public GameObject characterSelect;
	private Image characterSelectImage;
	public GameObject mapSelect;
	private Image mapSelectImage;

	public int mapIndex;//TODO
	public GameObject readyBtn;
	public GameObject goBtn;

	public bool isReady = false;//是否准备

	//载入游戏需要的数据成员
	public Slider processBar;
	private AsyncOperation async;
	private int nowProcess;//当前进度
	public GameObject loadingPicture;//Lodading界面图片
	private int sceneIndex = 0;//表示需要载入的游戏场景的Index
	private int sceneTotal;//表示总共可以载入的游戏场景关卡
	public Text[] teamText;
	public int[] playerCharacterIndexArray;
	public int[] playerReadyStatus;
	public int[] playerTeamStatus;

	// Use this for initialization

	public void SetPlayerTeam(int player, int team)
	{
		playerTeamStatus[player] = team;
	}

	public void SetPlayerReady(int player)
	{
		playerReadyStatus[player] = 1;
		readyCount++;
	}
	public void SetPlayerCharacter(int player, int character)
	{
		playerCharacterIndexArray[player] = character;
	}
	public void SetMap(int map)
	{
		mapIndex = map;
	}
	public void SetPlayerQuit(int player)
	{
		playerCharacterIndexArray[player] = -1;
		playerReadyStatus[player] = 0;
		playerTeamStatus[player] = 0;
		playerTotal--;
	}
	public void StartGame(){}

	public void ReceiveNewIncomer(int player)
	{
		playerCharacterIndexArray[player] = 0;
		playerTotal++;
		TcpClient_All._instance.SendCharacterSelectCommand(playerIndex, picIndex);
		TcpClient_All._instance.SendTeamCommand(playerIndex, playerTeamStatus[playerIndex]);
		if(isReady)
		{
			TcpClient_All._instance.SendReadyCommand(playerIndex);
		}
		if(playerIndex == 0)
		{
			TcpClient_All._instance.SendMapSelectCommand(mapIndex);
		}

	}

	void CheckPlayerStatus()
	{
		for(int i = 0; i < 4; i++){
			if(playerCharacterIndexArray[i] == -1)
			{
				playerCharacterImage[i].sprite = defaultCharacter;
			}
			else
			{
				playerCharacterImage[i].sprite = characterSprites[playerCharacterIndexArray[i]];
			}
			if(playerReadyStatus[i] == 1)
			{
				readyPic[i].SetActive(true);
			}
			else
			{
				readyPic[i].SetActive(false);
			}
		}
	}
	void CheckMapSelect()
	{
		mapSelectImage.sprite = mapSprites[mapIndex];
	}

	void CheckTeamStatus()
	{
		int count = 0;
		for(int i = 0; i < 4; i++)
		{
			if(playerTeamStatus[i] == 1){
				count++;
				teamText[i].text = "Team1";
			}
			else if(playerTeamStatus[i] == 2)
			{
				teamText[i].text = "Team2";
			}
			else
			{
				teamText[i].text = "";
			}
		}
		team1Number = count;
	}

	void Awake()
	{
		_instance = this;
		playerCharacterImage = new Image[4];
		for(int i = 0; i < 4; i++){
			playerCharacterImage[i] = playerCharacter[i].GetComponent<Image>();
		}
		characterSelectImage = characterSelect.GetComponent<Image>();
		mapSelectImage = mapSelect.GetComponent<Image>();
		for(int i = 0; i < 4; i++)
		{
			playerCharacterImage[i].sprite = defaultCharacter;
		}
		characterSelectImage.sprite = characterSprites[0];
		mapIndex = 0;
		playerCharacterIndexArray = new int[4];
		playerReadyStatus = new int[4];
		playerTeamStatus = new int[4];
		for(int i = 0; i < 4; i++){
			playerCharacterIndexArray[i] = -1;
		}
	}

	void Start () {
		//TODO:进入房间，将4个玩家的人物显示出来
		//如果之前有玩家进入，默认初始化玩家的头像
		/*
		while(playerTotal >= 0)
		{
			P[playerTotal].GetComponent<Image>().sprite = sprites[0];
			playerTotal--;
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {

		if(playerIndex == 0)
		{
			goBtn.SetActive(true);
			readyBtn.SetActive(false);
		}
		else
		{
			readyBtn.SetActive(true);
			goBtn.SetActive(false);
		}
		
		CheckPlayerStatus();
		CheckMapSelect();
		CheckTeamStatus();

		if(async == null)
		{
			return;
		}
		int toProcess;
		if(async.progress < 0.9f)
		{
			toProcess = (int)async.progress * 100;
		}
		else
		{
			toProcess = 100;
		}
		if(nowProcess < toProcess)
		{
			nowProcess++;
		}
		processBar.value = nowProcess / 100f;
		if(nowProcess == 100)
		{
			async.allowSceneActivation = true;
		}
	}

	//改变制定位置的立绘图像
	//右选人按钮的点击事件
	public void OnCharRightBtnDown()
	{
		picIndex = (picIndex + 1) % 4;
		characterSelectImage.sprite = characterSprites[picIndex];
		SetPlayerCharacter(playerIndex, picIndex);
		TcpClient_All._instance.SendCharacterSelectCommand(playerIndex, picIndex);

	}

	//左选人按钮的点击事件
	public void OnCharLeftBtnDown()
	{
		picIndex = (picIndex + 3) % 4;
		characterSelectImage.sprite = characterSprites[picIndex];
		SetPlayerCharacter(playerIndex, picIndex);
		TcpClient_All._instance.SendCharacterSelectCommand(playerIndex, picIndex);
	
	}

	//TODO:地图按钮
	
	public void OnMapLeftBtnDown()
	{
		mapIndex = (mapIndex + 1)%2;
		SetMap(mapIndex);
		TcpClient_All._instance.SendMapSelectCommand(mapIndex);
	}

	public void OnMapRightBtnDown()
	{
		mapIndex = (mapIndex + 1)%2;
		SetMap(mapIndex);
		TcpClient_All._instance.SendMapSelectCommand(mapIndex);
	}

	public void OnTeamOneButton()
	{
		if(!isTeam1)
		{
			isTeam1 = true;
			team1Number++;
		}
		SetPlayerTeam(playerIndex, 1);
		TcpClient_All._instance.SendTeamCommand(playerIndex, 1);
	}

	public void OnTeamTwoButton()
	{
		if(isTeam1)
		{
			isTeam1 = false;
			team1Number--;
		}
		SetPlayerTeam(playerIndex,2);
		TcpClient_All._instance.SendTeamCommand(playerIndex,2);
	}
	

	//当除房主外另外3个玩家准备完毕后，开始按钮点击事件
	/* 
	public void OnGoBtnDown(int sceneIndex)
	{
		loadingPicture.SetActive(true);
		processBar.gameObject.SetActive(true);
		StartCoroutine(LoadGame(sceneIndex));
	}
	*/

	//退出按钮的点击事件
	public void OnExitBtnDown()
	{
		SceneManager.LoadScene("StartMenu");
		TcpClient_All._instance.enabled = false;
	}

	//准备按钮的点击事件
	public void OnReadyBtnDown()
	{
		playerReadyStatus[playerIndex] = 1;
		readyCount++;
		isReady = true;
		TcpClient_All._instance.SendReadyCommand(playerIndex);
	}

	IEnumerator LoadGame(int sceneIndex)
	{
		async = SceneManager.LoadSceneAsync(sceneIndex);
		async.allowSceneActivation = false;
		yield return async;
	}
}