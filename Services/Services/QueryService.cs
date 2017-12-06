using IRepositories.Interfaces;
using PersonalSpendingAnalysis.IServices;
using PersonalSpendingAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PersonalSpendingAnalysis.Dtos;

namespace PersonalSpendingAnalysis.Services
{
    
    //todo rename this to QueryServiceService
    public class QueryService : IQueryService
    {
        IPersonalSpendingAnalysisRepo repo;

        public QueryService(IPersonalSpendingAnalysisRepo _repo)
        {
            repo = _repo;
        }

        public List<CategoryTotal> GetCategoryTotals(DateTime startDate, DateTime endDate, bool showDebitsOnly)
        {
            var dtos = repo.GetCategoryTotals(startDate, endDate, showDebitsOnly);
            return MapCategoryTotalDtoToCategoryTotal(dtos);
        }

        private List<CategoryTotal> MapCategoryTotalDtoToCategoryTotal(List<CategoryTotalDto> dtos)
        {
            return dtos.Select(x => new CategoryTotal
            {
                Amount = x.Amount,
                CategoryName = x.CategoryName
            }).ToList();
        }

        public List<CategoryTotal> GetCategoryTotalsForAllTime()
        {
            var list = repo.GetCategoryTotalsForAllTime();
            return MapCategoryTotalDtoToCategoryTotal(list);
        }

        public double GetNumberOfDaysOfRecordsInSystem()
        {
            var result = repo.GetNumberOfDaysOfRecordsInSystem();
            return result;
        }

        
    }
}
