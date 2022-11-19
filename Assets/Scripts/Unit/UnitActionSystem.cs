using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public BaseAction SelectedAction
    {
        get => _selectedAction;
        set
        { 
            _selectedAction = value;
            OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [field: SerializeField] public Unit SelectedUnit { get; private set; }
    [SerializeField] private LayerMask _unitLayerMask;

    private BaseAction _selectedAction;
    private bool _isBusy = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There's more than one {typeof(UnitActionSystem).FullName} " + transform + " - " + Instance);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(SelectedUnit);
    }

    private void Update()
    {
        if (_isBusy)
            return;

        if (!TurnSystem.Instance.IsPlayerTurn)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (TryHandleUnitSelection())
            return;

        HandleSelectedAction();
    }

    private void SetBusy()
    {
        _isBusy = true;
        OnBusyChanged?.Invoke(this, _isBusy);
    }
    
    private void ClearBusy()
    { 
        _isBusy = false;
        OnBusyChanged?.Invoke(this, _isBusy);
    }

    private void HandleSelectedAction()
    {
        if (!InputManager.Instance.IsMouseButtonDownThisFrame())
            return;

        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        if (SelectedAction?.IsValidActionGridPosition(mouseGridPosition) ?? false)
        {
            if (SelectedUnit.TrySpendActionPointsToTakeAction(SelectedAction))
            {
                SetBusy();
                SelectedAction.TakeAction(mouseGridPosition, ClearBusy);
                OnActionStarted?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        if (!InputManager.Instance.IsMouseButtonDownThisFrame())
            return false;

        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _unitLayerMask))
            if (hit.transform.TryGetComponent(out Unit unit) && !unit.IsEnemy)
            {
                if (unit == SelectedUnit)
                    return false;
                SetSelectedUnit(unit);
                return true;
            }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        SelectedUnit = unit;
        SelectedAction = unit.GetActions().FirstOrDefault();

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }
}
