using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBlock : MonoBehaviour
{
	[SerializeField] private LayerMask layerMask;
	public void OnCollisionEnter(Collision collision)
	{
		if(((1 << collision.gameObject.layer) & layerMask) != 0)
		AudioManager.Instance.SFXSourceFallBlock.Play();
	}
}
