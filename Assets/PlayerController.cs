using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private bool _isActive;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private OrbManager _orbManager;
    [SerializeField] private Collider _collider;

    [SerializeField] private float _currentYRotation;

    [SerializeField] private Animator _paperAnim;
    [SerializeField] private Transform _paperTransform;

    private static readonly int IsHorizontal = Animator.StringToHash("IsHorizontal");
    private static readonly int HorizontalInput = Animator.StringToHash("HorizontalInput");
    private static readonly int VerticalInput = Animator.StringToHash("VerticalInput");


    void Awake()
    {
        _orbManager = GetComponentInChildren<OrbManager>();
        _collider = GetComponent<Collider>();
    }

    
    void Update()
    {
        if (!_isActive)
            return;

        if(Input.GetButtonDown("Action"))
            _orbManager.StartAttack();

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        Move(new Vector2(xInput, yInput).normalized);
    }

    void Move(Vector2 inputDirection)
    {
    
        _rigidbody.velocity = transform.forward * inputDirection.y * _speed +
                              transform.right * inputDirection.x * _speed;

        UpdatePaperAnim(inputDirection);
    }

    void UpdatePaperAnim(Vector2 inputDirection)
    {
        if(Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
            _paperAnim.SetBool(IsHorizontal, true);

        if (Mathf.Abs(inputDirection.x) < Mathf.Abs(inputDirection.y))
            _paperAnim.SetBool(IsHorizontal, false);

        _paperAnim.SetFloat(HorizontalInput, Mathf.Abs(inputDirection.x));
        _paperAnim.SetFloat(VerticalInput, Mathf.Abs(inputDirection.y));

       int scaleX = inputDirection.x < 0 ? 1 : -1;
       int scaleZ = inputDirection.y < 0 ? 1 : -1;
       if(inputDirection != Vector2.zero) 
           _paperTransform.localScale = new Vector3(scaleX, 1, scaleZ);
        
    }

  

    public void Disappear()
    {
        _paperTransform.gameObject.SetActive(false);
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        _orbManager.HideOrbs(true);
    }

    public void Reappear()
    {
        _paperTransform.gameObject.SetActive(true);
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
        _orbManager.HideOrbs(false);
        ToggleIsActive();
    }

    public void ResetRotation()
    {
        transform.eulerAngles = new Vector3(0, _currentYRotation, 0);
    }

    [Button]
    public void ToggleIsActive()
    {
        _isActive = !_isActive;
    }

    public void TakeDamage()
    {
        Debug.Log("Ow!");
    }
}
