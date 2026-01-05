using System.Collections.Generic;
using UnityEngine;

namespace Library.Utility
{
    public static class TransformExtensionMethods
    {
        public static List<T> GetComponentsInFirstChildren<T>(this Transform transform, bool includeInactive = false) where T : Component
        {
            T[] components = transform.GetComponentsInChildren<T>(includeInactive);
            List<T> componentsInFirstChildren = new();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].transform.parent == transform)
                {
                    componentsInFirstChildren.Add(components[i]);
                }
            }
            
            return componentsInFirstChildren;
        }

        public static T GetComponentInChildrenWithName<T>(this Transform transform, string name, bool includeInactive = false) where T : Component
        {
            T[] components = transform.GetComponentsInChildren<T>(includeInactive);
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].gameObject.name == name)
                {
                    return components[i];
                }
            }
            
            return null;
        }

        public static void SetPosX(this Transform transform, float posX)
        {
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }

        public static void SetPosY(this Transform transform, float posY)
        {
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }
        
        public static void SetPosZ(this Transform transform, float posZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, posZ);
        }
        
        public static void SetLocalPosX(this Transform transform, float localPosX)
        {
            transform.localPosition = new Vector3(localPosX, transform.localPosition.y, transform.localPosition.z);
        }

        public static void SetLocalPosY(this Transform transform, float localPosY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, localPosY, transform.localPosition.z);
        }
        
        public static void SetLocalPosZ(this Transform transform, float localPosZ)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, localPosZ);
        }
        
        public static void SetLocalScaleX(this Transform transform, float valueX)
        {
            transform.localScale = new Vector3(valueX, transform.localScale.y, transform.localScale.z);
        }

        public static void SetLocalScaleY(this Transform transform, float valueY)
        {
            transform.localScale = new Vector3(transform.localScale.x, valueY, transform.localScale.z);
        }
        
        public static void SetLocalScaleZ(this Transform transform, float valueZ)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, valueZ);
        }
        
        public static void SetAnchorPosX(this RectTransform rectTransform, float posX)
        {
            rectTransform.anchoredPosition = new Vector2(posX, rectTransform.anchoredPosition.y);
        }

        public static void SetAnchorPosY(this RectTransform rectTransform, float posY)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, posY);
        }
        
        public static void SetWidth(this RectTransform rectTransform, float width)
        {
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }
        
        public static void SetHeight(this RectTransform rectTransform, float height)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        }
    }
}