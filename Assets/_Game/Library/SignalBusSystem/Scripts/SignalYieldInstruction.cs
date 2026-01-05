using System;
using UnityEngine;

namespace Library.SignalBusSystem
{
    [Serializable]
    public class SignalYieldInstruction : CustomYieldInstruction {
 
        private bool _shouldContinue;

        public SignalYieldInstruction()
        {
            _shouldContinue = false;
        }
        
        public void Continue()
        {
            _shouldContinue = true;
        }
 
        public override bool keepWaiting => !_shouldContinue;
    }
}