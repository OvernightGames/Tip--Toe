using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController instance;


	private GameObject player;
	private GameObject toy;
	private GameObject enemy;
	
	public bool hasToy = false;

	// Use this for initialization
	void Start () 
	{
		if(instance == null)
			instance = this;
		else 
			Destroy(this.gameObject);
	

		player = GameObject.FindGameObjectWithTag("Player");
		toy = GameObject.FindGameObjectWithTag("Toy");
		enemy = GameObject.FindGameObjectWithTag("Enemy");
	}
	void OnApplicationQuit() 
	{
		PlayerPrefs.SetInt("HasPlayed",0);
	}
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("StartMenu");


		if(Vector3.Distance(this.transform.position, toy.transform.position) < 1f)
		{
			////// WE WIN CODE GOES HERE //////	
			Debug.Log("Yoda we have won the game.");	
			SceneManager.LoadScene("GameWin");
		}
		if(Vector3.Distance(enemy.transform.position, player.transform.position) < 1.5f  || Vector3.Distance(enemy.transform.position, toy.transform.position) < 1.5f)
		{
			PlayerPrefs.SetInt("HasPlayed",1);
			////// WE LOSE CODE GOES HERE /////
			/// 
			SceneManager.LoadScene("TestOrlando");
			Debug.Log("Yoda we have lost the game.");
		}
		if(Vector3.Distance(toy.transform.position, player.transform.position) < 1.25f && !hasToy)
		{
			Debug.Log("Yoda we have the toy lets go!");
			toy.transform.SetParent(player.transform);
			toy.transform.localPosition = new Vector3(-0.154698f,-0.4f,0.483082f);
			toy.transform.localRotation = new Quaternion(-0.04239476f,0.9174203f,0.1017217f,0.382355f);
			hasToy = true;
		}
	}




/*
	TODO
	ADD ENEMY ANIMATIONS 
	ADD DISTANCE CODE TO ENEMY FOR AUDIO REACTIONS
	PROGRAM AUDIO TRIGGERS 
	PROGRAM BUTTONS FOR MENUS
	POLISH ROTATION FOR PLAYER GIVE HIM SOME HEAD ROTATION 
		
*/
}
