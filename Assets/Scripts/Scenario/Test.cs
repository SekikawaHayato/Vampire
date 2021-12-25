using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATest
{
    public struct A
    {
        int _a;
        int _b;


        public int GetA()
        {
            return _a;
        }
        public A(int a, int b)
        {
            this._a = a;
            this._b = b;
        }
    }

}

