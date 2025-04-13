
using UnityEngine;

namespace MH.Singleton
{
    public class Singleton<T>  where T : Singleton<T> 
    {
        private static T _instance;

    }
}
