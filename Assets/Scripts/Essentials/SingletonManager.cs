using Sirenix.Utilities;

namespace Assets.Scripts.Helpers
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class SingletonManager
    {

        private static Dictionary<Type, Object> _singletons;

        static SingletonManager()
        {
            Init();
        }

        public static void Init()
        {
            _singletons = new Dictionary<Type, Object>();
        }

        public static T Get<T>() where T : Object
        {
            T returnVal;
            if (_singletons.TryGetValue(typeof(T), out var val) && val != null)
            {
                returnVal = (T)val;
            }
            else
            {
                var ob = GameObject.FindObjectOfType<T>();
                if (ob == null)
                {
                    throw new Exception("SingletonManager failed lookup: object type does not exist -- " + typeof(T));
                }

                _singletons.Remove(typeof(T));
                _singletons.Add(typeof(T), ob);

                returnVal = ob;
            }

            return returnVal;
        }

    }
}