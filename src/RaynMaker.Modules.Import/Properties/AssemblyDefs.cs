using System.Runtime.CompilerServices;
using System.Windows.Markup;

[assembly: InternalsVisibleTo("RaynMaker.Modules.Import.UnitTests")]
// SDK gives access to dedicated internal APIs for testing only!
[assembly: InternalsVisibleTo("RaynMaker.SDK")]

[assembly: XmlnsPrefix("https://github.com/bg0jr/RaynMaker/Import/Spec", "rymspec")]
[assembly: XmlnsDefinition("https://github.com/bg0jr/RaynMaker/Import/Spec", "RaynMaker.Modules.Import.Spec.v2")]
[assembly: XmlnsDefinition("https://github.com/bg0jr/RaynMaker/Import/Spec", "RaynMaker.Modules.Import.Spec.v2.Extraction")]
[assembly: XmlnsDefinition("https://github.com/bg0jr/RaynMaker/Import/Spec", "RaynMaker.Modules.Import.Spec.v2.Locating")]

