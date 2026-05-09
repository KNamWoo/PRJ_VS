using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AudioSettingsUI : MonoBehaviour
{
	[SerializeField]
	Slider masterSlider;
	[SerializeField]
	Text masterText;
	
	[SerializeField]
	Slider bgmSlider;
	[SerializeField]
	Text bgmText;
	
	[SerializeField]
	Slider sfxSlider;
	[SerializeField]
	Text sfxText;
	
	[SerializeField]
	Slider uiSlider;
	[SerializeField]
	Text uiText;

	InputAction       cancelAct;
	public GameObject mainUI;
	public GameObject settingUI;
	
	AudioSettingsManager audioMan;
	
	private void Start()
	{
		cancelAct = InputSystem.actions.FindAction("Cancel");
		masterSlider.onValueChanged.AddListener(OnMasterChanged);
		bgmSlider.onValueChanged.AddListener(OnBGMChanged);
		sfxSlider.onValueChanged.AddListener(OnSFXChanged);
		uiSlider.onValueChanged.AddListener(OnUIChanged);

		SyncSliders();
	}

	void LateUpdate()
	{
		if(cancelAct.IsPressed())
		{
			CancelBtn();
			mainUI.SetActive(true);
			settingUI.SetActive(false);
		}
	}

	private void OnEnable()
	{
		SyncSliders();
	}

	private bool TryEnsureAudioManager()
	{
		if(audioMan != null)
			return true;
		
		audioMan = AudioSettingsManager.instance;
		
		if(audioMan == null)
		{
			Debug.LogWarning("[AudioSettingsUI] AudioSettingsManager instance is null!");
			return false;
		}
		return true;
	}
	
	private void Awake()
	{
		TryEnsureAudioManager();
	}

	private void SyncSliders()
	{
		if(!TryEnsureAudioManager())
			return;
		
		masterSlider.SetValueWithoutNotify(audioMan.MasterVolume);
		masterText.text = $"{(int)(audioMan.MasterVolume * 100)}";
		bgmSlider.SetValueWithoutNotify(audioMan.BGMVolume);
		bgmText.text = $"{(int)(audioMan.BGMVolume * 100)}";
		sfxSlider.SetValueWithoutNotify(audioMan.SFXVolume);
		sfxText.text = $"{(int)(audioMan.SFXVolume * 100)}";
		uiSlider.SetValueWithoutNotify(audioMan.UIVolume);
		uiText.text = $"{(int)(audioMan.UIVolume * 100)}";
	}

	private void OnMasterChanged(float value)
	{
		audioMan.SetMasterVolume(value);
		masterText.text = $"{(int)(value * 100)}";
	}
	
	private void OnBGMChanged(float value)
	{
		audioMan.SetBGMVolume(value);
		bgmText.text = $"{(int)(value * 100)}";
	}

	private void OnSFXChanged(float value)
	{
		audioMan.SetSFXVolume(value);
		sfxText.text = $"{(int)(value * 100)}";
	}

	private void OnUIChanged(float value)
	{
		audioMan.SetUIVolume(value);
		uiText.text = $"{(int)(value * 100)}";
	}

	public void SaveBtn()
	{
		audioMan.SaveSettings();
	}

	public void CancelBtn()
	{
		audioMan.CancelSettings();
		SyncSliders();
	}
}