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
[Tree("settings", "fileTree", "Folders")]
[PluginController("FilePicker")]
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
        var ctrl = new FilePickerApiController();
        var nodes = new TreeNodeCollection();
        var folder = queryStrings.Get("startfolder");
        if (string.IsNullOrWhiteSpace(folder))
            return null;

        string path = IOHelper.MapPath(folder);


        foreach (var file in ctrl.GetFiles(folder))
        {
            var node = CreateTreeNode(file.FullName.Replace(path, "").Replace("\\", "/"), path, queryStrings, file.Name, "icon-document", false);
            nodes.Add(node);
        }
        return nodes;
    }


    private TreeNodeCollection AddFolders(string parent, FormDataCollection queryStrings)
    {
        var ctrl = new FilePickerApiController();
        
        var nodes = new TreeNodeCollection();

               
        foreach (var dir in ctrl.GetFolders(parent))
        {
            var node = CreateTreeNode(dir.FullName.Replace(IOHelper.MapPath("~"), "").Replace("\\", "/"), "~/"+parent, queryStrings, dir.Name, "icon-folder", dir.EnumerateDirectories().Any());
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