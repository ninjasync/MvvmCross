// MvxSingleton.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cirrious.CrossCore.Exceptions;

#if DOT42
using Java.Util.Concurrent;
#endif

namespace Cirrious.CrossCore.Core
{
    public abstract class MvxSingleton
        : IDisposable
    {
        ~MvxSingleton()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool isDisposing);

        private static readonly List<MvxSingleton> Singletons = new List<MvxSingleton>();

        protected MvxSingleton()
        {
            lock (Singletons)
            {
                Singletons.Add(this);
            }
        }

        public static void ClearAllSingletons()
        {
            lock (Singletons)
            {
                foreach (var s in Singletons)
                {
                    s.Dispose();
                }

                Singletons.Clear();
            }
        }
    }

#if !DOT42
    public abstract class MvxSingleton<TInterface>
        : MvxSingleton
        where TInterface : class
    {
        protected MvxSingleton()
        {
            if (Instance != null)
                throw new MvxException("You cannot create more than one instance of MvxSingleton");

            Instance = this as TInterface;
        }

        public static TInterface Instance { get; private set; }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Instance = null;
            }
        }
    }
#else
    public abstract class MvxSingleton<TInterface>
        : MvxSingleton
        where TInterface : class
    {
        [SuppressMessage("dot42", "StaticFieldInGenericType")]
        private static readonly ConcurrentHashMap<Type, object> _instances = new ConcurrentHashMap<Type, object>();

        protected MvxSingleton()
        {
            if (Instance != null)
                throw new MvxException("You cannot create more than one instance of MvxSingleton");

            Instance = this as TInterface;
        }

        public static TInterface Instance 
        {
            get
            {
                return _instances.Get(typeof(TInterface)) as TInterface;
            }       
            
            private set
            {
                if(value == null)
                    _instances.Remove(typeof(TInterface));
                else
                    _instances.Put(typeof(TInterface), value);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Instance = null;
            }
        }
    }
#endif
}