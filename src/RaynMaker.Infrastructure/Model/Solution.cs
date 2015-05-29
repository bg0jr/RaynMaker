using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace RaynMaker.Infrastructure.Model
{
    /// <summary>
    /// Root entity for all loaded projects
    /// </summary>
    [Export]
    public class Solution
    {
        public Solution()
        {
            Projects = new ObservableCollection<IProject>();
        }

        public ObservableCollection<IProject> Projects { get; private set; }
    }
}
