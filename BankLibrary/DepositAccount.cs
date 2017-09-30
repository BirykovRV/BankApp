﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    class DepositAccount : Account
    {
        public DepositAccount(decimal sum, int percentage) : base(sum, percentage)
        {
        }

        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Открыт новый депозитный счет! Id счета: {_id}", sum));
        }

        public override void Put(decimal sum)
        {
            if (days % 30 == 0)
            {
                base.Put(sum);
            }
            else
            {
                base.OnAdded(new AccountEventArgs($"На счет можно положить только после 30 дневного периода", 0));
            }
        }

        public override decimal Withdraw(decimal sum)
        {
            if (days % 30 == 0)
            {
                return base.Withdraw(sum);
            }
            else
            {
                base.OnWithdrawed(new AccountEventArgs($"Вывести средства можно только после 30-ти дневного периода", 0));
            }
            return 0;
        }

        protected internal override void Calculate()
        {
            if (days % 30 == 0)
            {
                base.Calculate();
            }            
        }
    }
}
