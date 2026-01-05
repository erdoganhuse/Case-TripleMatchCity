using System.Collections.Generic;
using System.Linq;

namespace Library.Utility
{
    public static class ListExtensionMethods
    {
        private static readonly System.Random Random = new();

        public static List<T> Shuffle<T>(this List<T> list)
        {
            return list.OrderBy(a => Random.Next()).ToList();
        }
        
        public static T GetRandom<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        } 
    }
}