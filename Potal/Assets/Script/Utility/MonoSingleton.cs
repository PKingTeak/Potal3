using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance;

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

	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
			// 필요하면 개별 클래스에서 DontDestroyOnLoad 호출
		}
		else if (instance != this)
		{
			Destroy(gameObject); // 중복 인스턴스 제거
		}
	}
}
