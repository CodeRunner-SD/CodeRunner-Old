using CodeRunner.Diagnostics;
using CodeRunner.Templates;

namespace CodeRunner.Operations
{
    public static class OperationVariables
    {
        public static Variable VarInputPath => new Variable("inputPath").Required();

        public static Variable VarOutputPath => new Variable("outputPath").Required();

        public static Variable VarShell => new Variable("shell").Required();

        public static Variable VarWorkingDirectory => new Variable("workingDirectory").NotRequired("");

        public static string GetInputPath(this ResolveContext context)
        {
            Assert.IsNotNull(context);
            return context.GetVariable<string>(VarInputPath);
        }

        public static string GetOutputPath(this ResolveContext context)
        {
            Assert.IsNotNull(context);
            return context.GetVariable<string>(VarOutputPath);
        }

        public static string GetShell(this ResolveContext context)
        {
            Assert.IsNotNull(context);
            return context.GetVariable<string>(VarShell);
        }

        public static string GetWorkingDirectory(this ResolveContext context)
        {
            Assert.IsNotNull(context);
            return context.GetVariable<string>(VarWorkingDirectory);
        }

        public static ResolveContext SetInputPath(this ResolveContext context, string value)
        {
            Assert.IsNotNull(context);
            return context.WithVariable(VarInputPath.Name, value);
        }

        public static ResolveContext SetOutputPath(this ResolveContext context, string value)
        {
            Assert.IsNotNull(context);
            return context.WithVariable(VarOutputPath.Name, value);
        }

        public static ResolveContext SetWorkingDirectory(this ResolveContext context, string value)
        {
            Assert.IsNotNull(context);
            return context.WithVariable(VarWorkingDirectory.Name, value);
        }

        public static ResolveContext SetShell(this ResolveContext context, string value)
        {
            Assert.IsNotNull(context);
            return context.WithVariable(VarShell.Name, value);
        }
    }
}
