using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using System.Windows.Forms;

namespace Plug2
{
    public class Ayuda : IMenu
    {
        ToolStripMenuItem menu;
        List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
        public Ayuda()
        {
            menu = new ToolStripMenuItem("Ayuda");

            ToolStripMenuItem i = new ToolStripMenuItem();
            i.Text = "Help";
            i.Click += new EventHandler(Help_Click);
            items.Add(i);
 
            menu.DropDownItems.AddRange(items.ToArray());
            
        }

        void Help_Click(object sender, EventArgs e)
        {
            Help h = new Help();
            h.Show();
        }

        public ToolStripMenuItem Menu()
        {
            return menu;
        }
    }
}
