using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private bool _isActive;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private Animator _paperAnim;
    [SerializeField] private Transform _paperTransform;
    private PlayerController _playerController;

    private static readonly int IsHorizontal = Animator.StringToHash("IsHorizontal");


    void Awake()
    {
        _collider = GetComponent<Collider>();
        _playerController = SingletonManager.Get<PlayerController>();
    }


    void Update()
    {
        if (!_isActive)
            return;

        TurnTowardsPlayer();
        Move(Vector2.zero);
    }

    void Move(Vector2 inputDirection)
    {

        _rigidbody.velocity = transform.forward * _speed;

        UpdatePaperAnim(inputDirection);
    }

    void UpdatePaperAnim(Vector2 inputDirection)
    {
        if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
            _paperAnim.SetBool(IsHorizontal, true);

        if (Mathf.Abs(inputDirection.x) < Mathf.Abs(inputDirection.y))
            _paperAnim.SetBool(IsHorizontal, false);


        int scaleX = inputDirection.x < 0 ? 1 : -1;
        int scaleZ = inputDirection.y < 0 ? 1 : -1;
        if (inputDirection != Vector2.zero)
            _paperTransform.localScale = new Vector3(scaleX, 1, scaleZ);

    }


    void TurnTowardsPlayer()
    {
        transform.LookAt(_playerController.transform);
    }


    public void Disappear()
    {
        _paperTransform.gameObject.SetActive(false);
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
    }

    public void Reappear()
    {
        _paperTransform.gameObject.SetActive(true);
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
        ToggleIsActive();
    }

    [Button]
    public void ToggleIsActive()
    {
        _isActive = !_isActive;
    }
}
