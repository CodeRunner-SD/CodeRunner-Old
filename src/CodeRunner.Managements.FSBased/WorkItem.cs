using System;
using System.IO;

namespace CodeRunner.Managements.FSBased
{
    public class WorkItem : IWorkItem
    {
        public static WorkItem CreateByFile(Workspace onwer, FileInfo file) => new WorkItem(onwer, file, null, WorkItemType.File);

        public static WorkItem CreateByDirectory(Workspace onwer, DirectoryInfo dir) => new WorkItem(onwer, null, dir, WorkItemType.Directory);

        private WorkItem(Workspace onwer, FileInfo? file, DirectoryInfo? directory, WorkItemType type)
        {
            Onwer = onwer;
            File = file;
            Directory = directory;
            Type = type;
            Id = Guid.NewGuid();
        }

        private Workspace Onwer { get; }

        public FileInfo? File { get; }

        public DirectoryInfo? Directory { get; }

        public WorkItemType Type { get; }

        public FileSystemInfo Target => Type == WorkItemType.File ? (FileSystemInfo)File! : (Directory!);

        public string RelativePath => Path.GetRelativePath(Onwer.PathRoot.FullName, Target.FullName);

        public Guid Id { get; }

        public string Name => Type == WorkItemType.File ? RelativePath : $"@{RelativePath}";
    }
}
