﻿/*************************************************************************************************
  Required Notice: Copyright (C) EPPlus Software AB. 
  This software is licensed under PolyForm Noncommercial License 1.0.0 
  and may only be used for noncommercial purposes 
  https://polyformproject.org/licenses/noncommercial/1.0.0/

  A commercial license to use this software can be purchased at https://epplussoftware.com
 *************************************************************************************************
  Date               Author                       Change
 *************************************************************************************************
  10/15/2020         EPPlus Software AB       ToDataTable function
 *************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OfficeOpenXml.Export.ToDataTable
{
    public class DataColumnMappingCollection : List<DataColumnMapping>
    {
        private readonly Dictionary<int, DataColumnMapping> _mappingIndexes = new Dictionary<int, DataColumnMapping>();
        internal void Validate()
        {
            foreach(var mapping in this)
            {
                mapping.Validate();
            }
        }

        public void Add(int zeroBasedIndexInRange, string dataColumnName)
        {
            Add(zeroBasedIndexInRange, dataColumnName, null);
        }

        public void Add(int zeroBasedIndexInRange, string dataColumnName, Type dataColumnType)
        {
            Add(zeroBasedIndexInRange, dataColumnName, dataColumnType, true);
        }

        public void Add(int zeroBasedIndexInRange, string dataColumnName, Type dataColumnType, bool allowNull)
        {
            var mapping = new DataColumnMapping
            {
                ZeroBasedColumnIndexInRange = zeroBasedIndexInRange,
                DataColumnName = dataColumnName,
                DataColumnType = dataColumnType,
                AllowNull = allowNull
            };
            mapping.Validate();
            if (this.Any(x => x.ZeroBasedColumnIndexInRange == zeroBasedIndexInRange)) throw new InvalidOperationException("Duplicate index in range: " + zeroBasedIndexInRange);
            _mappingIndexes[mapping.ZeroBasedColumnIndexInRange] = mapping;
            Add(mapping);
            Sort((x, y) => x.ZeroBasedColumnIndexInRange.CompareTo(y.ZeroBasedColumnIndexInRange));
        }

        internal DataColumnMapping GetByRangeIndex(int index)
        {
            if (!_mappingIndexes.ContainsKey(index)) throw new ArgumentOutOfRangeException("Index");
            return _mappingIndexes[index];
        }

        internal bool ContainsMapping(int index)
        {
            return _mappingIndexes.ContainsKey(index);
        }
    }
}