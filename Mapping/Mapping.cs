using AutoMapper;
using PersonalSpendingAnalysis.Repo.Entities;
using PersonalSpendingAnalysis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping
{
    public static class Mapping
    {
        static Mapping()
        {
            //todo sort out the mapping
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Transaction, TransactionDto>());
        }
    }
}
