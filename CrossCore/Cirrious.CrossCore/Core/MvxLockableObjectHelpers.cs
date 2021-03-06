// MvxLockableObjectHelpers.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
#if !DOT42
using System;
using System.Threading;

namespace Cirrious.CrossCore.Core
{
    public static class MvxLockableObjectHelpers
    {
        public static void RunSyncWithLock(object lockObject, Action action)
        {
            lock (lockObject)
            {
                action();
            }
        }

        public static void RunAsyncWithLock(object lockObject, Action action)
        {
            MvxAsyncDispatcher.BeginAsync(() =>
                {
                    lock (lockObject)
                    {
                        action();
                    }
                });
        }

        public static void RunSyncOrAsyncWithLock(object lockObject, Action action, Action whenComplete = null)
        {
            if (Monitor.TryEnter(lockObject))
            {
                try
                {
                    action();
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }

                if (whenComplete != null)
                    whenComplete();
            }
            else
            {
                MvxAsyncDispatcher.BeginAsync(() =>
                    {
                        lock (lockObject)
                        {
                            action();
                        }

                        if (whenComplete != null)
                            whenComplete();
                    });
            }
        }
    }
}
#endif