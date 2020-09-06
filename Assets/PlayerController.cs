using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private bool _isActive;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private OrbManager _orbManager;

    [SerializeField] private float _currentYRotation;

   
    void Awake()
    {
        _orbManager = GetComponentInChildren<OrbManager>();
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
    }

  

    public void Disappear()
    {
        GetComponent<Renderer>().enabled = false;
        _orbManager.HideOrbs(true);
    }

    public void Reappear()
    {
        GetComponent<Renderer>().enabled = true;
        _orbManager.HideOrbs(false);
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
}
