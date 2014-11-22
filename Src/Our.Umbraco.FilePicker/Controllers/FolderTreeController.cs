using System.Linq;
using System.Net.Http.Formatting;
using Umbraco.Core.IO;
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
			if (!string.IsNullOrWhiteSpace(queryStrings.Get("files")))
				return AddFiles(queryStrings);

			return AddFolders(id == "-1" ? "" : id, queryStrings);
		}

		private TreeNodeCollection AddFiles(FormDataCollection queryStrings)
		{
			var pickerApiController = new FilePickerApiController();
			var str = queryStrings.Get("startfolder");

			if (string.IsNullOrWhiteSpace(str))
				return null;

			var filter = queryStrings.Get("filter").Split(',');
			

			var path = IOHelper.MapPath(str);
			var treeNodeCollection = new TreeNodeCollection();
			treeNodeCollection.AddRange(pickerApiController.GetFiles(str, filter)
				.Select(file => CreateTreeNode(file.FullName.Replace(path, "").Replace("\\", "/"),
					path, queryStrings, file.Name, "icon-document", false)));

			return treeNodeCollection;
		}

		private TreeNodeCollection AddFolders(string parent, FormDataCollection queryStrings)
		{
			var pickerApiController = new FilePickerApiController();

			var filter = queryStrings.Get("filter");
			if (string.IsNullOrWhiteSpace(filter))
				filter = "*";

			var treeNodeCollection = new TreeNodeCollection();
			treeNodeCollection.AddRange(pickerApiController.GetFolders(parent, filter)
				.Select(dir => CreateTreeNode(dir.FullName.Replace(IOHelper.MapPath("~"), "").Replace("\\", "/"),
					"~/" + parent, queryStrings, dir.Name,
					"icon-folder", dir.EnumerateDirectories().Any())));

			return treeNodeCollection;
		}

		protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
		{
			return null;
		}
	}
}
