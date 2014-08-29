using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using srt.bipanel.interfaces;

namespace srt.bipanel.entidades
{
    public class FactoryBIPanel
    {
        public static IIndicador Indicador()
        {
            return new Indicador() { Ascendente = true };
        }
        public static IValorIndicador ValorIndicador()
        {
            return new ValorIndicador();
        }
    }
}
