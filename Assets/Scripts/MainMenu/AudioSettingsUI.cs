/*
    최초 작성일:26/05/10
    최종 변경일:26/05/17
    
    수정자
    - 김남우
    -
    
    목적
    - AudioSettingsManager의 볼륨 조절 기능과 연동하여 AudioSettingsUI의 슬라이더와 텍스트를 동기화하는 로직을 개발
	 또한, Cancel 버튼과 Cancel 입력 액션을 통해 변경 사항을 취소하고 이전 설정으로 되돌리는 기능을 설계
*/

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