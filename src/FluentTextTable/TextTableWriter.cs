﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FluentTextTable
{
    public class TextTableWriter<TItem> : ITextTableWriter<TItem>
    {
        private readonly List<Column<TItem>> _columns;
        private readonly Headers<TItem> _headers;
        private readonly Borders _borders;

        private TextTableWriter(TextTableConfig<TItem> config, List<Column<TItem>> columns)
        {
            _columns = columns;
            _borders = config.BuildBorders();
            _headers = new Headers<TItem>(_columns, _borders);
        }

        public string ToString(IEnumerable<TItem> items)
        {
            var writer = new StringWriter();
            Write(writer, items);
            return writer.ToString();
        }

        public void Write(TextWriter writer, IEnumerable<TItem> items)
        {
            var body = new Body<TItem>(_columns, _borders, items);
            var table = new TextTable<TItem>(_columns, _headers, body, _borders);
            
            WriteHorizontalBorder(writer, _borders.Top, table);
            WriteHeader(writer, table);
            WriteHorizontalBorder(writer, _borders.HeaderHorizontal, table);
            WriteBody(writer, table);
            WriteHorizontalBorder(writer, _borders.Bottom, table);
        }
        
        private void WriteHorizontalBorder(TextWriter textWriter, HorizontalBorder border, ITextTable<TItem> table)
        {
            if(!border.IsEnable) return;
            
            if(_borders.Left.IsEnable) textWriter.Write(border.LeftStyle);
            var items = new List<string>();
            foreach (var column in _columns)
            {
                items.Add(new string(border.LineStyle, table.GetColumnWidth(column)));
            }

            textWriter.Write(_borders.InsideVertical.IsEnable
                ? string.Join(border.IntersectionStyle.ToString(), items)
                : string.Join(string.Empty, items));

            if(_borders.Right.IsEnable) textWriter.Write(border.RightStyle);
            
            textWriter.WriteLine();
        }

        private void WriteHeader(TextWriter writer, ITextTable<TItem> table)
        {
            _borders.Left.Write(writer);
            
            _columns[0].WriteHeader(writer, table);
            writer.Write(" ");

            for (var i = 1; i < _columns.Count; i++)
            {
                _borders.InsideVertical.Write(writer);

                _columns[i].WriteHeader(writer, table);
                writer.Write(" ");
            }
            
            _borders.Right.Write(writer);

            writer.WriteLine();
        }

        private void WriteBody(TextWriter textWriter, TextTable<TItem> table)
        {
            if (table.Body.Rows.Any())
            {
                table.Body.Rows[0].WritePlanText(textWriter, table);
                for (var i = 1; i < table.Body.Rows.Count; i++)
                {
                    WriteHorizontalBorder(textWriter, _borders.InsideHorizontal, table);
                    table.Body.Rows[i].WritePlanText(textWriter, table);
                }
            }
        }

        public static TextTableWriter<TItem> Build()
        {
            var config = new TextTableConfig<TItem>();
            AddColumns(config);
            return new TextTableWriter<TItem>(config, config.FixColumnSpecs());
        }

        public static TextTableWriter<TItem> Build(Action<ITextTableConfig<TItem>> configure)
        {
            var config = new TextTableConfig<TItem>();
            configure(config);
            if (config.AutoGenerateColumns)
            {
                AddColumns(config);
            }
            return new TextTableWriter<TItem>(config, config.FixColumnSpecs());
        }
        
        private static void AddColumns(TextTableConfig<TItem> config)
        {
            var memberInfos =
                typeof(TItem).GetMembers(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.MemberType == MemberTypes.Field || x.MemberType == MemberTypes.Property);
            var members = new List<(int index, MemberInfo memberInfo, ColumnFormatAttribute columnFormat)>();
            foreach (var memberInfo in memberInfos)
            {
                var columnFormat = memberInfo.GetCustomAttribute<ColumnFormatAttribute>();
                if (columnFormat is null)
                {
                    members.Add((0, memberInfo, null));
                }

                if (columnFormat != null)
                {
                    members.Add((columnFormat.Index, memberInfo, columnFormat));
                }
            }

            foreach (var member in members.OrderBy(x => x.index))
            {
                var column = config.AddColumn(member.memberInfo);
                if (member.columnFormat != null)
                {
                    if (member.columnFormat.Header != null) column.NameIs(member.columnFormat.Header);
                    column
                        .AlignHorizontalTo(member.columnFormat.HorizontalAlignment)
                        .AlignVerticalTo(member.columnFormat.VerticalAlignment)
                        .FormatTo(member.columnFormat.Format);
                }
            }
        }
    }
}