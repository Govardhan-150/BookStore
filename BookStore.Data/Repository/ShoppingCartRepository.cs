using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using OnlineBookStoreApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<NewShoppingCartList>, IshoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(NewShoppingCartList shoppingCart)
        {
            _db.Update(shoppingCart);
        }
        public int Decrement(NewShoppingCartList shoppingCart, int Count)
        {
            shoppingCart.count -= Count;
            return shoppingCart.count;
        }

        public int Increment(NewShoppingCartList shoppingCart, int Count)
        {
            shoppingCart.count += Count;
            return shoppingCart.count;
        }
    }
}
