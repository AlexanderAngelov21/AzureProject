

namespace AleksandarWebDevelopment2Project.Data.Seeding
{
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;
    using Models;

    public class ExtraTypesSeeder : ISeeder
    {
        public async Task SeedAsync(CarDealershipDbContext dbContext)
        {
            if (dbContext.ExtraTypes.Any())
            {
                return;
            }

            var extraTypesToSeed = new List<ExtraType>()
            {
                new ExtraType() { Name = "Parktronic"},
                new ExtraType() { Name = "Tiptronic"},
                new ExtraType() { Name = "Heating seats"},
                new ExtraType() { Name = "Cooling seats"},
                new ExtraType() { Name = "Panorama view"},
                new ExtraType() { Name = "Launch control"},
                new ExtraType() { Name = "Systems off"},
                new ExtraType() { Name = "Air suspension"},
            };

            await dbContext.ExtraTypes.AddRangeAsync(extraTypesToSeed);
            await dbContext.SaveChangesAsync();
        }
    }
}