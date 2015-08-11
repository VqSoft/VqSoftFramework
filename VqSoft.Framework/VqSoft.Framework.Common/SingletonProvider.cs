using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VqSoft.Framework.Common
{
    public class SingletonProvider<T> where T : new()
    {
        private SingletonProvider() { }

        public static T Instance
        {
            get
            {
                return SingletonCreator.Instance;
            }
        }

        private class SingletonCreator
        {
            static SingletonCreator() { }

            internal static readonly T Instance = new T();
        }
    }//class
}
