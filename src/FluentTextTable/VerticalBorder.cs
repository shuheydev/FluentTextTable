﻿using System.IO;

namespace FluentTextTable
{
    internal class VerticalBorder : BorderBase
    {
        internal VerticalBorder(bool isEnable, char lineStyle) : base(isEnable, lineStyle)
        {
        }

        internal void Write(ITextTableWriter writer)
        {
            if(IsEnable) writer.Write(_lineStyle);
        }
    }
}