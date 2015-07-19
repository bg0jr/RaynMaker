using System;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Translation
    {
        private Currency myTarget;

        public string To { get; set; }

        public Currency Target
        {
            get
            {
                if( myTarget == null )
                {
                    myTarget = Currencies.Parse( To );
                }

                return myTarget;
            }
        }

        public DateTime Timestamp { get; set; }

        public double Rate { get; set; }
    }
}
