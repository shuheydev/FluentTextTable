﻿using System;

namespace FluentTextTable.Sample._01.BasicForJP
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday;
    }

    public class Sample
    {
        public static void WriteConsole()
        {
            var users = new[]
            {
                new User {Id = 1, Name = "ビル・ゲイツ", Birthday = DateTime.Parse("1955/10/28")},
                new User {Id = 2, Name = "スティーブ・ジョブズ", Birthday = DateTime.Parse("1955/2/24")}
            };
            Build
                .TextTable<User>()
                .WriteLine(users);
        }
    }


}