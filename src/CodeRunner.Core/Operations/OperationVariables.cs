using CodeRunner.Templates;

namespace CodeRunner.Operations
{
    public static class OperationVariables
    {
        public static Variable InputPath => new Variable("inputPath").Required();

        public static Variable OutputPath => new Variable("outputPath").Required();

        public static Variable Shell => new Variable("shell").Required();

        public static Variable WorkingDirectory => new Variable("workingDirectory").NotRequired("");
    }
}
