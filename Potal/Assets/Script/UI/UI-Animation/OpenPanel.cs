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
        timeRate = 1.0f / frame;
		transform.localScale = new Vector3(time, 1, 1);
        Invoke("OpenAnimation", 0f);
    }
    private void OpenAnimation()
    {
        while (true)
        {
            time = time + Time.deltaTime * timeRate;
            if (1.0 < time)
            {
                time = 1.0f;
                transform.localScale = new Vector3(curve.Evaluate(time), 1, 1);
                return;
            }
            transform.localScale = new Vector3(curve.Evaluate(time), 1, 1);
        }
	}
}
