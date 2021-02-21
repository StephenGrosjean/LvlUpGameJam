using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject optionsMenu;

	public void StartGame(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void Options()
	{
		gameObject.SetActive(false);
		optionsMenu.SetActive(true);
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
