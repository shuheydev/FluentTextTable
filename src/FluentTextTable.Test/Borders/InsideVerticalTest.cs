﻿using System;
using Xunit;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FluentTextTable.Test.Borders
{
    public class InsideVerticalTest
    {
        [Fact]
        public void WhenDisable()
        {

            var table = Build.TextTable<User>(builder =>
            {
                builder.Borders.InsideVertical.AsDisable();
            });
            var text = table.ToString(new[]
            {
                new User {Id = 1, Name = "ビル ゲイツ", Birthday = DateTime.Parse("1955/10/28")},
                new User {Id = 2, Name = "Steven Jobs", Birthday = DateTime.Parse("1955/2/24")}
            });

            Assert.Equal(
                @"
 +-----------------------------------------------------------+
 | Id  Name         Parents  Occupations  Birthday           |
 +-----------------------------------------------------------+
 | 1   ビル ゲイツ                        1955/10/28 0:00:00 |
 +-----------------------------------------------------------+
 | 2   Steven Jobs                        1955/02/24 0:00:00 |
 +-----------------------------------------------------------+
", $"{Environment.NewLine}{text}");
        }
            
        [Fact]
        public void WhenChangeDecorations()
        {

            var table = Build.TextTable<User>(builder =>
            {
                builder.Borders.InsideVertical.LineStyleAs("\\\\");
                builder.Borders.Top.IntersectionStyleAs("12");
                builder.Borders.HeaderHorizontal.IntersectionStyleAs("34");
                builder.Borders.InsideHorizontal.IntersectionStyleAs("56");
                builder.Borders.Bottom.IntersectionStyleAs("78");
            });
            var text = table.ToString(new[]
            {
                new User {Id = 1, Name = "ビル ゲイツ", Birthday = DateTime.Parse("1955/10/28")},
                new User {Id = 2, Name = "Steven Jobs", Birthday = DateTime.Parse("1955/2/24")}
            });

            Assert.Equal(
                @"
 +----12-------------12---------12-------------12--------------------+
 | Id \\ Name        \\ Parents \\ Occupations \\ Birthday           |
 +----34-------------34---------34-------------34--------------------+
 | 1  \\ ビル ゲイツ \\         \\             \\ 1955/10/28 0:00:00 |
 +----56-------------56---------56-------------56--------------------+
 | 2  \\ Steven Jobs \\         \\             \\ 1955/02/24 0:00:00 |
 +----78-------------78---------78-------------78--------------------+
", $"{Environment.NewLine}{text}");
        }
        
        [Fact]
        public void WhenVerticalLineStyleWidthIsUnmatched()
        {
            Assert.Throws<InvalidOperationException>(() => 
                Build.TextTable<User>(builder =>
                {
                    builder.Borders.InsideVertical.LineStyleAs("12");
                }));
        }

        [Fact]
        public void WhenTopInsideVerticalWidthIsUnmatched()
        {
            Assert.Throws<InvalidOperationException>(() => 
                Build.TextTable<User>(builder =>
                {
                    builder.Borders.Top.IntersectionStyleAs("12");
                }));
        }
            
        [Fact]
        public void WhenHeaderHorizontalInsideVerticalWidthIsUnmatched()
        {
            Assert.Throws<InvalidOperationException>(() => 
                Build.TextTable<User>(builder =>
                {
                    builder.Borders.HeaderHorizontal.IntersectionStyleAs("12");
                }));
        }
            
        [Fact]
        public void WhenInsideHorizontalInsideVerticalWidthIsUnmatched()
        {
            Assert.Throws<InvalidOperationException>(() => 
                Build.TextTable<User>(builder =>
                {
                    builder.Borders.InsideHorizontal.IntersectionStyleAs("12");
                }));
        }
            
        [Fact]
        public void WhenBottomInsideVerticalWidthIsUnmatched()
        {
            Assert.Throws<InvalidOperationException>(() => 
                Build.TextTable<User>(builder =>
                {
                    builder.Borders.Bottom.IntersectionStyleAs("12");
                }));
        }
        
        private class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Birthday;
            public string Parents { get; set; }
            public string[] Occupations { get; set; }

        }
    }
}