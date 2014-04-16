using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFLive
{
    public class PDF2 : IPDF
    {
        #region Properties
        /// <summary>
        /// Devuelve el productor del PDF
        /// </summary> 
        public string Producer
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Agrega o devuelve el autor del PDF
        /// </summary>  
        public string Author
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Agrega o devuelve el titulo del PDF
        /// </summary>  
        public string Title
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Agrega o devuelve el asunto del PDF
        /// </summary>  
        public string Subject
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Crea un PDF vacio
        /// </summary>  
        public void createFile()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Abre el pdf desde una ruta fisica
        /// </summary>
        /// <param name="path">Path del PDF</param>
        public void openFile(string path)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Abre el pdf desde un contenido binario
        /// </summary>
        /// <param name="content">Contenido binario</param>
        public void open(byte[] content)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Concatena el pdf abierto con otro desde un pdf fisico
        /// </summary>
        /// <param name="path">Path del PDF</param>
        public void addFile(string path)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Concatena el pdf abierto con otro desde un contenido binario
        /// </summary>
        /// <param name="content">Contenido binario</param>
        public void add(byte[] content)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Devuelve la cantidad de paginas del PDF abierto
        /// </summary>
        public int cantPages()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Agrega un encabezado header
        /// </summary>
        /// <param name="key">Nombre del encabezado</param>
        /// <param name="value">Valor del encabezado</param>
        public void addHeader(string key, string value)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Devuelve la cantidad de paginas del PDF abierto
        /// </summary>
        public string getHeader(string key)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Crea una nueva pagina en el PDF abierto
        /// </summary>     
        public void newPage()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Inserta texto en el PDF abierto
        /// </summary>
        /// <param name="texto">Texto a insertar</param>
        /// <param name="fuente">Nombre de la fuente</param>
        /// <param name="size">Tamaño de la fuente</param>
        /// <param name="color">Color de la fuente en hexa (#000000)</param>
        /// <param name="leading">Interlineado</param>
        public void addText(string texto, string fuente, float size, string color, string leading)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Devuelve el pdf abierto en bytes
        /// </summary>     
        public byte[] Generar()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Agrega una marca de agua en el pdf abierto
        /// </summary>
        /// <param name="watermarkText">Texto a insertar como marca de agua</param>
        /// <param name="fuente">Nombre de la fuente</param>
        /// <param name="size">Tamaño de la fuente</param>
        /// <param name="color">Color de la fuente en hexa (#000000)</param>
        public void addWaterMark(string watermarkText, string fuente, float size, string color)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Inserta texto en todas las paginas del PDF abierto
        /// </summary>
        /// <param name="texto">Texto a insertar</param>
        /// <param name="fuente">Nombre de la fuente</param>
        /// <param name="size">Tamaño de la fuente</param>
        /// <param name="color">Color de la fuente en hexa (#000000)</param>
        /// <param name="xPosition">Posicion horizontal del texto</param>
        /// <param name="yPosition">Posicion vertical del texto</param>
        /// <param name="angle">Angulo del texto</param>
        public void addTextAllPages(string texto, string fuente, float size, string color, float xPosition, float yPosition, float angle)
        {
            throw new NotImplementedException();
        }
         #endregion
    }
}
