﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RaynMaker.Entities
{
    public class Translation : EntityTimestampBase
    {
        private Currency mySource;
        private Currency myTarget;
        private double myRate;

        public long SourceId { get; set; }

        [Required]
        public Currency Source
        {
            get { return mySource; }
            set
            {
                var old = mySource;
                if( SetProperty( ref mySource, value ) )
                {
                    UpdateTimestamp();
                }
            }
        }

        public long TargetId { get; set; }
        
        [Required]
        public Currency Target
        {
            get { return myTarget; }
            set
            {
                var old = myTarget;
                if( SetProperty( ref myTarget, value ) )
                {
                    UpdateTimestamp();
                }
            }
        }

        [Required]
        public double Rate
        {
            get { return myRate; }
            set
            {
                var old = myRate;
                if( SetProperty( ref myRate, value ) )
                {
                    UpdateTimestamp();
                }
            }
        }
    }
}
