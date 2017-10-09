using System.Collections.Generic;

namespace Web.Infrastructure.FileManager
{
    public class MergeFileManager
    {
        private static MergeFileManager _instance;
        private readonly List<string> MergeFileList;

        private MergeFileManager()
        {
            try
            {
                MergeFileList = new List<string>();
            }
            catch
            {
                // ignored
            }
        }

        public static MergeFileManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MergeFileManager();
                return _instance;
            }
        }

        public void AddFile(string BaseFileName)
        {
            MergeFileList.Add(BaseFileName);
        }

        public bool InUse(string BaseFileName)
        {
            return MergeFileList.Contains(BaseFileName);
        }

        public bool RemoveFile(string BaseFileName)
        {
            return MergeFileList.Remove(BaseFileName);
        }
    }
}
