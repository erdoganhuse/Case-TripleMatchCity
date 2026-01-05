using UnityEngine;

namespace Library.ScreenManagement
{
    public class SampleScreen01Args : BaseScreenArgs
    {
        public string TestString;
    }

    public class SampleScreen01 : BaseScreen<SampleScreen01Args>
    {
        public override void OnInitialize()
        {
            Debug.Log($"{nameof(SampleScreen01)}-{nameof(OnInitialize)}");
        }

        public override void OnSetup()
        {
            Debug.Log($"{nameof(SampleScreen01)}-{nameof(OnSetup)}");
            Debug.Log($"{GetArgs().TestString}");
        }

        public override void OnClear()
        {
            Debug.Log($"{nameof(SampleScreen01)}-{nameof(OnClear)}");
        }

        public override void OnOpen()
        {
            Debug.Log($"{nameof(SampleScreen01)}-{nameof(OnOpen)}");
        }

        public override void OnClose()
        {
            Debug.Log($"{nameof(SampleScreen01)}-{nameof(OnClose)}");
        }

        public override void OnShow()
        {
            Debug.Log($"{nameof(SampleScreen01)}-{nameof(OnShow)}");
        }

        public override void OnHide()
        {
            Debug.Log($"{nameof(SampleScreen01)}-{nameof(OnHide)}");
        }

        public void OnCloseButtonClicked()
        {
            Close();
        }
    }
}