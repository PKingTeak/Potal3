using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				// 씬에서 존재하는 인스턴스 검색
				instance = FindObjectOfType<T>();

				// 없다면 새로 생성
				if (instance == null)
				{
					GameObject singletonObject = new GameObject(typeof(T).Name);
					instance = singletonObject.AddComponent<T>();
					//DontDestroyOnLoad(singletonObject);
				}
			}

			return instance;
		}
	}
}
