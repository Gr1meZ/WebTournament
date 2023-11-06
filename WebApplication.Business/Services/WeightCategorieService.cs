using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Services
{
    public class WeightCategorieService : IWeightCategorieService
    {
        public Task AddWeightCategorie(WeightCategorieViewModel weightCategorieViewModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteWeightCategorie(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task EditWeightCategorie(WeightCategorieViewModel weightCategorieViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<WeightCategorieViewModel> GetWeightCategorie(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<WeightCategorieViewModel>> GetWeightCategories()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<WeightCategorieViewModel[]>> WeightCategoriesList(PagedRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
