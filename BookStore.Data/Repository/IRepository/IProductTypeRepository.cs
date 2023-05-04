using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IProductTypeRepository:IRepository<ProductType>
    {
        public void Update(ProductType product);
        public void Save();
    }
}
