using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using srt.bipanel.entidades;
using srt.bipanel.interfaces;
using srt.bipanel.servicios;

public partial class _Default : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        AddPanel("Gerencia de Sistemas");
        AddPanel("Gerencia de Prevención");

    }

    void AddPanel(string area)
    {
        int count = 0;

        System.Web.UI.HtmlControls.HtmlGenericControl dynDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
        dynDiv.Attributes.Add("class","accordion");

        System.Web.UI.HtmlControls.HtmlGenericControl dynDiv2 = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
        dynDiv2.InnerHtml = "<b>" + area + "</b>";
        dynDiv.Controls.Add(dynDiv2);

        System.Web.UI.HtmlControls.HtmlGenericControl dynDiv3 = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");

        AddGauge(Gauge1(), dynDiv3, count);
        AddGauge(Gauge2(), dynDiv3, count);
        AddGauge(Gauge3(), dynDiv3, count);
        AddGauge(Gauge4(), dynDiv3, count);
        AddGauge(Gauge2(), dynDiv3, count);
        AddGauge(Gauge1(), dynDiv3, count);

        dynDiv.Controls.Add(dynDiv3);

        DivContent.Controls.Add(dynDiv);

    }
    IIndicador Gauge1()
    {
        IIndicador indicador = FactoryBIPanel.Indicador();
        indicador.Titulo = "Cantidad de Expedientes asignados";
        indicador.SubTitulo = "Depto. de Desarrollo de Aplicaciones";
        indicador.Verde = 100;
        indicador.Rojo = 50;
        indicador.Ascendente = false;
        indicador.Periodo = EPeriodo.Mensual;
        indicador.UnidadMedida = "Expedientes";

        IValorIndicador valor1 = FactoryBIPanel.ValorIndicador();
        valor1.Valor = 45;
        indicador.Valores.Add(valor1);

        return indicador;
    }
    IIndicador Gauge2()
    {
        IIndicador indicador = FactoryBIPanel.Indicador();
        indicador.Titulo = "Tiempo de Respuesta de Expedientes";
        indicador.SubTitulo = "Depto. de Desarrollo de Aplicaciones";
        indicador.Verde = 10;
        indicador.Rojo = 25;
        indicador.Ascendente = true;
        indicador.Periodo = EPeriodo.Mensual;
        indicador.UnidadMedida = "Horas";

        IValorIndicador valor1 = FactoryBIPanel.ValorIndicador();
        valor1.Valor = 5;
        indicador.Valores.Add(valor1);

        return indicador;
    }
    IIndicador Gauge3()
    {
        IIndicador indicador = FactoryBIPanel.Indicador();
        indicador.Titulo = "Cantidad de Llamadas Atendidas";
        indicador.SubTitulo = "Depto. de Desarrollo de Aplicaciones";
        indicador.Verde = 250;
        indicador.Rojo = 100;
        indicador.Ascendente = false;
        indicador.Periodo = EPeriodo.Mensual;
        indicador.UnidadMedida = "Llamadas";

        IValorIndicador valor1 = FactoryBIPanel.ValorIndicador();
        valor1.Valor = 200;
        indicador.Valores.Add(valor1);

        return indicador;

    }
    IIndicador Gauge4()
    {
        IIndicador indicador = FactoryBIPanel.Indicador();
        indicador.Titulo = "Cantidad de Reclamos Solucionados";
        indicador.SubTitulo = "Depto. de Desarrollo de Aplicaciones";
        indicador.Verde = 250;
        indicador.Rojo = 100;
        indicador.Ascendente = false;
        indicador.Periodo = EPeriodo.Mensual;
        indicador.UnidadMedida = "Reclamos";

        IValorIndicador valor1 = FactoryBIPanel.ValorIndicador();
        valor1.Valor = 260;
        indicador.Valores.Add(valor1);

        return indicador;

    }

    void AddGauge(IIndicador indicador, System.Web.UI.HtmlControls.HtmlGenericControl div, int count)
    {
        count++;

        if (count > 3)
        {
            div.Controls.Add(new LiteralControl("<br />"));
            count = 1;
        }

        System.Web.UI.HtmlControls.HtmlGenericControl dynDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
        dynDiv.ID = "dynDiv" + DateTime.Now.Ticks.ToString();
        dynDiv.Style.Add(HtmlTextWriterStyle.Display, "inline-block");

        Literal ctrlChart = new Literal();
        ctrlChart.ID = "ltrChart" + DateTime.Now.Ticks.ToString();
        ctrlChart.Text = GestorGrafico.GetInstance().CreateGauge(indicador);

        dynDiv.Controls.Add(ctrlChart);

        div.Controls.Add(dynDiv);
    }

    
}
