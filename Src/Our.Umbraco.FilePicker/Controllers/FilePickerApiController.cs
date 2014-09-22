using System.Collections.Generic;
using System.IO;
using Umbraco.Core.IO;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Our.Umbraco.FilePicker.Controllers
{ 
	[PluginController("FilePicker")]
	public class FilePickerApiController : UmbracoAuthorizedJsonController
	{
		public IEnumerable<DirectoryInfo> GetFolders(string folder, string filter = "*")
		{
			var path = IOHelper.MapPath("~/" + folder.TrimStart('~', '/'));
			return new DirectoryInfo(path).GetDirectories(filter);
		}

		public IEnumerable<FileInfo> GetFiles(string folder, string filter = "*")
		{
			var path = IOHelper.MapPath("~/" + folder.TrimStart('~', '/'));
			return new DirectoryInfo(path).GetFiles(filter);
		}
	}
}