using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private bool _isActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive)
            return;

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Move(new Vector2(xInput, yInput).normalized);
    }

    void Move(Vector2 inputDirection)
    {
        if (inputDirection == Vector2.zero)
            return;

        transform.position += transform.forward * inputDirection.y * _speed * Time.deltaTime + transform.right * inputDirection.x * _speed * Time.deltaTime;
    }

    [Button]
    public void ToggleIsActive()
    {
        _isActive = !_isActive;
    }
}
