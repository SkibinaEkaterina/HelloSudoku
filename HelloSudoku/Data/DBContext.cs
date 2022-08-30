using System;
using Microsoft.EntityFrameworkCore;
using HelloSudoku.Models;

namespace HelloSudoku
{
    public class DBContext : DbContext
    {
        public DbSet<SudokuGame> GameInfo => Set<SudokuGame>();

        public DBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite("Data Source=/Users/ekaterina/Projects/HelloSudoku/HelloSudoku/Data/Database/SydokuGame.db");
            optionsBuilder.LogTo(Console.WriteLine);
        }
    }
}

