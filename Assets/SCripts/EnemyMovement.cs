using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{

	public AudioSource feetSource;
	public AudioSource headSource;
	public AudioClip[] clips;

	public GameObject audioObject;

	private GameObject player;
	private Vector3 waypoint;
	private Animator animator;

	private float wanderTimer = 0;
	private float moveTimer = 0;
	private float maxWanderTime = 3;
	private float moveSpeed = .75f;
	private float minDist = 3f;

	private float roarTimer = 0;
	private float maxRoarTimer = 5;
	private float growlTimer = 0;
	private float maxGrowlTimer = 5;


	public float maxDist = 5;
	private int wanderMoves = 0;
	private int wanderMaxMoves = 3;
	private int randRoarSound = 0;
	private int randGrowlSound = 0;

	private bool canMove = false;
	private bool forceFollow = false;
	private bool isRoaring = false;
	private bool canRoar = false;
	private bool isGrowling = false;
	private bool canGrowl = false;
	private bool isBreathing = false;


	void OnEnable()
	{
		player = GameObject.FindGameObjectWithTag("Player") as GameObject;
		animator = this.gameObject.GetComponent<Animator>();

	}

	void FixedUpdate()
	{
		Move();

		if(!isGrowling && !isRoaring  && !isBreathing)
		{
			isBreathing = true;
			headSource.clip = clips[1];
			headSource.Play();
			headSource.loop = true;
		}
		else
		{
			headSource.loop = false;
		}

		if(canRoar)
		{
			if(roarTimer > maxRoarTimer && !isRoaring)
			{
				randRoarSound = Random.Range(5,6);
				headSource.clip = clips[randRoarSound];
				headSource.Play();
				StartCoroutine(WaitToRoar(clips[randRoarSound].length));
				isRoaring = true;
				isBreathing = false;
				canRoar = false;
			}
			else
			{
				roarTimer += Time.deltaTime;
			}
		}

		if(canGrowl)
		{
			if(growlTimer > maxGrowlTimer  && !isGrowling)
			{
				randGrowlSound = Random.Range(3,4);
				headSource.clip = clips[randGrowlSound];
				headSource.Play();
				StartCoroutine(WaitToGrowl(clips[randGrowlSound].length));
				isGrowling = true;
				isBreathing =false;
				canGrowl = false;
			}
			else
			{
				growlTimer += Time.deltaTime;
			}
		}
	}

	void Move()
	{
		if(wanderMoves >= wanderMaxMoves)
		{
			forceFollow = true;
			moveTimer = 0;
			wanderMoves = 0;
		}

		if(moveTimer <= maxWanderTime)
			moveTimer += Time.deltaTime;
		else
			forceFollow = false;

		if(Vector3.Distance(this.transform.position,player.transform.position) <= minDist)
		{
			animator.SetBool("isAttacking", true);
			animator.SetBool("isWalking", false);
			feetSource.Stop();
			canRoar = true;
		}
		else
		{
			animator.SetBool("isAttacking", false);
		}

		if(GameController.instance.hasToy || forceFollow ||Vector3.Distance(this.transform.position,player.transform.position) <= 3 )
		{

			Vector3 direction = player.transform.position - this.transform.position;
			Quaternion rot = Quaternion.LookRotation(direction);
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, 3 * Time.deltaTime);
			this.transform.position += transform.forward * moveSpeed * Time.deltaTime;
			animator.SetBool("isWalking", true);
			canRoar = true;
			feetSource.Play();

			if(animator.GetBool("isAttacking"))
				animator.SetBool("isAttacking", false);
		}
		else
		{
			if(wanderTimer < maxWanderTime)
			{
				wanderTimer += Time.deltaTime;

				if(Vector3.Distance(this.transform.position,waypoint) >= 2 )
				{
					Vector3 direction = waypoint - this.transform.position;
					Quaternion rot = Quaternion.LookRotation(direction);
					this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, 3 * Time.deltaTime);
					this.transform.position += transform.forward * moveSpeed * Time.deltaTime;
					animator.SetBool("isWalking", true);
					canGrowl = true;
					feetSource.Play();

					if(animator.GetBool("isAttacking"))
						animator.SetBool("isAttacking", false);
				}
				else
				{
					animator.SetBool("isWalking", false);
					feetSource.Stop();
				}
			}
			else 
			{
				wanderTimer = 0;
				wanderMoves++;
				waypoint = new Vector3(Random.Range(-15,15),1,Random.Range(-13,13));
				animator.SetBool("isWalking", false);
				feetSource.Stop();
				canRoar = true;
			}

		}
		this.transform.position = new Vector3(this.transform.position.x, .1f, this.transform.position.z);
		this.transform.localEulerAngles = new Vector3(0, this.transform.localEulerAngles.y, 0);
	}


	IEnumerator WaitToGrowl(float timer)
	{
		yield return new WaitForSeconds(timer);
		growlTimer = 0;
		isGrowling = false;
		canGrowl = false;
	}

	IEnumerator WaitToRoar(float timer)
	{
		yield return new WaitForSeconds(timer);
		roarTimer =0;
		isRoaring =false;
		canRoar = false;
	}
}