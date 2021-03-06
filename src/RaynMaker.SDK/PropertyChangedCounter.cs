﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace RaynMaker.SDK
{
    public class PropertyChangedCounter
    {
        // key: propertyName, value: raise count
        private Dictionary<string, int> myPropertyChanges;

        public PropertyChangedCounter(INotifyPropertyChanged source)
        {
            myPropertyChanges = new Dictionary<string, int>();

            PropertyChangedEventManager.AddHandler(source, OnPropertyChanged, string.Empty);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!myPropertyChanges.ContainsKey(e.PropertyName))
            {
                myPropertyChanges[e.PropertyName] = 0;
            }

            myPropertyChanges[e.PropertyName]++;
        }

        public int GetCount(string propertyName)
        {
            int count = 0;
            myPropertyChanges.TryGetValue(propertyName, out count);

            return count;
        }
    }
}
