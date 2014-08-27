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

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BasicLine();
        BasicBar();
        BasicColumn();
        BasicGauge();
        BasicGauge2();
    }

    void BasicLine()
    {

        Highcharts chart = new Highcharts("chartLine")
                        .InitChart(new Chart
                        {
                            Type = ChartTypes.Line,
                            Height = 300,
                            Width = 705,
                            BorderRadius = 10,
                            BorderWidth = 1,
                            /*MarginRight = 130,
                            MarginBottom = 25,*/
                            ClassName = "chart"
                        })
                        .SetTitle(new Title
                        {
                            Text = "Cantidad de Expedientes asignados", Style = "fontSize:'10pt'"
                        })
                        .SetXAxis(new XAxis { Categories = new string[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" } })
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Expedientes" }})
                        .SetTooltip(new Tooltip
                        {
                            Formatter = @"function() {
                                        return '<b>'+ this.series.name +'</b><br/>'+
                                    this.x +': '+ this.y +' Expedientes';
                                }"
                        })
                        .SetLegend(new Legend
                        {
                            Enabled = false
                            /*Layout = Layouts.Vertical,
                            Align = HorizontalAligns.Right,
                            VerticalAlign = VerticalAligns.Top,
                            X = -10,
                            Y = 100,
                            BorderWidth = 0*/
                        })
                        .SetSeries(new[]
                            {
                                new Series { Name = "Expedientes", Data = new Data(new object[] { 8,5,25,26,45,18,16,9,15,11,23,18 }) }//,
                                /*new Series { Name = "New York", Data = new Data(ChartsData.NewYorkData) },
                                new Series { Name = "Berlin", Data = new Data(ChartsData.BerlinData) },
                                new Series { Name = "London", Data = new Data(ChartsData.LondonData) }*/
                            }
                        );
        ltrChart.Text = chart.ToHtmlString();
    }

    void BasicBar()
    {

        Highcharts chart = new Highcharts("chartBar")
                        .InitChart(new Chart
                        {
                            Type = ChartTypes.Bar,
                            Height = 300,
                            Width = 350,
                            BorderRadius = 10,
                            BorderWidth = 1,
                            /*MarginRight = 130,
                            MarginBottom = 25,*/
                            ClassName = "chart"
                        })
                        .SetTitle(new Title
                        {
                            Text = "Cantidad de Expedientes asignados",
                            Style = "fontSize:'10pt'"
                        })
                        .SetXAxis(new XAxis { Categories = new string[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" } })
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Expedientes" } })
                        .SetTooltip(new Tooltip
                        {
                            Formatter = @"function() {
                                        return '<b>'+ this.series.name +'</b><br/>'+
                                    this.x +': '+ this.y +' Expedientes';
                                }"
                        })
                        .SetLegend(new Legend
                        {
                            Enabled = false
                            /*Layout = Layouts.Vertical,
                            Align = HorizontalAligns.Right,
                            VerticalAlign = VerticalAligns.Top,
                            X = -10,
                            Y = 100,
                            BorderWidth = 0*/
                        })
                        .SetSeries(new[]
                            {
                                new Series { Name = "Expedientes", Data = new Data(new object[] { 8,5,25,26,45,18,16,9,15,11,23,18 }) }//,
                                /*new Series { Name = "New York", Data = new Data(ChartsData.NewYorkData) },
                                new Series { Name = "Berlin", Data = new Data(ChartsData.BerlinData) },
                                new Series { Name = "London", Data = new Data(ChartsData.LondonData) }*/
                            }
                        );
        ltrChart2.Text = chart.ToHtmlString();
    }

    void BasicColumn()
    {

        Highcharts chart = new Highcharts("chartCol")
                        .InitChart(new Chart
                        {
                            Type = ChartTypes.Column,
                            Height= 300,
                            Width = 350,
                            BorderRadius = 10,
                            BorderWidth = 1,
                            /*MarginRight = 25,
                            MarginBottom = 25,*/
                            ClassName = "chart"
                        })
                        .SetTitle(new Title
                        {
                            Text = "Cantidad de Expedientes asignados",
                            Style = "fontSize:'10pt'"
                        })
                        .SetXAxis(new XAxis { Categories = new string[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" } })
                        .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Expedientes" } })
                        .SetTooltip(new Tooltip
                        {
                            Formatter = @"function() {
                                        return '<b>'+ this.series.name +'</b><br/>'+
                                    this.x +': '+ this.y +' Expedientes';
                                }"
                        })
                        .SetLegend(new Legend
                        {
                            Enabled = false
                            /*Layout = Layouts.Vertical,
                            Align = HorizontalAligns.Right,
                            VerticalAlign = VerticalAligns.Top,
                            X = -10,
                            Y = 100,
                            BorderWidth = 0*/
                        })
                        .SetSeries(new[]
                            {
                                new Series { Name = "Expedientes", Data = new Data(new object[] { 8,5,25,26,45,18,16,9,15,11,23,18 }) }//,
                                /*new Series { Name = "New York", Data = new Data(ChartsData.NewYorkData) },
                                new Series { Name = "Berlin", Data = new Data(ChartsData.BerlinData) },
                                new Series { Name = "London", Data = new Data(ChartsData.LondonData) }*/
                            }
                        );
        ltrChart3.Text = chart.ToHtmlString();
    }
    void BasicGauge()
    {
        Highcharts chart = new Highcharts("chartGauge")
                        .InitChart(new Chart { 
                            Type = ChartTypes.Solidgauge, 
                            Height = 200, 
                            Width = 350,
                            BorderRadius = 10,
                            BorderWidth = 1, })
                        .SetTitle(new Title { Text = "Cantidad de Expedientes asignados", Style = "fontSize:'10pt'" })
                        .SetPane(new Pane
                        {
                            Center = new[] { new PercentageOrPixel(50, true), new PercentageOrPixel(85, true) },
                            Size = new PercentageOrPixel(170, true),
                            StartAngle = -90,
                            EndAngle = 90,
                            Background = new[] 
                            { 
                                new BackgroundObject
                                { 
                                    BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#EEE")),
                                    InnerRadius = new PercentageOrPixel(60, true),
                                    OuterRadius = new PercentageOrPixel(100, true),                                   
                                    Shape = Shapes.Arc
                                }
                            }
                        })
                        .SetTooltip(new Tooltip { Enabled = false})
                        /*.SetTooltip(new Tooltip
                        {
                            Formatter = @"function() {
                                        return '<b>'+ this.series.name +'</b><br/>'+
                                    this.x +': '+ this.y +' Expedientes';
                                }"
                        })*/
                        .SetYAxis(new YAxis
                        {
                            LineWidth = 0,
                            MinorTickInterval = null,
                            TickPixelInterval = 400,
                            TickWidth = 0,
                            Labels = new YAxisLabels { Y = 16 },
                            Min = 200,
                            Max = 0,
                            Title = new YAxisTitle { Text = "" },
                            Stops = new Gradient
                            {
                                Stops = new object[,]
                                {
                                    { 0.1, ColorTranslator.FromHtml("#55BF3B") },
                                    { 0.5, ColorTranslator.FromHtml("#DDDF0D") },
                                    { 0.9, ColorTranslator.FromHtml("#DF5353") }
                                }
                            }
                        })
                        .SetPlotOptions(new PlotOptions
                        {
                            Solidgauge = new PlotOptionsSolidgauge
                            {
                                DataLabels = new PlotOptionsSolidgaugeDataLabels
                                {
                                    Y = 5,
                                    BorderWidth = 0,
                                    UseHTML = true,
                                    Format = "<div style=\"text-align:center\"><span style=\"font-size:15px;color:black\">{y}</span><br/><span style=\"font-size:12px;color:silver\">Expedientes<br/>Asignados</span></div>"
                                }
                            }
                        })
                        .SetSeries(new Series
                        {
                            Name = "Cantidad",
                            Data = new Data(new object[] { 30 }),
                        });

        ltrChart4.Text = chart.ToHtmlString();
    }

    void BasicGauge2()
    {
        Highcharts chart = new Highcharts("chartGauge2")
                        .InitChart(new Chart
                        {
                            Type = ChartTypes.Solidgauge,
                            Height = 200,
                            Width = 350,
                            BorderRadius = 10,
                            BorderWidth = 1,
                        })
                        .SetTitle(new Title { Text = "Tiempo de Respuesta de Expedientes", Style = "fontSize:'10pt'" })
                        .SetPane(new Pane
                        {
                            Center = new[] { new PercentageOrPixel(50, true), new PercentageOrPixel(85, true) },
                            Size = new PercentageOrPixel(170, true),
                            StartAngle = -90,
                            EndAngle = 90,
                            Background = new[] 
                            { 
                                new BackgroundObject
                                { 
                                    BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#EEE")),
                                    InnerRadius = new PercentageOrPixel(60, true),
                                    OuterRadius = new PercentageOrPixel(100, true),                                   
                                    Shape = Shapes.Arc
                                }
                            }
                        })
                        .SetTooltip(new Tooltip { Enabled = false })
            /*.SetTooltip(new Tooltip
            {
                Formatter = @"function() {
                            return '<b>'+ this.series.name +'</b><br/>'+
                        this.x +': '+ this.y +' Expedientes';
                    }"
            })*/
                        .SetYAxis(new YAxis
                        {
                            LineWidth = 0,
                            MinorTickInterval = null,
                            TickPixelInterval = 400,
                            TickWidth = 0,
                            Labels = new YAxisLabels { Y = 16 },
                            Min = 0,
                            Max = 48,
                            Title = new YAxisTitle { Text = "" },
                            Stops = new Gradient
                            {
                                Stops = new object[,]
                                {
                                    { 0.1, ColorTranslator.FromHtml("#55BF3B") },
                                    { 0.5, ColorTranslator.FromHtml("#DDDF0D") },
                                    { 0.9, ColorTranslator.FromHtml("#DF5353") }
                                }
                            }
                        })
                        .SetPlotOptions(new PlotOptions
                        {
                            Solidgauge = new PlotOptionsSolidgauge
                            {
                                DataLabels = new PlotOptionsSolidgaugeDataLabels
                                {
                                    Y = 5,
                                    BorderWidth = 0,
                                    UseHTML = true,
                                    Format = "<div style=\"text-align:center\"><span style=\"font-size:15px;color:black\">{y}</span><br/><span style=\"font-size:12px;color:silver\">Horas</span></div>"
                                }
                            }
                        })
                        .SetSeries(new Series
                        {
                            Name = "Cantidad",
                            Data = new Data(new object[] { 12 }),
                        });

        ltrChart5.Text = chart.ToHtmlString();
    }
}
