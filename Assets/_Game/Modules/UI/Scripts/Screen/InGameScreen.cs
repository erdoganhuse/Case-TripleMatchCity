using Library.ScreenManagement;
using TMPro;
using UnityEngine;

namespace Modules.UI
{
    public class InGameScreenArgs : BaseScreenArgs
    {
        public int LevelNo;
    }
    
    public class InGameScreen : BaseScreen<InGameScreenArgs>
    {
        [SerializeField] private TextMeshProUGUI _levelNoText;
        [SerializeField] private CountdownInfoPanel _countdownInfoPanel;
        [SerializeField] private TargetInfoPanel _targetInfoPanel;
        
        public override BaseScreenAnimationCommand GetAnimationCommand()
        {
            return new InstantScreenAnimationCommand();
        }

        public override void OnSetup()
        {
            _levelNoText.text = $"LEVEL {GetArgs().LevelNo}";
            _countdownInfoPanel.Setup();
            _targetInfoPanel.Setup();
        }

        public override void OnClose()
        {
            _countdownInfoPanel.Clear();
            _targetInfoPanel.Clear();
        }
    }
}