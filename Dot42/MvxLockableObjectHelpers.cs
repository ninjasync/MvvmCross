// MvxLockableObjectHelpers.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using System;
using Java.Util.Concurrent.Locks;

namespace Cirrious.CrossCore.Core
{
    public static class MvxLockableObjectHelpers
    {
        public static void RunSyncWithLock(ILock lockObject, Action action)
        {
            lockObject.Lock();
            try
            {
                action();
            }
            finally { lockObject.Unlock(); }
        }

        public static void RunAsyncWithLock(ILock lockObject, Action action)
        {
            MvxAsyncDispatcher.BeginAsync(() => RunSyncWithLock(lockObject, action));
        }

        public static void RunSyncOrAsyncWithLock(ILock lockObject, Action action, Action whenComplete = null)
        {
            if(lockObject.TryLock())
            {
                try
                {
                    action();
                }
                finally
                {
                    lockObject.Unlock();
                }

                if (whenComplete != null)
                    whenComplete();
            }
            else
            {
                MvxAsyncDispatcher.BeginAsync(() =>
                    {
                        RunSyncWithLock(lockObject, action);

                        if (whenComplete != null)
                            whenComplete();
                    });
            }
        }
    }
}
