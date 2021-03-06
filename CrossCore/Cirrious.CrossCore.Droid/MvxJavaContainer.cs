// MvxJavaContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#if DOT42
using System;
#else
using Java.Lang;
#endif

namespace Cirrious.CrossCore.Droid
{
    public class MvxJavaContainer : Object
    {
        public MvxJavaContainer(object theObject)
        {
            Object = theObject;
        }

        public object Object { get; private set; }
    }

    public class MvxJavaContainer<T> : MvxJavaContainer
    {
        public MvxJavaContainer(T theObject)
            : base(theObject)
        {
        }

        public new T Object
        {
            get { return (T) base.Object; }
        }
    }
}