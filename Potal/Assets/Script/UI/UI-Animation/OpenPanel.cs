using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpenPanel : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float time = 0;
    [SerializeField] private float frame = 60.0f;
    [SerializeField] private float timeRate = 0.0f;

	void OnEnable()
    {
		time = 0;
		transform.localScale = new Vector3(time, 1, 1);
        StartCoroutine(OpenAnimation());
    }

	private IEnumerator OpenAnimation()
    {
        while (time < 1.0f)
        {
            time = time + Time.unscaledDeltaTime;
			float clampedTime = Mathf.Clamp(time, 0, 1);
            transform.localScale = new Vector3(curve.Evaluate(clampedTime), 1, 1);
            yield return null;
        }
	}
}
