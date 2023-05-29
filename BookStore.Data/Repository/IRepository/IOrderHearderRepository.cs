using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IOrderHearderRepository:IRepository<OrderHeader>
    {
        public void update(OrderHeader orderHeader);
        public void updateStatus(int id, string orderstatus, string? PaymentStatus = null);
        public void updateStripePaymentID(int id, string sessionId, string PaymentIntentId);
        public void Save();
    }
}
