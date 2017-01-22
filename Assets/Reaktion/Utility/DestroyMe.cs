using UnityEngine;
using System.Collections;

public class DestroyMe : MonoBehaviour {

	void OnEnable()
	{
		StartCoroutine(WaitToDie());
	}

	IEnumerator WaitToDie()
	{
		yield return new WaitForSeconds(5);
		Destroy(this.gameObject);
	}
}
