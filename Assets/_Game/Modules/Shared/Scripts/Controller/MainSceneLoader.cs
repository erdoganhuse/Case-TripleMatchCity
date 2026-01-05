using Library.CoroutineSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Modules.Shared
{
    public class MainSceneLoader : MonoBehaviour
    {
        [SerializeField] private float _loadDelay;

        private void Start()
        {
            CoroutineManager.DoAfterGivenTime(_loadDelay, () =>
            {
                SceneManager.LoadScene(SceneNames.Main, LoadSceneMode.Single);
            });
        }
    }
}