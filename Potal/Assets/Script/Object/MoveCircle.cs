using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCircle : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] private Vector3 startPos;
	[SerializeField] private Vector3 startRot;
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private Rigidbody rb;
	void Start()
    {
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
		startRot = transform.rotation.eulerAngles;
		Init();
	}

	public void OnCollisionEnter(Collision collision)
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		Quaternion flip = Quaternion.LookRotation(-rb.transform.forward, rb.transform.up);
		//Quaternion flip = Quaternion.Euler(Vector3.forward * 180f);
		rb.rotation = flip;

		rb.velocity = -rb.transform.forward * moveSpeed;

		if (collision.gameObject.TryGetComponent(out CapsuleCollider player))
		{
			Init();
			//죽었습니다.
		}
	}
	private void Init()
	{
		rb.isKinematic = true;
		transform.position = startPos;
		rb.isKinematic = false;
		rb.velocity = Vector3.zero;
		rb.rotation = Quaternion.Euler(startRot);
		rb.velocity = rb.rotation * Vector3.forward * moveSpeed;
	}
}
