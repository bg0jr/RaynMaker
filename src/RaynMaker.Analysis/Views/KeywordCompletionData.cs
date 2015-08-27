using System;

namespace RaynMaker.Blade.Views
{
    class KeywordCompletionData : AbstractCompletionData
    {
        public KeywordCompletionData( Type type )
            : base( type.Name, type.Name )
        {
            Type = type;
        }

        public Type Type { get; private set; }
    }
}
