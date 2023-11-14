using PlatformService.Model;

namespace PlatformService.Data 
{
    public class PlatFormRepo : IPlatFormRepo
    {
        private readonly AppDbContext _context;
        public PlatFormRepo(AppDbContext context) => (_context) = (context);


        public void CreatePlatform(Platform plat)
        {
            if(plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.Platforms.Add(plat);
            SaveChanges();
        }

        public IEnumerable<Platform> GetAllFlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(s => s.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}