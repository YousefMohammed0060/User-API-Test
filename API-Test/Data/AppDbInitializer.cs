using API_Test.Models;

namespace API_Test.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                if (!context.users.Any())
                {
                    context.users.AddRange(new List<Users>()
                    {
                        new Users
                        {
                            Name= "Yousef",
                            UserName="Joe0060",
                            Password="As121212"
                        }
                    });
                    context.SaveChanges();
                }

              
            }
        }
    }
}
