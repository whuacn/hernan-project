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
        [Description("Valida el texto con las opciones seleccionadas.")]      
        public ValidationType Validation
        {
            get
            {
                if (ViewState["Validation"] == null)
                    ViewState["Validation"] = ValidationType.None;
                return (ValidationType)ViewState["Validation"];
            }
            set
            {
                ViewState["Validation"] = value;
            }
        }


        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {

            //Agrega el Lang
            String val = Required.ToString().ToLower();
            if (Validation != ValidationType.None)
            {
                val += ";" + Converter.EnumToString(Validation);
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
