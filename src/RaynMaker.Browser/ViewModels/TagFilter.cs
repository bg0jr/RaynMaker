using RaynMaker.Entities;

namespace RaynMaker.Browser.ViewModels
{
    public class TagFilter
    {
        public static string Blank = "<blank>";

        public TagFilter( string name )
        {
            Name = name;
            IsChecked = true;
        }

        public TagFilter( Tag tag )
            : this( tag.Name )
        {
        }

        public TagFilter( TagFilter filter )
            : this( filter.Name )
        {
            IsChecked = filter.IsChecked;
        }

        public string Name { get; private set; }

        public bool IsChecked { get; set; }
    }
}
