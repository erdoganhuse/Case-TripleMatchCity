using UnityEngine;

namespace ThirdParty.Other.EditorButton
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class EditorButtonAttribute : PropertyAttribute
    {
        public string name;

        public EditorButtonAttribute(string name = "")
        {
            this.name = name;
        }
    }
}