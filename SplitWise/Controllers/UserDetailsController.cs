using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer.Models;
using BuisnessLayer;
namespace SplitWise.Controllers
{
    
    [Authorize]

    public class UserDetailsController : Controller
    {
        
        
        public ActionResult Welcome()
        {
            SplitwiseEntities db = new SplitwiseEntities();
            
            
            IEnumerable<MonthlyExpenditure> monthLyExpenditures = db.MonthlyExpenditures.ToList();

            ViewData["Balances"] = db.tbl_Balance.ToList();
            db.Dispose();

            return View(monthLyExpenditures);
        }
        
        public ActionResult Create()
        {
            SplitwiseEntities db = new SplitwiseEntities();
            List<SplitwiseUser> users = db.SplitwiseUsers.ToList();
            ViewData["ListOfUser"] = users.ToList() ;
            return View();
        }
        [HttpPost]
        public ActionResult Create(MonthlyExpenditure monthlyExp, List<int> UserIdCheck)
        {
            SplitwiseEntities db = new SplitwiseEntities();
            List<SplitwiseUser> users = db.SplitwiseUsers.ToList();
            ViewData["ListOfUser"] = users.ToList();
            StringBuilder strBldr = new StringBuilder();
            bool IsSharedWithMe = false;
            int totalSharedUser = 0;
            int currentUserID = Convert.ToInt32(Session["UserID"]);
            if (UserIdCheck == null)
            {
                return Content("Please add shared with Expenses");
            }
            BuisnessLayerImplementation bsI = new BuisnessLayerImplementation();
            if (ModelState.IsValid)
            {
                
                using (TransactionScope transaction = new TransactionScope())
                {
                    //monthlyExp.UserId = Session["UserID"] as Int32?;
                    try
                    {
                        monthlyExp.SharedWith = bsI.GetStringBuilder(strBldr, UserIdCheck, currentUserID, out IsSharedWithMe, ref totalSharedUser);
                        decimal myBalance =0;
                        int myId = Convert.ToInt32(Session["UserID"]);
                        string otherIDs = strBldr.ToString();
                        decimal otherBalance = 0;
                        decimal Price = (decimal)monthlyExp.Price;
                        bsI.GetBalance(IsSharedWithMe, out myBalance, out otherBalance, totalSharedUser,Price);
                        db.InsertListItems(monthlyExp.Item, monthlyExp.DateBought, monthlyExp.Price, (int?) currentUserID, monthlyExp.SharedWith, (bool?)IsSharedWithMe);
                        db.UpdateBalances((decimal?)myBalance, (int?)myId, otherIDs, (decimal?)otherBalance);
                        db.SaveChanges();
                        transaction.Complete();
                        
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message);
                    }
                    finally
                    {

                        db.Database.Connection.Close();
                        db.Dispose();
                        
                    }
                }
                return RedirectToAction("Welcome", "UserDetails");
            }
            else
            {
                return View();
            }
            
        }
        public ActionResult Details(int id)
        {
            SplitwiseEntities db = new SplitwiseEntities();
            MonthlyExpenditure monthlyExp = db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id);
            
            db.Dispose();
            return View(monthlyExp);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            
            SplitwiseEntities db = new SplitwiseEntities();
            
            MonthlyExpenditure monthlyExp = db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id);
            if (monthlyExp.UserId != Convert.ToInt32(Session["UserID"]))
            {
                return Content("You donot have access");
            }
            ViewData["ListOfUserEdit"] = db.SplitwiseUsers.ToList();
            ViewData["StringListIds"] = db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id).SharedWith.Split(',').ToList();
            ViewData["IsSharedWithMe"] = db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id).SharedWithMe;
            db.Dispose();
            return View(monthlyExp);
        }

        [HttpPost]
        public ActionResult Edit(int id, MonthlyExpenditure monthlyExp, List<int> UserIdCheck)
        {
            BuisnessLayerImplementation bsI = new BuisnessLayerImplementation();
            SplitwiseEntities db = new SplitwiseEntities();
            ViewData["ListOfUserEdit"] = db.SplitwiseUsers.ToList();
            string InitialSharedUser = db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id).SharedWith;
            int InitialSharedUserCount = InitialSharedUser.Split(',').Length;
            decimal InitialPrice = (decimal)db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id).Price;
            bool WasSharedWithMe = (bool)db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id).SharedWithMe;
            decimal myInitialBalance = 0;
            decimal otherInitialBalance = 0;
            bsI.GetBalance(WasSharedWithMe, out myInitialBalance, out otherInitialBalance, InitialSharedUserCount, InitialPrice);

            //return Content("So far so good");
            StringBuilder strBldr = new StringBuilder();
            int currentUserID = Convert.ToInt32(Session["UserID"]);
            bool IsSharedWithMe = false;
            int totalSharedUser = 0;
            
            if (ModelState.IsValid)
            {

                using (TransactionScope transaction = new TransactionScope())
                {
                    //monthlyExp.UserId = Session["UserID"] as Int32?;
                    try
                    {
                        monthlyExp.SharedWith = bsI.GetStringBuilder(strBldr, UserIdCheck, currentUserID, out IsSharedWithMe, ref totalSharedUser);
                        decimal myBalance = 0;
                        int myId = Convert.ToInt32(Session["UserID"]);
                        string otherIDs = strBldr.ToString();
                        decimal otherBalance = 0;
                        decimal Price = (decimal)monthlyExp.Price;
                        bsI.GetBalance(IsSharedWithMe, out myBalance, out otherBalance, totalSharedUser, Price);
                        db.UpdateBalances((decimal?)(-myInitialBalance), (int?)myId, InitialSharedUser, (decimal?)-otherInitialBalance);
                        db.UpdateListItems((int?)id, monthlyExp.Item, monthlyExp.DateBought, monthlyExp.Price, (int?)myId, monthlyExp.SharedWith, (bool?)IsSharedWithMe);
                        db.UpdateBalances((decimal?)myBalance, (int?)myId, otherIDs, (decimal?)otherBalance);
                        db.SaveChanges();
                        transaction.Complete();

                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message);
                    }
                    finally
                    {

                        db.Database.Connection.Close();
                        db.Dispose();

                    }
                }
                return RedirectToAction("Welcome", "UserDetails");
            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            SplitwiseEntities db = new SplitwiseEntities();

            MonthlyExpenditure monthlyExp = db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id);
            if (monthlyExp.UserId != Convert.ToInt32(Session["UserID"]))
            {
                return Content("You donot have access");
            }
            ViewData["ListOfUserEdit"] = db.SplitwiseUsers.ToList();

            db.Dispose();
            return View(monthlyExp);
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Delete_Post(int id)
        {
            BuisnessLayerImplementation bsI = new BuisnessLayerImplementation();
            SplitwiseEntities db = new SplitwiseEntities();
            ViewData["ListOfUserEdit"] = db.SplitwiseUsers.ToList();
            MonthlyExpenditure monthlyExp = db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id);
            string InitialSharedUser = db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id).SharedWith;
            int InitialSharedUserCount = InitialSharedUser.Split(',').Length;
            decimal InitialPrice = (decimal)db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id).Price;
            bool WasSharedWithMe = (bool)db.MonthlyExpenditures.FirstOrDefault(monE => monE.ExpID == id).SharedWithMe;
            decimal myInitialBalance = 0;
            decimal otherInitialBalance = 0;
            bsI.GetBalance(WasSharedWithMe, out myInitialBalance, out otherInitialBalance, InitialSharedUserCount, InitialPrice);
            int myId = Convert.ToInt32(Session["UserID"]);
            using (TransactionScope transaction = new TransactionScope())
            {
                //monthlyExp.UserId = Session["UserID"] as Int32?;
                try
                {

                    db.UpdateBalances((decimal?)(-myInitialBalance), (int?)myId, InitialSharedUser, (decimal?)-otherInitialBalance);
                    db.MonthlyExpenditures.Remove(monthlyExp);
                    db.SaveChanges();
                    transaction.Complete();

                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
                finally
                {

                    db.Database.Connection.Close();
                    db.Dispose();

                }
            }
            return RedirectToAction("Welcome", "UserDetails");
        }

        public ActionResult GetIndividualBalance()
        {
            BuisnessLayerImplementation bsI = new BuisnessLayerImplementation();
            SplitwiseEntities db = new SplitwiseEntities();
            List<decimal?> Balances =  db.tbl_Balance.Select(bal => bal.Balance).ToList();
            List<int> UserId = db.tbl_Balance.Select(bal => bal.UserId).ToList();
            int myId = Convert.ToInt32(Session["UserID"]);
            decimal? CurrentUserBalance = db.tbl_Balance.FirstOrDefault(bal => bal.UserId == myId).Balance;
            int CurrentUserId = Convert.ToInt32(Session["UserID"]);
            Dictionary<int, decimal> individualBalance = bsI.GetBalanceIndividual(Balances, UserId, (decimal)CurrentUserBalance, CurrentUserId);
            ViewBag.CurrentUserBalanceViewBag = CurrentUserBalance;
            return PartialView("_IndividualBalance", individualBalance);
        }
    }
}
