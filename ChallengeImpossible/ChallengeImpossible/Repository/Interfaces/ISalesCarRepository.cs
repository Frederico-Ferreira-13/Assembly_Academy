using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Repository.Interfaces
{
    public interface ISalesCarRepository
    {
        SalesCar Create(SalesCar salesCar);
        SalesCar? Read(int id);
        List<SalesCar> ReadAll();
        SalesCar Update(SalesCar salesCar);
        bool Delete(int id);
    }
}
