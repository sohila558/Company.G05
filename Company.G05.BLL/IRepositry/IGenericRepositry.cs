﻿using Company.G05.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G05.BLL.IRepositry
{
    public interface IGenericRepositry<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T? Get(int id);
        void Add(T model);
        void Update(T model);
        void Delete(T model);
    }
}
