using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using srt.bipanel.interfaces;

namespace srt.bipanel.servicios
{
    public class GestorGrafico
    {
        private static GestorGrafico _gestor = null;
        public static GestorGrafico GetInstance()
        {
            if (_gestor == null)
                _gestor = new GestorGrafico();

            return _gestor;
        }

        public string ColorIndicator(double value, double green, double red, bool ascendent)
        {
            string color = "#DDDF0D"; //amarillo
            if (ascendent)
            {
                if (value <= green)
                    color = "#55BF3B"; //verde
                if (value >= red)
                    color = "#DF5353"; //rojo
            }
            else
            {
                if (value >= green)
                    color = "#55BF3B";
                if (value <= red)
                    color = "#DF5353";
            }
            return color;
        }
        public string RefrenceGreenTextIndicator(double value, bool ascendent)
        {
            string r = "";
            if (ascendent)
                r = "Menor a " + value.ToString();
            else
                r = "Mayor a " + value.ToString();
            return r;
        }
        public string RefrenceRedTextIndicator(double value, bool ascendent)
        {
            string r = "";
            if (ascendent)
                r = "Mayor a " + value.ToString();
            else
                r = "Menor a " + value.ToString();
            return r;
        }

        public string CreateGauge(IIndicador indicador)
        {
            Highcharts chart = new Highcharts("chartGauge"+DateTime.Now.Ticks.ToString())
                .InitChart(new Chart
                {
                    Type = ChartTypes.Solidgauge,
                    Height = 200,
                    Width = 350,
                    BorderRadius = 10,
                    BorderWidth = 1,
                })
                .SetTitle(new Title { Text = indicador.Titulo, Style = "fontSize:'10pt', fontWeight:'bold'" })
                .SetSubtitle(new Subtitle { Text = indicador.SubTitulo })
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

                .SetYAxis(new YAxis
                {
                    LineWidth = 0,
                    MinorTickInterval = null,
                    TickPixelInterval = 400,
                    TickWidth = 0,
                    Labels = new YAxisLabels { Y = 16 },
                    Min = GetMinValue(indicador),
                    Max = GetMaxValue(indicador),
                    Title = new YAxisTitle { Text = "" },
                    Stops = new Gradient
                    {
                        Stops = new object[,]
                                {
                                    { 0.0, ColorTranslator.FromHtml(GestorGrafico.GetInstance().ColorIndicator(indicador.Valores.First().Valor,indicador.Verde,indicador.Rojo, indicador.Ascendente)) }
                                }
                    }
                })
                .SetPlotOptions(new PlotOptions
                {
                    Solidgauge = new PlotOptionsSolidgauge
                    {
                        DataLabels = new PlotOptionsSolidgaugeDataLabels
                        {
                            Y = 30,
                            BorderWidth = 0,
                            UseHTML = true,
                            Format = "<div style=\"text-align:center\"><span style=\"font-size:15px;color:black\">{y}</span><br/><span style=\"font-size:12px;color:silver\">" + indicador.UnidadMedida + "</span><br/><span style=\"font-size:12px;color:#55BF3B\">" + GestorGrafico.GetInstance().RefrenceGreenTextIndicator(indicador.Verde, indicador.Ascendente) + "</span><br/><span style=\"font-size:12px;color:#DF5353\">" + GestorGrafico.GetInstance().RefrenceRedTextIndicator(indicador.Rojo, indicador.Ascendente) + "</span></div>"
                        }
                    }
                })
                .SetSeries(new Series
                {
                    Name = indicador.UnidadMedida,
                    Data = new Data(new object[] { indicador.Valores.First().Valor }),
                });

            return chart.ToHtmlString();
        }

        double GetMaxValue(IIndicador indicador)
        {
            double r = 0;
            if (indicador.Ascendente)
                r = indicador.Rojo + indicador.Valores.First().Valor;
            return r;
        }

        double GetMinValue(IIndicador indicador)
        {
            double r = 0;
            if (!indicador.Ascendente)
                r = indicador.Verde + indicador.Valores.First().Valor;
            return r;
        }
    }
}
