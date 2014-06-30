using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

public class NetGalleryController
{
    public static string StorePath = null;
    public static string PhysicalPath = null;

    static public List<NetFile> GetGalleryFiles(string path)
    {
        path = Base64Decode(path);
        path = path.Replace(StorePath, "");
        List<NetFile> files = new List<NetFile>();
        string NetGalleryPath = Path.Combine(PhysicalPath, path);
        files.AddRange(GetGalleryFolders(NetGalleryPath));
        files.AddRange(GetGalleryPhotos(NetGalleryPath));
        return files;
    }

    static List<NetFile> GetGalleryFolders(string path)
    {
        List<NetFile> folders = new List<NetFile>();
        if (Directory.Exists(path))
        {
            foreach (string name in Directory.GetDirectories(path))
            {
                if (name.IndexOf("_tumb") == -1)
                {
                    NetFile folder = new NetFile();
                    folder.Type = TypeFile.Folder;
                    folder.Name = new DirectoryInfo(name).Name;
                    folder.Path = resolveVirtual(name);
                    folder.Description = GetDescription(folder.Path, folder.Name);
                    folder.Path = Base64Encode(folder.Path);
                    folders.Add(folder);
                }
            }            
        }
        return folders;

    }

    static List<NetFile> GetGalleryPhotos(string path)
    {
        List<NetFile> photos = new List<NetFile>();
        string[] extensions = { "jpg", "png", "gif" };
        if (Directory.Exists(path))
        {
            foreach (string name in Directory.GetFiles(path, "*.*").Where(f => extensions.Contains(f.Split('.').Last().ToLower())).ToArray())
             {
                 NetFile photo = new NetFile();
                 photo.Type = TypeFile.Photo;
                 photo.Name = Path.GetFileName(name);
                 photo.Path = Base64Encode(resolveVirtual(name));
                 photo.Description = GetDescription(path, photo.Name);
                 photos.Add(photo);
                 //CheckThumbnailPhoto(path, photo.Name);
             }
        }
        return photos;
    }

    static string GetDescription(string path, string name)
    {
        string ret = "";
        string FileDescription = Path.Combine(path, name + ".txt");
        if (File.Exists(FileDescription))
        {
            ret = File.ReadAllText(FileDescription);
        }
        else
            ret = name;
        return ret;
    }

    public static List<NavBar> GetNavBar(string path)
    {
        path = Base64Decode(path);
        path = path.Replace(StorePath, "");
        
        List<NavBar> list = new List<NavBar>();
        NavBar n;
        string[] navs = path.Split('/');
        string ant = "";
        for (int i = navs.Length - 1; i >= 0; i--)
        {
            if (navs[i] != "")
            {
                n = new NavBar();
                n.Name = navs[i];
                int index = path.LastIndexOf(ant);

                if (index != -1 && index < path.Length - 1)
                    path = path.Substring(0, index);

                n.Link = "/?p=" + Base64Encode(path);
                list.Add(n);
                ant = n.Name;
            }
        }

        n = new NavBar();
        n.Name = "Inicio";
        n.Link = "/";
        list.Add(n);

        list.Reverse();

        return list;
    }

    public static string resolveVirtual(string physicalPath)
    {
        string url = physicalPath.Substring(PhysicalPath.Length).Replace('\\', '/').Insert(0, StorePath);
        return (url);
    }

    public static byte[] GetPhoto(string path)
    {
        path = Base64Decode(path);
        byte[] ret = null;
        string PhotoPhysicalPath = System.Web.Hosting.HostingEnvironment.MapPath(path);
        if (File.Exists(PhotoPhysicalPath))
        {
            ret = File.ReadAllBytes(PhotoPhysicalPath);
        }
        return ret;
    }

    public static byte[] GetThumbnailPhoto(string path)
    {
        path = Base64Decode(path);
        byte[] ret = null;
        string PhotoPhysicalPath = System.Web.Hosting.HostingEnvironment.MapPath(path);
        if (File.Exists(PhotoPhysicalPath))
        {
            MemoryStream imageStream = new MemoryStream();
            Image image = Image.FromFile(PhotoPhysicalPath);
            int originalHeight = image.Height;
            int originalWidth = image.Width;

            int newWidth = 200;
            int newHeight = (newWidth * originalHeight) / originalWidth;

            Image thumb = image.GetThumbnailImage(newWidth, newHeight, () => false, IntPtr.Zero);
            image.Dispose();

            Rectangle cropRect = new Rectangle(0, 0, newWidth, 150);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(thumb, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }
            thumb.Dispose();
            target.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            target.Dispose();
            imageStream.Position = 0;
            ret = new Byte[imageStream.Length];
            imageStream.Read(ret, 0, (int)imageStream.Length);
            imageStream.Close();            
        }
        return ret;
        /*
        path = Base64Decode(path);
        string tumbfilename = Path.GetFileName(path) + ".tumb";
        string tumbpath = path.Substring(0, path.LastIndexOf('/'));
        tumbpath = Path.Combine(tumbpath, "_tumb");
        tumbpath = Path.Combine(tumbpath, tumbfilename);

        byte[] ret = null;
        string PhotoPhysicalPath = System.Web.Hosting.HostingEnvironment.MapPath(tumbpath);
        if (File.Exists(PhotoPhysicalPath))
        {
            ret = File.ReadAllBytes(PhotoPhysicalPath);
        }
        return ret;
        */
    }

    static void CheckThumbnailPhoto(string path, string filename)
    {
        string tumbfilename = filename + ".tumb";
        string tumbpath = Path.Combine(path, "_tumb");

        if (!Directory.Exists(tumbpath))
            Directory.CreateDirectory(tumbpath);

        tumbpath = Path.Combine(tumbpath, tumbfilename);
        if (!File.Exists(tumbpath))
        {
            CreateThumbnailPhoto(Path.Combine(path, filename), tumbpath);
        }
    }
    static void CreateThumbnailPhoto(string path, string tumbPath)
    {        
        string PhotoPhysicalPath = path;
        if (File.Exists(PhotoPhysicalPath))
        {
            
            Image image = Image.FromFile(PhotoPhysicalPath);
            int originalHeight = image.Height;
            int originalWidth = image.Width;

            int newWidth = 200;
            int newHeight = (newWidth * originalHeight) / originalWidth;

            Image thumb = image.GetThumbnailImage(newWidth, newHeight, () => false, IntPtr.Zero);
            image.Dispose();

            Rectangle cropRect = new Rectangle(0, 0, newWidth, 150);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(thumb, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }

            target.Save(tumbPath);
            
            //thumb = ((Bitmap)thumb).Clone(new Rectangle(0, 0, 200, 150), System.Drawing.Imaging.PixelFormat.Undefined);
            //thumb.Save(tumbPath);

        }

    }
    static string Base64Encode(string plainText)
    {
        try
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        catch (Exception)
        {
            return "";
        }
    }

    static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

}