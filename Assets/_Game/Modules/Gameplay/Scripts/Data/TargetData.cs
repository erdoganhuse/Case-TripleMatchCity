using System;
using UnityEngine;

namespace Modules.Gameplay.Data
{
    [Serializable]
    public class TargetData
    {
        public int ItemId;
        public int ItemAmount;
        public Sprite Icon;
        public float CollectScale;
    }
}