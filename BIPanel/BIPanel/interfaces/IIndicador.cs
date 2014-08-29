using System;
using System.Collections.Generic;
using srt.bipanel.entidades;
namespace srt.bipanel.interfaces
{
    public interface IIndicador
    {
        bool Ascendente { get; set; }
        EPeriodo Periodo { get; set; }
        double Rojo { get; set; }
        string SubTitulo { get; set; }
        string Titulo { get; set; }
        string UnidadMedida { get; set; }
        double Verde { get; set; }
        List<IValorIndicador> Valores { get; set; }
    }
}
