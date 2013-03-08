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
    [ToolboxData("<{0}:DropDownListExt runat=server></{0}:DropDownListExt>")]
    public class DropDownListExt : DropDownList, IControlValidatable
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
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [DefaultValue("false")]
        [Category("Validations")]
        [Description("Setea si el campo es obligatorio.")]
        public bool Required
        {
            get
            {
                if (ViewState["Required"] == null)
                    ViewState["Required"] = false;
                return (bool)ViewState["Required"];
            }
            set
            {
                ViewState["Required"] = value;
            }
        }

        [Bindable(true)]
        [Browsable(true)]
        [DefaultValue(ValidationType.None)]
        [Category("Validations")]
        [Description("Tipo de dato a validar.")]
        public ValidationType Type
        {
            get
            {
                if (ViewState["Type"] == null)
                    ViewState["Type"] = ValidationType.None;
                return (ValidationType)ViewState["Type"];
            }
            set
            {
                ViewState["Type"] = value;
            }
        }

        protected override void AddAttributesToRender(HtmlTextWriter output)
        {

            //Agrega el Lang
            String val = Required.ToString().ToLower();
            if (Type != ValidationType.None)
            {
                val += ";" + Converter.EnumToString(Type);
            }
            output.AddAttribute("lang", val);


            base.AddAttributesToRender(output);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            AddAttributesToRender(output);
            output.Write(Text);
        }
    }
}
