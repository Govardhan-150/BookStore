using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using OnlineBookStoreApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHearderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void update(OrderHeader orderHeader)
        {
            _db.Update(orderHeader);
        }

        public void updateStatus(int id, string orderstatus, string? PaymentStatus = null)
        {
            var  orderHeaderFromDb=_db.OrederHeader.FirstOrDefault(u=>u.Id==id);
            if(orderHeaderFromDb != null)
            {
                orderHeaderFromDb.OrderStatus = orderstatus;
                if(!string.IsNullOrEmpty(PaymentStatus))
                {
                    orderHeaderFromDb.PaymentStatus = PaymentStatus;
                }
            }
        }

        public void updateStripePaymentID(int id, string sessionId, string PaymentIntentId)
        {
            var orderHeaderFromDb = _db.OrederHeader.FirstOrDefault(u => u.Id == id);
            if(!string.IsNullOrEmpty(sessionId))
            {
                orderHeaderFromDb.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(PaymentIntentId))
            {
                orderHeaderFromDb.PaymentIntentId = PaymentIntentId;
            }

        }
    }
}
