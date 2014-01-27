using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Umbraco.Core.IO;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

[PluginController("FilePicker")]
public class FilePickerApiController : UmbracoAuthorizedJsonController
{
    public IEnumerable<DirectoryInfo> GetFolders(string parent)
    {
        string relPath = "~/" + parent;
        string absPath = IOHelper.MapPath(relPath);

        DirectoryInfo dirs = new DirectoryInfo(absPath);

        return dirs.EnumerateDirectories();
        
    }

    public IEnumerable<FileInfo> GetFiles(string folder)
    {
        string path = IOHelper.MapPath(folder);


       
        DirectoryInfo files = new DirectoryInfo(path);

        return files.EnumerateFiles();

        

    }
}