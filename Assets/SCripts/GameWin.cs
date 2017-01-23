using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameWin : MonoBehaviour {

	// Use this for initialization
	void Awake () 
	{
		((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
		StartCoroutine(WaitForMovie());
	}

	IEnumerator WaitForMovie()
	{
		yield return new WaitForSeconds(25);
		SceneManager.LoadScene("Intro");
	}
}
