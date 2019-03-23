using System;
using System.Windows.Forms;

namespace Calc_for_Matrix
{
    static class Program
    {       
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
