﻿using System;
using System.Windows.Forms;
using DataNetClient.Forms;

namespace DataNetClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMainDN());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in DataNet apllication. Message ="+ex.Message,"ERROR");
            } 
        }
    }
}
