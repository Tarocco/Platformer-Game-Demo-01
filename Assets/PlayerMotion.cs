using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour, ISerializationCallbackReceiver
{
    public Rigidbody Rigidbody;
    public float Mobility = 8f;

    public Vector3 Movement { get; set; }
    public bool DebounceFlag
    {
        get; set;
    }
    void Start()
    {

    }

    public void ApplyImpulse(Vector3 impulse)
    {
        Rigidbody.velocity += Rigidbody.transform.InverseTransformVector(impulse);
    }
    void FixedUpdate()
    {
        var velocity = Rigidbody.velocity;
        velocity = Rigidbody.transform.InverseTransformVector(velocity);

        velocity.x = Mathf.Lerp(velocity.x, Movement.x, Time.fixedDeltaTime * Mobility);
        velocity.y += Movement.y;

        velocity = Rigidbody.transform.TransformVector(velocity);
        Rigidbody.velocity = velocity;
    }

    public void OnBeforeSerialize()
    {
        // Null coalescing operator does not work on some
        // built-in component types for some odd reason...

        // Rigidbody = Rigidbody ?? GetComponent<Rigidbody>();

        if (Rigidbody == null)
            Rigidbody = GetComponent<Rigidbody>();
    }

    public void OnAfterDeserialize()
    {
    }
}
