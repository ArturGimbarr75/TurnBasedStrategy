using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonGenerator : MonoBehaviour
{
    [Serializable]
    public struct SceneInfo
    {
        public string Name;
        public int BuildIndex;
    }

    [SerializeField] private ButtonModel _buttonModel;
    [SerializeField] private SceneInfo[] _sceneInfo;

    private void Start()
    {
        foreach (SceneInfo si in _sceneInfo)
        {
            ButtonModel temp = Instantiate(_buttonModel, transform);
            temp.gameObject.SetActive(true);
            int bi = si.BuildIndex;
            temp.Button.onClick.AddListener(() => SceneManager.LoadScene(bi));
            temp.Text.text = si.Name;
        }
    }
}
