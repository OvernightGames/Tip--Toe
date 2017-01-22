using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{

	public AudioSource feetSource;
	public AudioSource headSource;
	public AudioClip[] clips;


	private GameObject player;
	private Vector3 waypoint;
	private Animator animator;

	private float wanderTimer = 0;
	private float moveTimer = 0;
	private float maxWanderTime = 3;
	private float moveSpeed = .75f;
	private float minDist = 3f;

	public float maxDist = 5;
	private int wanderMoves = 0;
	private int wanderMaxMoves = 3;

	private bool canMove = false;
	private bool forceFollow = false;




	void OnEnable()
	{
		player = GameObject.FindGameObjectWithTag("Player") as GameObject;
		animator = this.gameObject.GetComponent<Animator>();

	}
	void FixedUpdate()
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
			}

		}
		this.transform.position = new Vector3(this.transform.position.x, .1f, this.transform.position.z);
		this.transform.localEulerAngles = new Vector3(0, this.transform.localEulerAngles.y, 0);
	}
}