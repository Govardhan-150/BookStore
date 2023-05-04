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
    public class CovertypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public CovertypeRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(CoverType obj)
        {
            _db.CoverTypes.Update(obj);
        }
    }
}
