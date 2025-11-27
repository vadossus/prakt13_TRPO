using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreProject_TRPO
{
    public class BaseDbService
    {
        private BaseDbService()
        {
            try
            {
                context = new AppDbContext();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка подключения к базе данных: {ex.Message}");
            }
        }

        private static BaseDbService? instance;
        public static BaseDbService Instance
        {
            get
            {
                if (instance == null)
                    instance = new BaseDbService();
                return instance;
            }
        }

        private AppDbContext context;
        public AppDbContext Context => context;
    }
}
