using PactNet.Infrastructure.Outputters;
using Xunit.Abstractions;

namespace Provider.ContractTests;

public class PactXUnitOutput : IOutput
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PactXUnitOutput(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public void WriteLine(string line)
    {
        _testOutputHelper.WriteLine(line);
    }
}