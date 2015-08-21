using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using System.Windows.Forms;

namespace Plug1
{
    public class Herramientas : IMenu
    {
        ToolStripMenuItem menu;
        List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
        public Herramientas()
        {
            menu = new ToolStripMenuItem("Herramientas");

            ToolStripMenuItem i = new ToolStripMenuItem();
            i.Text = "Hacer 1";
            i.Click += new EventHandler(H1_Click);
            items.Add(i);
            i = new ToolStripMenuItem();
            i.Text = "Hacer 2";
            items.Add(i);
            i = new ToolStripMenuItem();
            i.Text = "Hacer 3";
            items.Add(i);       
 
            menu.DropDownItems.AddRange(items.ToArray());
            
        }

        void H1_Click(object sender, EventArgs e)
        {
            About frmAbout = new About();
            frmAbout.Show();
        }
        public ToolStripMenuItem Menu()
        {
            return menu;
        }

    }
}
