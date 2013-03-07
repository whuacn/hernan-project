using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Srt.WebControlExtend
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TextBoxExt runat=server></{0}:TextBoxExt>")]
    public class TextBoxExt : TextBox
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? "[" + this.ID + "]" : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }


        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Localizable(true)]
        public bool Required
        {
            get
            {
                bool s = (bool)ViewState["Required"];
                return s;
            }

            set
            {
                ViewState["Required"] = value;
            }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute("lang", Required.ToString().ToLower());
            base.AddAttributesToRender(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.Write(Text);
        }
    }
}
