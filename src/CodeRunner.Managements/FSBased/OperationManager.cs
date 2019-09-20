﻿using CodeRunner.Managements.FSBased.Templates;
using CodeRunner.Operations;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements.FSBased
{
    public class OperationManager : ItemManager<OperationSettings, BaseOperation>, IOperationManager
    {
        public OperationManager(DirectoryInfo pathRoot) : base(pathRoot, new System.Lazy<CodeRunner.Templates.DirectoryTemplate>(() => new OperationsSpaceTemplate()))
        {
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await Set("hello", Templates.OperationsSpaceTemplate.Hello);
        }
    }
}
