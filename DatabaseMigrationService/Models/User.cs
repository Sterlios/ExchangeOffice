﻿namespace DatabaseMigrationService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public List<UserCurrency> UserCurrencies { get; set; }
    }
}
