using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class NetFile
{
    public NetFile()
    {
    }
    public TypeFile Type { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Description { get; set; }

}

 public enum TypeFile
 {
     Folder,
     Photo
 }