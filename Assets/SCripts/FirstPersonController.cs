using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(Input.GetKey(KeyCode.W))
		{
			this.transform.Translate(Vector3.forward * 1 * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.S))
		{
			this.transform.Translate(Vector3.back * 1 * Time.deltaTime);
		}

		/*if(Input.GetKey(KeyCode.A))
		{
			this.transform.Translate(Vector3.left * 1 * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.D))
		{
			this.transform.Translate(Vector3.right * 1 * Time.deltaTime);
		}*/


		//this.transform.localEulerAngles += new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + Input.GetAxis("Horizontal") * 3, this.transform.localEulerAngles.z);
		float mouseInput = Input.GetAxis("Horizontal");
		Vector3 lookhere = new Vector3(0,mouseInput,0);
		transform.Rotate(lookhere);

	}
}
