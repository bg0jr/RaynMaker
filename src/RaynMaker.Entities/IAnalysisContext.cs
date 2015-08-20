using System;
using System.Data.Entity;

namespace RaynMaker.Entities
{
    public interface IAnalysisContext 
    {
        IDbSet<AnalysisTemplate> AnalysisTemplates { get; }

        int SaveChanges();
    }
}
