using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour 
{
	public AudioClip[] clips;
	public AudioSource feetSource;

	public AudioSource headSource;
	public GameObject audioObject;

	private GameObject enemy;
	private EnemyMovement enemyController;

	private float idleTimer = 0;
	private float maxIdleTimer = 9;
	private float screamTimer = 3;
	private float maxScreamTimer = 4;
	private float randomTimer =0;
	private float maxRandTimer = 5;



	private bool isMoving = false;
	private bool isHumming = false;
	private bool isScreaming = false;
	private bool isTalking = false;
	private bool isWithToy = false;
	public bool canMove = true;


	private int randomTrackNum = 0;
	// Use this for initialization
	void Awake () 
	{
			enemy = GameObject.FindGameObjectWithTag("Enemy");
			enemyController = enemy.GetComponent<EnemyMovement>();

		if(PlayerPrefs.GetInt("HasPlayed") == 1)
		{
			GameObject clone = Instantiate(audioObject, this.transform.position, Quaternion.identity) as GameObject;
			clone.GetComponent<AudioSource>().clip = clips[2];
			clone.GetComponent<AudioSource>().Play();

		}
		canMove = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		Move();

		CheckForScream();

		if(!isScreaming && !isTalking)
			CheckForHumming();

		if(!isScreaming && !isMoving && !isTalking)
			CheckForRandomClip();

		if(Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("Toy").transform.position) < 1f  && !isWithToy)  
		{
			isWithToy = true;
			headSource.clip = clips[8];
			headSource.Play();
		}
	}

	void CheckForRandomClip()
	{
		if(randomTimer > maxRandTimer)
		{

			isTalking = true;
			randomTrackNum = Random.Range(1,5) + 8;
			headSource.clip = clips[randomTrackNum];
			headSource.Play();

			StartCoroutine(WaitForRandom());
		}
		else
			randomTimer += Time.deltaTime;
	}

	void CheckForScream()
	{


		if(screamTimer > maxScreamTimer  && !isScreaming)
		{
			float distance = Vector3.Distance(enemy.transform.position, this.transform.position);
			float playerAngle =Vector3.Angle(this.transform.forward,(this.transform.position - enemy.transform.position)); 


			if(distance < 5 && playerAngle > 150)
			{
				Debug.Log("Yoda we screamed we saw the monster!");
				headSource.volume = 1;
				isScreaming = true;
				headSource.clip = clips[4];
				headSource.Play();
				StartCoroutine(WaitForScream());
			}
		}
		else
		{
			screamTimer += Time.deltaTime;
		}
	}

	void CheckForHumming()
	{
		if(!isMoving && idleTimer > maxIdleTimer)
		{
			isHumming = true;
		}
		else if(idleTimer < maxIdleTimer && !isMoving)
		{
			idleTimer += Time.deltaTime;
		}
		else if( isMoving && isHumming)
		{
			idleTimer = 0;
			isHumming = false;
		}


		if(isHumming && headSource.clip!=clips[7])
		{
			headSource.clip = clips[7];
			headSource.Play();
		}
		else if(!isHumming && headSource.clip != clips[5]  && Vector3.Distance(enemy.transform.position, this.transform.position) > enemyController.maxDist)
		{
			idleTimer = 0;
			headSource.clip = clips[5];
			headSource.Play();
		}
		else if(!isHumming && headSource.clip != clips[6]  && Vector3.Distance(enemy.transform.position, this.transform.position) < enemyController.maxDist)
		{
			idleTimer = 0;
			headSource.clip = clips[6];
			headSource.Play();
		}
			
	}

	void Move()
	{

		if(Input.GetAxis("Vertical") > .1f  && canMove)
		{
			this.transform.Translate(Vector3.forward * 1 * Time.deltaTime);
			isMoving = true;
			// play audio clip walk
			if(!feetSource.isPlaying)
				feetSource.Play();
		}
		else if(Input.GetAxis("Vertical") < -.1f && canMove)
		{
			this.transform.Translate(Vector3.back * 1 * Time.deltaTime);

			isMoving = true;
			// play audio clip walk
			if(!feetSource.isPlaying)
				feetSource.Play();
		}
		else
		{
			isMoving = false;

			if(feetSource.isPlaying)
				feetSource.Stop();
		}

		float mouseInput = Input.GetAxis("Horizontal");
		Vector3 lookhere = new Vector3(0,mouseInput,0);
		transform.Rotate(lookhere);
	}

	IEnumerator WaitForScream()
	{
		yield return new WaitForSeconds(clips[4].length);
		screamTimer = 0;
		isScreaming = false;
		headSource.volume = .05f;
	}
	
	IEnumerator WaitForRandom()
	{
		yield return new WaitForSeconds(clips[randomTrackNum].length);
		randomTimer = 0;
		isTalking = false;
	}
}
