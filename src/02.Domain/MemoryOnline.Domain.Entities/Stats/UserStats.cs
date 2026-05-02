using MemoryOnline.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Domain.Entities.Stats
{
    public class UserStats
    {
        public Guid IdUser { get; private set; }
        public int TotalMoves { get; private set; }
        public int TotalFails { get; private set; }
        public int TotalMatchs { get; private set; }

        public void AddMoves(int moves)
        {
            TotalMoves += moves;
        }

        public void AddFails(int fails)
        {
            TotalFails += fails;
        }

        public void AddMatchs(int matchs)
        {
            TotalMatchs += matchs;
        }

        public class Builder
        {
            private readonly UserStats _stats = new UserStats();

            public Builder()
            {
                // Valores por defecto iniciales
                _stats.TotalMoves = 0;
                _stats.TotalFails = 0;
                _stats.TotalMatchs = 0;
            }

            public Builder WithId(Guid id)
            {
                _stats.IdUser = id;
                return this;
            }

            public Builder WithTotalMoves(int moves)
            {
                _stats.TotalMoves = moves;
                return this;
            }

            public Builder WithTotalFails(int fails)
            {
                _stats.TotalFails = fails;
                return this;
            }

            public Builder WithTotalMatchs(int matchs)
            {
                _stats.TotalMatchs = matchs;
                return this;
            }


            public UserStats Build()
            {
                return _stats;
            }
        }
    }
}
