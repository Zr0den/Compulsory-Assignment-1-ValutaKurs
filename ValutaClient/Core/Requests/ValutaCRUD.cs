using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValutaClient.Core.Entities;

namespace ValutaClient.Core.Requests
{
    public class ValutaCRUD
    {
        private readonly IRepository<Valuta> _repository;

        public ValutaCRUD(IRepository<Valuta> repository)
        {
            _repository = repository;
        }

        public Valuta GetValuta(int id)
        {
            return _repository.GetById(id);
        }

        public void CreateValuta(Valuta valuta)
        {
            _repository.Add(valuta);
        }

        public void ValutaChanged(Valuta valuta)
        {
            _repository.Update(valuta);
        }
    }
}
