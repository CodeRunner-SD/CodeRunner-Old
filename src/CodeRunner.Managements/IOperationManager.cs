using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Packagings;

namespace CodeRunner.Managements
{
    public interface IOperationManager : IItemManager<OperationSettings, Package<BaseOperation>>
    {

    }
}
