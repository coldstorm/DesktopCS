﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace DesktopCS.Services
{
    public abstract class UIInvoker
    {
        private readonly Dispatcher _dispatcher = Application.Current.Dispatcher;

        protected bool InvokeRequired
        {
            get
            {
               return _dispatcher.Thread != Thread.CurrentThread;
            }
        }

        protected void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }
    }
}