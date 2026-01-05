using UnityEngine;

namespace Library.ScreenManagement
{
    public class SampleScreen03 : BaseScreen
    {
        public override void OnInitialize()
        {
            Debug.Log($"{nameof(SampleScreen03)}-{nameof(OnInitialize)}");
        }

        public override void OnSetup()
        {
            Debug.Log($"{nameof(SampleScreen03)}-{nameof(OnSetup)}");
        }

        public override void OnClear()
        {
            Debug.Log($"{nameof(SampleScreen03)}-{nameof(OnClear)}");
        }

        public override void OnOpen()
        {
            Debug.Log($"{nameof(SampleScreen03)}-{nameof(OnOpen)}");
        }

        public override void OnClose()
        {
            Debug.Log($"{nameof(SampleScreen03)}-{nameof(OnClose)}");
        }

        public override void OnShow()
        {
            Debug.Log($"{nameof(SampleScreen03)}-{nameof(OnShow)}");
        }

        public override void OnHide()
        {
            Debug.Log($"{nameof(SampleScreen03)}-{nameof(OnHide)}");
        }

        public void OnCloseButtonClicked()
        {
            Close();       
        }
    }
}