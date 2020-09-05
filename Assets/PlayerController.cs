using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private bool _isActive;
    [SerializeField] private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive)
            return;

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        Move(new Vector2(xInput, yInput).normalized);
     // Move(new Vector3(xInput, 0, yInput));
    }

    void Move(Vector2 inputDirection)
    {
     //   transform.position += transform.forward * inputDirection.y * _speed * Time.deltaTime + transform.right * inputDirection.x * _speed * Time.deltaTime;
        //_rigidbody.AddForce(transform.forward * inputDirection.y * _speed, ForceMode.VelocityChange);
        //_rigidbody.AddForce(transform.right * inputDirection.x * _speed, ForceMode.VelocityChange);
        _rigidbody.velocity = transform.forward * inputDirection.y * _speed +
                              transform.right * inputDirection.x * _speed;
    }

    void Move(Vector3 inputDirection)
    {
        transform.localPosition += inputDirection * _speed * Time.deltaTime;
    }

    [Button]
    public void ToggleIsActive()
    {
        _isActive = !_isActive;
    }
}
