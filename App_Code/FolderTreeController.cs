using System.IO;
using System.Linq;
using System.Net.Http.Formatting;

using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;


/// <summary>
/// Shows tree of all folders in the site's filesystem or files in a selected folder
/// </summary>

//add treecontroller
[Tree("settings", "diskTree", "Folders")]
[PluginController("DiskPicker")]
public class FolderTreeController : TreeController
{


    protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
    {
        if (string.IsNullOrWhiteSpace(queryStrings.Get("files")))
        {
            //check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {
                return AddFolders("", queryStrings);
            }
            return AddFolders(id, queryStrings);
        }
        else
        {
            return AddFiles(queryStrings);
        }

    }

    private TreeNodeCollection AddFiles(FormDataCollection queryStrings)
    {
        if (string.IsNullOrWhiteSpace(queryStrings.Get("startfolder")))
            return null;

        string path = IOHelper.MapPath(queryStrings.Get("startfolder"));

        var nodes = new TreeNodeCollection();
        DirectoryInfo folder = new DirectoryInfo(path);

        foreach (var file in folder.EnumerateFiles())
        {
            var node = CreateTreeNode(file.FullName.Replace(path, "").Replace("\\", "/"), path, queryStrings, file.Name, "icon-document", false);
            nodes.Add(node);
        }
        return nodes;
    }


    private TreeNodeCollection AddFolders(string parent, FormDataCollection queryStrings)
    {
        string relPath = "~/" + parent;
        string absPath = IOHelper.MapPath(relPath);
        
        var nodes = new TreeNodeCollection();

        DirectoryInfo dirs = new DirectoryInfo(absPath);
               
        foreach (var dir in dirs.EnumerateDirectories())
        {
            var node = CreateTreeNode(dir.FullName.Replace(IOHelper.MapPath("~"), "").Replace("\\", "/"), relPath, queryStrings, dir.Name, "icon-folder", dir.EnumerateDirectories().Any());
            nodes.Add(node);
        }
        
        return nodes;
    }



    protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
    {

        var menu = new MenuItemCollection();

        return null;

    }
}