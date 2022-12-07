using UnityEngine;

public class EnableDisable : MonoBehaviour
{
    [SerializeField] private bool _invokeOnce = false;
    [SerializeField] private GameObject[] _enableOnInvoke;
    [SerializeField] private GameObject[] _disableOnInvoke;

    private bool _invoked = false;

    private void Start()
    {
        foreach (GameObject go in _enableOnInvoke)
            go?.SetActive(false);

        foreach (GameObject go in _disableOnInvoke)
            go?.SetActive(true);
    }

    public void Invoke()
    {
        if (_invokeOnce && _invoked)
            return;

        foreach (GameObject go in _enableOnInvoke)
            go?.SetActive(true);

        foreach (GameObject go in _disableOnInvoke)
            go?.SetActive(false);

        _invoked = true;
    }
}
