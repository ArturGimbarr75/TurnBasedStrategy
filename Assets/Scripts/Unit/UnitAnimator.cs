using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _rifleHandler;
    [SerializeField] private Transform _swordHandler;

    private Weapon _weapon;

    private Unit _unit;
    private MoveAction _moveAction;
    private ShootAction _shootAction;
    private SwordAction _swordAction;

    private void Awake()
    {
        _weapon = GetComponentInChildren<Weapon>();
        _unit = GetComponentInChildren<Unit>();

        if (TryGetComponent(out _moveAction))
        {
            _moveAction.OnStartMoving += OnStartMoving;
            _moveAction.OnStopMoving += OnStopMoving;
        }
        if (TryGetComponent(out _shootAction))
            _shootAction.OnShoot += OnShoot;
        if (TryGetComponent(out _swordAction))
            _swordAction.OnActionStarted += OnSwardActionStarted;

        UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;     
    }

    private void OnSelectedActionChanged(object sender, EventArgs args)
    {
        if (sender is UnitActionSystem uas && uas.SelectedUnit == _unit)
        {
            bool isSwordActionSelected = uas.SelectedAction is SwordAction;
            _rifleHandler.gameObject.SetActive(!isSwordActionSelected);
            _swordHandler.gameObject.SetActive(isSwordActionSelected);
        }
    }

    private void OnSwardActionStarted(object sender, EventArgs args)
    {
        _animator.SetTrigger("SwordSlash");
    }

    private void OnStartMoving(object sender, EventArgs args)
    {
        _animator.SetBool("IsWalking", true);
    }

    private void OnStopMoving(object sender, EventArgs args)
    {
        _animator.SetBool("IsWalking", false);
    }
    
    private void OnShoot(object sender, OnShootEventArgs args)
    {
        _animator.SetTrigger("Shoot");
        Transform bullet = Instantiate(_bulletProjectilePrefab, _weapon.ShootPoint.position, Quaternion.identity);
        Vector3 targetPosition = args.TargetUnit.transform.position;
        targetPosition.y = _weapon.ShootPoint.position.y;
        bullet.GetComponent<BulletProjectile>().Setup(targetPosition);
    }

    private void OnDisable()
    {
        if (_moveAction != null)
        {
            _moveAction.OnStartMoving -= OnStartMoving;
            _moveAction.OnStopMoving -= OnStopMoving;
        }
        if (_shootAction != null)
            _shootAction.OnShoot -= OnShoot;
        if (_swordAction != null)
            _swordAction.OnActionStarted -= OnSwardActionStarted;

        UnitActionSystem.Instance.OnSelectedActionChanged -= OnSelectedActionChanged;
    }
}
