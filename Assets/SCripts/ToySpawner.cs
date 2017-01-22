using UnityEngine;
using System.Collections;

public class ToySpawner : MonoBehaviour {


	public GameObject[] spawners;

	private GameObject player;
	private bool spawnToy = false;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");

		int spawnNum = Random.Range(0,spawners.Length);
		this.transform.position = spawners[spawnNum].transform.position;
	}


	void Update()
	{
		this.transform.LookAt(player.transform);
	}
}
