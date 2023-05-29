using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get;}
        public ICoverTypeRepository CoverType { get;}
        public IProductTypeRepository ProductType { get;}
        public ICompanyRepository Company { get; }
        public IshoppingCartRepository ShoppingCart { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public  IOrderHearderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetail { get; }
        public void Save();
    }
}
