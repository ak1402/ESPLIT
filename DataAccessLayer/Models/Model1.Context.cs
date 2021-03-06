﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class SplitwiseEntities : DbContext
    {
        public SplitwiseEntities()
            : base("name=SplitwiseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<SplitwiseUser> SplitwiseUsers { get; set; }
        public DbSet<SplitWiseUserRoles> SplitWiseUserRoles { get; set; }
        public DbSet<MonthlyExpenditure> MonthlyExpenditures { get; set; }
        public DbSet<tbl_Balance> tbl_Balance { get; set; }
    
        public virtual int InsertListItemsAndBalances(Nullable<decimal> myBalance, Nullable<int> myId, string otherIDs, Nullable<decimal> otherBalance)
        {
            var myBalanceParameter = myBalance.HasValue ?
                new ObjectParameter("MyBalance", myBalance) :
                new ObjectParameter("MyBalance", typeof(decimal));
    
            var myIdParameter = myId.HasValue ?
                new ObjectParameter("MyId", myId) :
                new ObjectParameter("MyId", typeof(int));
    
            var otherIDsParameter = otherIDs != null ?
                new ObjectParameter("OtherIDs", otherIDs) :
                new ObjectParameter("OtherIDs", typeof(string));
    
            var otherBalanceParameter = otherBalance.HasValue ?
                new ObjectParameter("otherBalance", otherBalance) :
                new ObjectParameter("otherBalance", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertListItemsAndBalances", myBalanceParameter, myIdParameter, otherIDsParameter, otherBalanceParameter);
        }
    
        public virtual int InsertListItems(string item, Nullable<System.DateTime> dateBought, Nullable<decimal> price, Nullable<int> userId, string sharedWith, Nullable<bool> sharedWithMe)
        {
            var itemParameter = item != null ?
                new ObjectParameter("Item", item) :
                new ObjectParameter("Item", typeof(string));
    
            var dateBoughtParameter = dateBought.HasValue ?
                new ObjectParameter("DateBought", dateBought) :
                new ObjectParameter("DateBought", typeof(System.DateTime));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var sharedWithParameter = sharedWith != null ?
                new ObjectParameter("SharedWith", sharedWith) :
                new ObjectParameter("SharedWith", typeof(string));
    
            var sharedWithMeParameter = sharedWithMe.HasValue ?
                new ObjectParameter("SharedWithMe", sharedWithMe) :
                new ObjectParameter("SharedWithMe", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertListItems", itemParameter, dateBoughtParameter, priceParameter, userIdParameter, sharedWithParameter, sharedWithMeParameter);
        }
    
        public virtual int UpdateBalances(Nullable<decimal> myBalance, Nullable<int> myId, string otherIDs, Nullable<decimal> otherBalance)
        {
            var myBalanceParameter = myBalance.HasValue ?
                new ObjectParameter("MyBalance", myBalance) :
                new ObjectParameter("MyBalance", typeof(decimal));
    
            var myIdParameter = myId.HasValue ?
                new ObjectParameter("MyId", myId) :
                new ObjectParameter("MyId", typeof(int));
    
            var otherIDsParameter = otherIDs != null ?
                new ObjectParameter("OtherIDs", otherIDs) :
                new ObjectParameter("OtherIDs", typeof(string));
    
            var otherBalanceParameter = otherBalance.HasValue ?
                new ObjectParameter("otherBalance", otherBalance) :
                new ObjectParameter("otherBalance", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateBalances", myBalanceParameter, myIdParameter, otherIDsParameter, otherBalanceParameter);
        }
    
        public virtual int UpdateListItems(Nullable<int> expID, string item, Nullable<System.DateTime> dateBought, Nullable<decimal> price, Nullable<int> userId, string sharedWith, Nullable<bool> sharedWithMe)
        {
            var expIDParameter = expID.HasValue ?
                new ObjectParameter("ExpID", expID) :
                new ObjectParameter("ExpID", typeof(int));
    
            var itemParameter = item != null ?
                new ObjectParameter("Item", item) :
                new ObjectParameter("Item", typeof(string));
    
            var dateBoughtParameter = dateBought.HasValue ?
                new ObjectParameter("DateBought", dateBought) :
                new ObjectParameter("DateBought", typeof(System.DateTime));
    
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var sharedWithParameter = sharedWith != null ?
                new ObjectParameter("SharedWith", sharedWith) :
                new ObjectParameter("SharedWith", typeof(string));
    
            var sharedWithMeParameter = sharedWithMe.HasValue ?
                new ObjectParameter("SharedWithMe", sharedWithMe) :
                new ObjectParameter("SharedWithMe", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateListItems", expIDParameter, itemParameter, dateBoughtParameter, priceParameter, userIdParameter, sharedWithParameter, sharedWithMeParameter);
        }
    
        public virtual int updt_User(Nullable<int> userID, string userName, string userPassword, Nullable<int> userRole)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var userPasswordParameter = userPassword != null ?
                new ObjectParameter("UserPassword", userPassword) :
                new ObjectParameter("UserPassword", typeof(string));
    
            var userRoleParameter = userRole.HasValue ?
                new ObjectParameter("UserRole", userRole) :
                new ObjectParameter("UserRole", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("updt_User", userIDParameter, userNameParameter, userPasswordParameter, userRoleParameter);
        }
    }
}
