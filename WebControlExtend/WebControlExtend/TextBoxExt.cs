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
    [ParseChildren(true)]
    [PersistChildren(true)]
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TextBoxExt runat=server></{0}:TextBoxExt>")]
    public class TextBoxExt : TextBox, IControlValidatable
    {

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


        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {

            //Agrega el Lang
            String val = Required.ToString().ToLower();
            if (Type != ValidationType.None)
            {
                val += ";" + Converter.EnumToString(Type);
            }
            writer.AddAttribute("lang", val);


            base.AddAttributesToRender(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.Write(Text);
        }

        #region Miembros de IControlValidatable

        public void ValidateInput()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
