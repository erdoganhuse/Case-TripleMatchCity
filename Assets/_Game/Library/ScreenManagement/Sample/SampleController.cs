using System.Collections;
using Library.CoroutineSystem;
using UnityEngine;

namespace Library.ScreenManagement
{
    public class SampleController : MonoBehaviour
    {
        [SerializeField] private ScreenManager _screenManager;
        [SerializeField] private SampleScreen03 _sampleScreen03;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2f);
            
            _screenManager.Register(_sampleScreen03);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OpenSampleScreen01();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OpenSampleScreen02();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                OpenSampleScreen03();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CoroutineManager.BeginCoroutine(OpenSampleScreen03_Alt());
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                CoroutineManager.BeginCoroutine(CloseSampleScreen03_Alt());
            }            
            
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                _screenManager.CloseAll();
            }
        }

        private void OpenSampleScreen01()
        {
            _screenManager.Open<SampleScreen01>(new SampleScreen01Args()
            {
                OnOpened = () => { Debug.Log("OpenSampleScreen01 OnOpened"); },
                OnClosed = () => { Debug.Log("OpenSampleScreen01 OnClosed"); },
                TestString = "Test String"
            });
        }

        private void OpenSampleScreen02()
        {
            _screenManager.Open<SampleScreen02>();
        }

        private void OpenSampleScreen03()
        {
            _screenManager.Open<SampleScreen03>();
        }

        private IEnumerator OpenSampleScreen03_Alt()
        {
            ScreenOperationOption option = new ScreenOperationOption();
            option.SetHideUnderneath(false);            
            option.SetAnimateBg(false);
            
            option.OnComplete(() =>
            {
                Debug.Log("OpenSampleScreen03_Alt OnComplete");
            });
            
            _screenManager.Open<SampleScreen03>(default, option);
            yield return option.WaitForCompletion();
            Debug.Log("OpenSampleScreen03_Alt WaitForCompletion");
        }        
        
        private IEnumerator CloseSampleScreen03_Alt()
        {
            ScreenOperationOption option = new ScreenOperationOption();
            option.SetAnimateBg(false);
            
            _screenManager.Close<SampleScreen03>(option);

            yield return option.WaitForCompletion();

            Debug.Log("CloseSampleScreen03_Alt WaitForCompletion");
            
            _screenManager.Open<SampleScreen02>(default, new ScreenOperationOption().SetAnimateBg(false));
        }
    }
}