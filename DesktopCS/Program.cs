﻿using System;
using System.Windows.Forms;
using DesktopCS.Forms;

namespace DesktopCS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm login = new LoginForm();
            login.Show();

            Application.Run();
        }
    }
}
