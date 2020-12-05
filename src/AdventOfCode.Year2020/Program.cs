using AdventOfCode.Lib;
using Microsoft.Extensions.DependencyInjection;
using static AdventOfCode.Lib.BaseProgram;

var services = ConfigureServices();
var serviceProvider = services.BuildServiceProvider();
var solver = serviceProvider.GetRequiredService<Solver>();
await RunSolver(args, solver);
