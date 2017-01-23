using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenu : MonoBehaviour {

	public void OnNewGameClicked()
	{
		StartCoroutine(WaitToStartGame());
	}

	public void OnQuitClicked()
	{
		StartCoroutine(WaitForEnd());
	}

	IEnumerator WaitToStartGame()
	{
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene("TestOrlando");
	}

	IEnumerator WaitForEnd()
	{
		yield return new WaitForSeconds(3);
		Application.Quit();
	}
}
