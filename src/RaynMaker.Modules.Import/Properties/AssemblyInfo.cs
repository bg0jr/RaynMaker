using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

[assembly: AssemblyTitle("RaynMaker.Modules.Import")]
[assembly: ComVisible(false)]
[assembly: Guid("5ef8f3af-198f-42ca-b26f-a3916c625a01")]

[assembly: InternalsVisibleTo( "RaynMaker.Modules.Import.UnitTests" )]
// SDK gives access to dedicated internal APIs for testing only!
[assembly: InternalsVisibleTo( "RaynMaker.SDK" )]

[assembly: XmlnsPrefix( "https://github.com/bg0jr/RaynMaker/Import/Spec", "rymspec" )]
[assembly: XmlnsDefinition( "https://github.com/bg0jr/RaynMaker/Import/Spec", "RaynMaker.Modules.Import.Spec.v2" )]
[assembly: XmlnsDefinition( "https://github.com/bg0jr/RaynMaker/Import/Spec", "RaynMaker.Modules.Import.Spec.v2.Extraction" )]
[assembly: XmlnsDefinition( "https://github.com/bg0jr/RaynMaker/Import/Spec", "RaynMaker.Modules.Import.Spec.v2.Locating" )]

