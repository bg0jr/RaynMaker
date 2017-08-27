using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle( "RaynMaker.Modules.Analysis" )]
[assembly: ComVisible( false )]

[assembly: XmlnsPrefix( "https://github.com/bg0jr/RaynMaker/Analysis", "rma" )]
[assembly: XmlnsDefinition( "https://github.com/bg0jr/RaynMaker/Analysis", "RaynMaker.Modules.Analysis.AnalysisSpec" )]

[assembly: InternalsVisibleTo( "RaynMaker.Modules.Analysis.UnitTests" )]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
)]

