using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLayer;
namespace BuisnessLayer
{
    public class BuisnessLayerImplementation
    {
        public string GetStringBuilder(StringBuilder strBldr, List<int> UserId, int CurrentUser, out bool SharedWithMe, ref int totalSharedUser)
        {
            bool flag = false;
            for (int i = 0; i <= UserId.Count - 1; i++)
            {
                if (UserId[i] == Convert.ToInt32(CurrentUser))
                {
                    flag = true;
                }
                
                if (i != UserId.Count - 1 && UserId[i] != Convert.ToInt32(CurrentUser))
                {
                    strBldr.Append(UserId[i].ToString() + ',');
                    totalSharedUser++;
                }
                else if (UserId[i] != Convert.ToInt32(CurrentUser))
                {
                    strBldr.Append(UserId[i].ToString());
                    totalSharedUser++;
                }

            }
            if (strBldr[strBldr.Length - 1] == ',')
            {
                strBldr.Remove(strBldr.Length - 1, 1);
            }
            SharedWithMe = flag;
            return strBldr.ToString();
        }
        public void GetBalance(bool IsSharedWithMe, out decimal myBalance, out decimal otherBalance, int totalSharedUser, decimal Price)
        {
            if (IsSharedWithMe)
            {
                myBalance = (Price / (totalSharedUser + 1)) * totalSharedUser;
                otherBalance = -(Price / (totalSharedUser + 1));
            }
            else
            {
                myBalance = Price;
                otherBalance = -(Price / (totalSharedUser));
            }
        }
        public Dictionary<int, decimal> GetBalanceIndividual(List<decimal?>Balances, List<int>UserId, decimal CurrentUserBalance, int CurrentUserId)
        {
            decimal totalSumBalance = 0;
            foreach (decimal Balance in Balances)
            {
                if (Balance >= 0)
                {
                    totalSumBalance = totalSumBalance + Balance;
                }
            }
            Dictionary<int, decimal> indivisualBalance = new Dictionary<int, decimal>();
            if (CurrentUserBalance < 0)
            {
                decimal tempBalance;
                for (int i = 0; i <= Balances.Count - 1; i++)
                {
                    if (Balances[i] >= 0 && UserId[i] != CurrentUserId)
                    {
                        tempBalance = (decimal)((Balances[i] / totalSumBalance) * CurrentUserBalance);
                        indivisualBalance.Add(UserId[i], tempBalance);
                    }
                }
            }
            else 
            {
                decimal tempBalance;
                for (int i = 0; i <= Balances.Count - 1; i++)
                {
                    if (Balances[i] < 0 && UserId[i] != CurrentUserId)
                    {
                        tempBalance = (decimal)((CurrentUserBalance / totalSumBalance) * Balances[i]);
                        indivisualBalance.Add(UserId[i], tempBalance);
                    }
                }
            }
            
            return indivisualBalance;
        }

    }
}
