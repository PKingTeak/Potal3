using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCircle : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] private Vector3 startPos;
    [SerializeField] private float moveSpeed = 5f;
	[SerializeField] private Rigidbody rb;
	void Start()
    {
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
		Init();
	}

	public void OnCollisionEnter(Collision collision)
	{
		Quaternion flip = Quaternion.AngleAxis(180f, transform.up);
		rb.rotation = flip * rb.rotation;

		rb.velocity = rb.rotation * Vector3.forward * moveSpeed;

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
		rb.rotation = Quaternion.Euler(Vector3.zero);
		rb.velocity = Vector3.forward * moveSpeed;
	}
}
