using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public abstract class Account : IAccount
    {
        // Событие, при снятии денег
        protected internal event AccountStateHandler Withdrawed;
        // Событие возникающее при добавление на счет
        protected internal event AccountStateHandler Added;
        // Событие возникающее при открытии счета
        protected internal event AccountStateHandler Opened;
        // Событие возникающее при закрытии счета
        protected internal event AccountStateHandler Closed;
        // Событие возникающее при начислении процентов
        protected internal event AccountStateHandler Calculated;

        protected int _id;
        static int counter = 0;

        protected decimal sum; // Переменная для хранения суммы
        protected int percentage; // Переменная для хранения процента

        protected int days = 0; // время с момента открытия счета

        public int Percentage
        {
            get { return percentage; }
        }
        
        public decimal CurrentSum
        {
            get { return sum; }
        }

        public Account(decimal sum, int percentage)
        {
            this.sum = sum;
            this.percentage = percentage;
            _id = ++counter;
        }

        public int Id
        {
            get { return _id; }
        }
        // Вызов события
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (e != null)
                handler?.Invoke(this, e);           
        }
        // вызов отдельных событий. Для каждого события определяется свой витуальный метод
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }

        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }

        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }
        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }
        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        public virtual void Put(decimal sum)
        {
            this.sum += sum;
            OnAdded(new AccountEventArgs($"На счет поступило {sum}. Ваш баланс: {CurrentSum}. Номер счета: {Id}", sum));
        }        

        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (sum <= this.sum)
            {
                this.sum -= sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs($"Сумма {sum} снята со счета {_id}. Ваш баланс: {CurrentSum}. Номер счета: {Id}", sum));
            }
            else
            {
                OnWithdrawed(new AccountEventArgs($"Недостаточно средств на счете {_id}", 0));
            }
            return result;
        }
        // открытие счета
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs($"Открыт новый счет! Id счета: {_id}", sum));
        }
        // закрытие счета
        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs($"Счет {_id} закрыт. Итоговая сумма: {CurrentSum}", CurrentSum));
        } 

        protected internal void IncrementDays()
        {
            days += 30;
        }

        // Начисление процента
        protected internal virtual void Calculate()
        {
            decimal increment = sum * percentage / 100;
            sum += increment;
            OnCalculated(new AccountEventArgs($"Начислены проценты в размере: {increment}. Ваш баланс: {CurrentSum}. Номер счета: {Id}", increment));
        }
    }
}
