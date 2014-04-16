using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFLive
{
    interface IPDF
    {
        #region Properties

        String Producer
        {
            get;
        }

        String Author
        {
            set;
            get;
        }

        String Title
        {
            set;
            get;
        }

        String Subject
        {
            set;
            get;
        }

        #endregion

        #region Public Methods

        void createFile();


        void openFile(String path);


        void open(byte[] content);


        void addFile(String path);


        void add(byte[] content);


        int cantPages();


        void addHeader(string key, string value);


        string getHeader(string key);


        void newPage();


        void addText(String texto, String fuente, float size, String color, String leading);


        byte[] Generar();


        void addWaterMark(String watermarkText, String fuente, float size, String color);


        void addTextAllPages(String texto, String fuente, float size, String color, float xPosition, float yPosition, float angle);

        #endregion

    }
}
