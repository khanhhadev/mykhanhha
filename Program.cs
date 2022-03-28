using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShinkoComm_V205;
using System.Resources;
using System.Globalization;

namespace CommTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            System.Diagnostics.Process hThisProcess = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] hProcesses = System.Diagnostics.Process.GetProcessesByName(hThisProcess.ProcessName);
            foreach (System.Diagnostics.Process hProcess in hProcesses)
            {
                if (hProcess.Id != hThisProcess.Id)
                {
                    if (string.Compare(hProcess.MainModule.FileName, hThisProcess.MainModule.FileName) == 0)
                    {
                        MessageBox.Show("EEPROM_CHECKソフトは既に起動しています", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
