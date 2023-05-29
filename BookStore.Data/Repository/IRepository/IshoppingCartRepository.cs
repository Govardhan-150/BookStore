using BookStore.Models;
using BookStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public  interface IshoppingCartRepository:IRepository<NewShoppingCartList>
    {
        public void Update(NewShoppingCartList shoppingCart);
        public void Save();
        int Decrement(NewShoppingCartList shoppingCart, int Count);
        int Increment(NewShoppingCartList shoppingCart, int Count);
    }
}
