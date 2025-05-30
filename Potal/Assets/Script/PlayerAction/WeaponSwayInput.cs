using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class WeaponSwayInput : MonoBehaviour
{
    [SerializeField] private float swayAmount = 0.05f;
    [SerializeField] private float maxSway = 0.1f;
    [SerializeField] private float smoothSpeed = 10f ;

    private Vector2 _lookDelta;

    private Vector3 _position;
    private void Start()
    {
        _position = transform.localPosition;
    }

    private void Update()
    {
        float swayX = Mathf.Clamp(-_lookDelta.x * swayAmount, -maxSway, maxSway);
        float swayY = Mathf.Clamp(-_lookDelta.y * swayAmount, -maxSway, maxSway);

        Vector3 pos = _position + new Vector3(swayX, swayX, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, Time.deltaTime * smoothSpeed);
    }

    public void OnWeaponLook(InputAction.CallbackContext context)
    {
        _lookDelta = context.ReadValue<Vector2>();
    }
}
