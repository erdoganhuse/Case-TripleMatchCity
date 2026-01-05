using UnityEngine;

namespace Library.ScreenManagement
{
    public class SampleScreen02 : BaseScreen
    {
        public override void OnInitialize()
        {
            Debug.Log($"{nameof(SampleScreen02)}-{nameof(OnInitialize)}");
        }

        public override void OnSetup()
        {
            Debug.Log($"{nameof(SampleScreen02)}-{nameof(OnSetup)}");
        }

        public override void OnClear()
        {
            Debug.Log($"{nameof(SampleScreen02)}-{nameof(OnClear)}");
        }

        public override void OnOpen()
        {
            Debug.Log($"{nameof(SampleScreen02)}-{nameof(OnOpen)}");
        }

        public override void OnClose()
        {
            Debug.Log($"{nameof(SampleScreen02)}-{nameof(OnClose)}");
        }

        public override void OnShow()
        {
            Debug.Log($"{nameof(SampleScreen02)}-{nameof(OnShow)}");
        }

        public override void OnHide()
        {
            Debug.Log($"{nameof(SampleScreen02)}-{nameof(OnHide)}");
        }

        public void OnCloseButtonClicked()
        {
            Close();
        }
    }
}