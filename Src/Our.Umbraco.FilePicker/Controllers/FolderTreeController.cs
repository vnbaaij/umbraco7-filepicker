using System.Linq;
using System.Net.Http.Formatting;
using Umbraco.Core.IO;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace Our.Umbraco.FilePicker.Controllers
{
	[Tree("dummy", "fileTree", "Folders")]
	[PluginController("FilePicker")]
	public class FolderTreeController : TreeController
	{
		protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
		{
            if (!string.IsNullOrWhiteSpace(queryStrings.Get("startfolder")))
            {
                string folder = id == "-1" ? queryStrings.Get("startfolder") : id;
                folder = folder.EnsureStartsWith("/");
                TreeNodeCollection tempTree = AddFolders(folder, queryStrings);
                tempTree.AddRange(AddFiles(folder, queryStrings));
                return tempTree;
            }

			return AddFolders(id == "-1" ? "" : id, queryStrings);
		}

		private TreeNodeCollection AddFiles(string folder, FormDataCollection queryStrings)
		{
			var pickerApiController = new FilePickerApiController();
			//var str = queryStrings.Get("startfolder");

			if (string.IsNullOrWhiteSpace(folder))
				return null;

			var filter = queryStrings.Get("filter").Split(',').Select(a=>a.Trim().EnsureStartsWith(".")).ToArray();
 			

			var path = IOHelper.MapPath(folder);
            var rootPath = IOHelper.MapPath(queryStrings.Get("startfolder"));
			var treeNodeCollection = new TreeNodeCollection();
			treeNodeCollection.AddRange(pickerApiController.GetFiles(folder, filter)
				.Select(file => CreateTreeNode(file.FullName.Replace(rootPath, "").Replace("\\", "/"),
					path, queryStrings, file.Name, "icon-document", false)));

			return treeNodeCollection;
		}

		private TreeNodeCollection AddFolders(string parent, FormDataCollection queryStrings)
		{
			var pickerApiController = new FilePickerApiController();

            var filter = queryStrings.Get("filter").Split(',').Select(a => a.Trim().EnsureStartsWith(".")).ToArray();

			var treeNodeCollection = new TreeNodeCollection();
			treeNodeCollection.AddRange(pickerApiController.GetFolders(parent,filter)
				.Select(dir => CreateTreeNode(dir.FullName.Replace(IOHelper.MapPath("~"), "").Replace("\\", "/"),
					"~/" + parent, queryStrings, dir.Name,
                    "icon-folder", filter[0]=="." ? dir.EnumerateDirectories().Any() : pickerApiController.GetFiles(dir.FullName.Replace(IOHelper.MapPath("~"), "").Replace("\\", "/"), filter).Any())));

			return treeNodeCollection;
		}

		protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
		{
			return null;
		}
	}
}
