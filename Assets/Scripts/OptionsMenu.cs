using System;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
	[SerializeField] private GameObject mainMenu;
	[SerializeField] private AudioMixer mixer;

	public void Back()
	{
		gameObject.SetActive(false);
		mainMenu.SetActive(true);
	}

	public void SetLevelMaster(float level)
	{
		mixer.SetFloat("Master", Mathf.Log10(level) * 20);
	}

	public void SetLevelMusic(float level)
	{
		mixer.SetFloat("Music", Mathf.Log10(level) * 20);
	}

	public void SetLevelSFX(float level)
	{
		mixer.SetFloat("SFX", Mathf.Log10(level) * 20);
	}
}
