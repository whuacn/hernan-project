using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using srt.bipanel.interfaces;

namespace srt.bipanel.entidades
{
    [Serializable]
    public class ValorIndicador : IValorIndicador
    {
        private double _valor;

        public double Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }
        
    }
}
