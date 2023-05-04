using BookStore.DataAccess.Repository.IRepository;
using OnlineBookStoreApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            CoverType=new CovertypeRepository(_db);
            ProductType=new ProductTypeRepository(_db);
        }
        public ICategoryRepository Category { get;private set; }

        public ICoverTypeRepository CoverType { get;private set; }

        public IProductTypeRepository ProductType { get;private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
