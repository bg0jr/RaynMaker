using System.Collections.Generic;
using Plainion.AppFw.Wpf.Infrastructure;
using RaynMaker.Entities;

namespace RaynMaker.Infrastructure
{
    public interface IProject
    {
        IAssetsContext GetAssetsContext();

        /// <summary>
        /// Temporary solution to store kind of "user data"
        /// </summary>
        IDictionary<string, string> UserData { get; }

        bool IsDirty { get; set; }
    }
}
