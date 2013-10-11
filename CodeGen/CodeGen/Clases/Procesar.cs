using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using CodeGen.Properties;

namespace CodeGen.Clases
{
    class Procesar
    {
        #region Private Members
        Table table;
        List<Column> ColumnsSelected;
        string TEMPLATEENTIDAD;
        string TEMPLATEGESTOR;
        #endregion

        public Procesar(Table t)
        {
            table = t;
            ColumnsSelected = t.Columns.Where(c => c.Selected == true).ToList();
        }

        private string FullPathEntidad
        {
            get
            {
                string fullPath =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}/{1}/{2}.cs",
                        Application.StartupPath,
                        table.ClassName,
                        table.ClassName);

                return fullPath;
            }
        }

        private string FullPathGestor
        {
            get
            {
                string fullPath =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}/{1}/{2}.cs",
                        Application.StartupPath,
                        table.ClassName,
                        table.GestorName);

                return fullPath;
            }

        }

        private string Path
        {
            get
            {
                string Path =
                    string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}/{1}",
                        Application.StartupPath,
                        table.ClassName);

                return Path;
            }
        }

        public void Iniciar()
        {
            LoadTemplate();

            DefaultTokens();

            if (!Directory.Exists(this.Path))
                Directory.CreateDirectory(this.Path);

            File.WriteAllText(this.FullPathEntidad, this.TEMPLATEENTIDAD, Encoding.Default);
            File.WriteAllText(this.FullPathGestor, this.TEMPLATEGESTOR, Encoding.Default);

        }
        void LoadTemplate()
        {
            string templatePathEntidad =
                string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}\TemplateEntidad.txt",
                    Application.StartupPath);
            this.TEMPLATEENTIDAD = File.ReadAllText(templatePathEntidad);

            string templatePathGestor =
                string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}\TemplateGestor.txt",
                    Application.StartupPath);
            this.TEMPLATEGESTOR = File.ReadAllText(templatePathGestor);
        }
        void DefaultTokens()
        {
            this.TEMPLATEENTIDAD =
                this.TEMPLATEENTIDAD
                    .Replace(Template.DATECREATED, DateTime.Today.ToShortDateString())
                    .Replace(Template.NAMESPACE, table.NameSpaceName)
                    .Replace(Template.OBJECTNAME, table.ClassName)
                    .Replace(Template.CONSTRUCTOR, CrearConstructor())
                    .Replace(Template.PROPERTIES, CrearPropiedades())
                    .Replace(Template.DATAMEMBERS, CrearDataMembers());

            this.TEMPLATEGESTOR =
                this.TEMPLATEGESTOR
                    .Replace(Template.NAMESPACE, table.NameSpaceName)
                    .Replace(Template.OBJECTNAME, table.GestorName)
                    .Replace(Template.CLASSTYPE, table.ClassName)
                    .Replace(Template.CLASSTYPELOWER, table.ClassName.ToLower())
                    .Replace(Template.TABLENAME, table.TableName)
                    .Replace(Template.STOREDPARAMS, CrearStoredParams())
                    .Replace(Template.MAPPER, CrearMapper());            

        }


        string CrearConstructor()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(@"		public {0} ();{1}", table.ClassName, Environment.NewLine);
            builder.AppendFormat(@"		{{{0}", Environment.NewLine);
            builder.AppendFormat(@"		    Initialize();{0}", Environment.NewLine);
            builder.AppendFormat(@"		}}{0}", Environment.NewLine);
            builder.AppendFormat(@"		{0}", Environment.NewLine);
            builder.AppendFormat(@"		public Initialize ();{0}", Environment.NewLine);
            builder.AppendFormat(@"		{{{0}", Environment.NewLine);
            builder.AppendFormat(@"		{0}", Environment.NewLine);
            builder.AppendFormat(@"		}}{0}", Environment.NewLine);

            return builder.ToString().Trim();
        }

        string CrearPropiedades()
        {
            StringBuilder builder = new StringBuilder();

            foreach (Column c in ColumnsSelected)
            {
                builder.AppendFormat(@"		public {0} {1} {{{2}", c.DataType, c.Property, Environment.NewLine);
                builder.AppendFormat(@"		    get {{ return _{0}; }}{1}", c.Property, Environment.NewLine);
                builder.AppendFormat(@"		    set {{ this._{0} = value; }}{1}", c.Property, Environment.NewLine);
                builder.AppendFormat(@"		}}{0}{0}", Environment.NewLine);
            }

            return builder.ToString().Trim();
        }

        string CrearDataMembers()
        {
            StringBuilder builder = new StringBuilder();

            foreach (Column c in ColumnsSelected)
            {
                builder.AppendFormat(@"		private {0} _{1};{2}", c.DataType, c.Property, Environment.NewLine);
            }

            return builder.ToString().Trim();
        }

        string CrearMapper()
        {
            StringBuilder builder = new StringBuilder();

            foreach (Column c in ColumnsSelected)
            {
                builder.AppendFormat(@"		        {0} = row[""{1}""].ToString(),{2}", c.Property, c.ColumnName, Environment.NewLine);
            }

            var index = builder.ToString().LastIndexOf(',');
            if (index >= 0)
                builder.Remove(index, 1);

            return builder.ToString().Trim();
        }

        string CrearStoredParams()
        {
            StringBuilder builder = new StringBuilder();

            foreach (Column c in ColumnsSelected)
            {
                builder.AppendFormat(@"		        repo.AddParameter(""@{0}"", {1}.{2});{3}", c.ColumnName, table.ClassName.ToLower(), c.Property, Environment.NewLine);
            }

            return builder.ToString().Trim();
        }


    }
}
