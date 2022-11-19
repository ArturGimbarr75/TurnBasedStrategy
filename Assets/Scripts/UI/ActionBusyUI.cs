using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += OnBusyChanged;
        gameObject.SetActive(false);
    }

    private void OnBusyChanged(object sender, bool isBusy)
    {
        gameObject.SetActive(isBusy);
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnBusyChanged -= OnBusyChanged;
    }
}
