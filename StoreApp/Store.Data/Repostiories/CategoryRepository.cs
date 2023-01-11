using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Repostiories
{
    public class CategoryRepository :EntityRepository<Category> , ICateoryRepository
    {
        private readonly StoreDbContext _context;

        public CategoryRepository(StoreDbContext context):base(context)
        {
            this._context = context;
        }
     
    }
}
