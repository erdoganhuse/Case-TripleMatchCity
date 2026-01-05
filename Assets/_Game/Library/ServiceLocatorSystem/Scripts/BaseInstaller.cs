using UnityEngine;

namespace Library.ServiceLocatorSystem
{
    public abstract class BaseInstaller : MonoBehaviour
    {
        private void Awake()
        {
            InstallBindings();
        }

        protected abstract void InstallBindings();
    }
}