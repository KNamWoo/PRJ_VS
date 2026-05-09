using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
	[SerializeField]
	Slider masterSlider;
	[SerializeField]
	Slider bgmSlider;
	[SerializeField]
	Slider sfxSlider;
	[SerializeField]
	Slider uiSlider;
	
	AudioSettingsManager audioMan;

	void Awake()
	{
		audioMan = AudioSettingsManager.instance;
	}

	private void Start()
	{
		masterSlider.onValueChanged.AddListener(OnMasterChanged);
		bgmSlider.onValueChanged.AddListener(OnBGMChanged);
		sfxSlider.onValueChanged.AddListener(OnSFXChanged);
		uiSlider.onValueChanged.AddListener(OnUIChanged);

		SyncSliders();
	}

	private void OnEnable()
	{
		SyncSliders();
	}

	private void SyncSliders()
	{
		if(audioMan == null)
			return;
		
		masterSlider.SetValueWithoutNotify(audioMan.MasterVolume);
		bgmSlider.SetValueWithoutNotify(audioMan.BGMVolume);
		sfxSlider.SetValueWithoutNotify(audioMan.SFXVolume);
		uiSlider.SetValueWithoutNotify(audioMan.UIVolume);
	}

	private void OnMasterChanged(float value)
	{
		audioMan.SetMasterVolume(value);
	}
	
	private void OnBGMChanged(float value)
	{
		audioMan.SetBGMVolume(value);
	}

	private void OnSFXChanged(float value)
	{
		audioMan.SetSFXVolume(value);
	}

	private void OnUIChanged(float value)
	{
		audioMan.SetUIVolume(value);
	}
}