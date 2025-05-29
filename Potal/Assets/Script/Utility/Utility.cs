using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Utility : MonoBehaviour
{
	Button button;
	public static void ButtonBind(Button button, Action action)
	{
		button.onClick.AddListener(() => action());
		button.onClick.AddListener(() => AudioManager.Instance.SFXSourceButton.Play());
		//button.OnPointerEnter(()=>AudioManager.Instance.SFXSourceButton.Play());
		//button.transition.
		// 3. Hover 사운드 (EventTrigger를 통해 동적으로 등록)
		EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
		if (trigger == null)
			trigger = button.gameObject.AddComponent<EventTrigger>();

		var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
		entry.callback.AddListener((BaseEventData _) =>
		{
			if (button.transition == Selectable.Transition.ColorTint)
			{
				AudioManager.Instance.SFXSourceButtonHover.Play();
			}
		});
		trigger.triggers.Add(entry);
	}
}
