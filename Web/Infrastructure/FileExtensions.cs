using System.IO;
using Core;

namespace Web.Infrastructure
{
	public static class FileExtensions
	{
		public static FileInfo GetNestedFile(this DirectoryInfo dir, string path) {
			return new FileInfo("{0}/{1}".Fmt(dir.FullName, path));
		}
	}
}