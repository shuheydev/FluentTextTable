﻿using System.IO;

namespace FluentTextTable
{
    public interface IRow
    {
        int GetCellWidth(IColumn column);
        void Write(TextWriter textWriter, ITextTableLayout textTableLayout);
    }
}