using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
	// 슬라이더가 특정 값에서 딱 달라붙게 하는 스크립트
	public class StickySnapSlider : MonoBehaviour
	{
		[SerializeField]
		private Slider slider;

		private float step      = 0.1f;
		private float snapEnter = 0.02f; // 해당 수가 넘어가면 다음 수에 붙음
		private float snapExit  = 0.08f; // 해당 수가 넘어가면 떨어짐

		private bool  isSnapped;
		private float snappedValue;
		private bool  internalSet;

		private void Awake()
		{
			slider.onValueChanged.AddListener(OnSliderChanged);
		}

		private void OnSliderChanged(float raw)
		{
			if(internalSet) return;
			float v = ApplyStickySnap(raw);

			if(!Mathf.Approximately(v, raw))
			{
				internalSet = true;
				slider.SetValueWithoutNotify(v); // 이벤트 루프 방지
				internalSet = false;
			}
			// v값은 이 이후에 사용
		}

		private float ApplyStickySnap(float value)
		{
			float nearst = Mathf.Round(value / step) * step;
			float dist = Mathf.Abs(value - nearst);

			if(isSnapped)
			{
				if(Mathf.Abs(value - snappedValue) <= snapExit)
				{
					return snappedValue; // 계속 붙어있음
				}
				isSnapped = false; // 이탈
			}

			if(dist <= snapEnter)
			{
				isSnapped = true;
				snappedValue = nearst;
				return snappedValue; // 새로 붙음
			}
			return value;
		}
	}
}