using RoaringFangs.Inputs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ArthurController : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    private MonoBehaviour InputBehavior;
    public IInput Input
    {
        get { return (IInput)InputBehavior; }
        set { InputBehavior = (MonoBehaviour)value; }
    }

    [SerializeField]
    private PlayerMotion _PlayerMotion;
    public PlayerMotion PlayerMotion
    {
        get { return _PlayerMotion; }
        set { _PlayerMotion = value; }
    }

    [SerializeField]
    private Animator _Animator;
    public Animator Animator
    {
        get { return _Animator; }
        set { _Animator = value; }
    }

    [SerializeField]
    private BoxCollider _PSTrigger;
    public BoxCollider PSTrigger
    {
        get { return _PSTrigger; }
        set { _PSTrigger = value; }
    }

    [SerializeField]
    private LayerMask _PSLayerMask;
    public LayerMask PSLayerMask
    {
        get { return _PSLayerMask; }
        set { _PSLayerMask = value; }
    }

    [SerializeField]
    private GameObject _AirColliders;

    public GameObject AirColliders
    {
        get { return _AirColliders; }
        set { _AirColliders = value; }
    }

    [SerializeField]
    private Transform _CollidersRoot;

    public Transform CollidersRoot
    {
        get { return _CollidersRoot; }
        set { _CollidersRoot = value; }
    }

    public float MovementSpeed = 1.0f;

    public float JumpSpeed = 1.0f;

    public float JumpHoldSpeed = 1.0f;

    public float HorizontalJumpSpeed = 1.0f;

    public AnimationCurve JumpHold = AnimationCurve.Linear(0.0f, 1.0f, 2.0f, 0.0f);

    private float JumpHoldTime;

    private float MoveY;

    void Start()
    {

    }

    private static Collider[] OverlapBoxCollider(
        BoxCollider collider,
        int layer_mask = -1,
        QueryTriggerInteraction qti = default(QueryTriggerInteraction))
    {
        var bounds = collider.bounds;
        var rot = collider.transform.rotation;
        return Physics.OverlapBox(
            bounds.center,
            0.5f * bounds.extents,
            rot, layer_mask,
            qti);
    }

    private void SetCollidersEnabled(bool enabled)
    {
        foreach (Transform collider in CollidersRoot)
            collider.gameObject.SetActive(enabled);
    }

    public void OnJump()
    {
        var move_x = Input.GetAxis("Horizontal");

        JumpHoldTime = Time.time;
        PlayerMotion.ApplyImpulse(new Vector3(HorizontalJumpSpeed * move_x, JumpSpeed, 0.0f));
        SetCollidersEnabled(false);
        AirColliders.gameObject.SetActive(true);
        Animator.ResetTrigger("Jump");
    }

    void Update()
    {
        float move_x, move_y;
        if (Input != null)
        {
            move_x = Input.GetAxis("Horizontal");
            move_y = Input.GetAxis("Vertical");
        }
        else
        {
            move_x = 0;
            move_y = 0;
        }

        //var move_dy = move_y - MoveY;
        var move_abs_dy = move_y - Mathf.Abs(MoveY);

        MoveY = move_y;

        var movement = new Vector3(MovementSpeed * move_x, 0.0f, 0.0f);

        Animator.SetFloat("HMovement", move_x);
        Animator.SetFloat("Walking Speed", Mathf.Abs(move_x));

        var ps_overlap = OverlapBoxCollider(PSTrigger, PSLayerMask);
        var physically_supported = ps_overlap.Any();

        if(physically_supported)
        {
            SetCollidersEnabled(true);
            if (move_abs_dy > 0.01f)
            {
                // Animator calls OnJump via animation clip
                Animator.SetTrigger("Jump");
            }
        }

        if (move_y < 0.01f)
            JumpHoldTime = -99999;

        movement.y = JumpHoldSpeed * JumpHold.Evaluate(Time.time - JumpHoldTime);

        PlayerMotion.Movement = movement;

        Animator.SetBool("Physically Supported", physically_supported);
    }

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
        Input = Input;

        PlayerMotion = PlayerMotion ?? GetComponent<PlayerMotion>();

        if (Animator == null)
            Animator = GetComponent<Animator>();

        if (CollidersRoot == null)
            CollidersRoot = transform.Find("Colliders");
    }
}
