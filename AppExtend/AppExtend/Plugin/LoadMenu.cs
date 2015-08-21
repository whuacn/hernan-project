using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using System.Windows.Forms;

namespace AppExtend.Plugin
{
    static class LoadMenu
    {
        public static void Load(List<IMenu> menues)
        {

            MenuStrip menu = (MenuStrip)Program.FormPrincipal.Controls["msMenu"];
            foreach (IMenu m in menues)
            {
                ToolStripMenuItem princ = m.Menu();                
                menu.Items.Add(princ);
            }
        }
    }
}
