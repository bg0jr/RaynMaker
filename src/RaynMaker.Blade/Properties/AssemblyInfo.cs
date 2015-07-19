using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle( "RaynMaker.Blade" )]
[assembly: ComVisible( false )]

[assembly: XmlnsPrefix( "https://github.com/bg0jr/RaynMaker/Analysis", "rma" )]
[assembly: XmlnsPrefix( "https://github.com/bg0jr/RaynMaker/Data", "rmd" )]
[assembly: XmlnsDefinition( "https://github.com/bg0jr/RaynMaker/Analysis", "RaynMaker.Blade.AnalysisSpec" )]
[assembly: XmlnsDefinition( "https://github.com/bg0jr/RaynMaker/Data", "RaynMaker.Blade.DataSheetSpec" )]
[assembly: XmlnsDefinition( "https://github.com/bg0jr/RaynMaker/Data", "RaynMaker.Blade.DataSheetSpec.Datums" )]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
)]

