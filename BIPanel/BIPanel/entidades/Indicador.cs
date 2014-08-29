using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using srt.bipanel.interfaces;

namespace srt.bipanel.entidades
{
    [Serializable]
    public class Indicador : IIndicador
    {

        public Indicador()
        {
            Valores = new List<IValorIndicador>();
        }

        private string _titulo;
        public string Titulo
        {
            get { return _titulo; }
            set { _titulo = value; }
        }

        private string _subtitulo;
        public string SubTitulo
        {
            get { return _subtitulo; }
            set { _subtitulo = value; }
        }

        private bool _ascendente;
        public bool Ascendente
        {
            get { return _ascendente; }
            set { _ascendente = value; }
        }

        private double _verde;
        public double Verde
        {
            get { return _verde; }
            set { _verde = value; }
        }

        private double _rojo;
        public double Rojo
        {
            get { return _rojo; }
            set { _rojo = value; }
        }

        private string _unidademedida;
        public string UnidadMedida
        {
            get { return _unidademedida; }
            set { _unidademedida = value; }
        }

        private EPeriodo _periodo;
        public EPeriodo Periodo
        {
            get { return _periodo; }
            set { _periodo = value; }
        }

        private List<IValorIndicador> _valores;

        public List<IValorIndicador> Valores
        {
            get { return _valores; }
            set { _valores = value; }
        }
        
        
        
    }
}
