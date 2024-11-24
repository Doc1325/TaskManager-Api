using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Dtos;
using TaskManager.Models;
using TaskManager.Repository;

namespace TaskManager.Services
{
    public static class InitialSetup
    {

        public static List<Status> BasicStatuses = new List<Status> {
              new Status
                {
                   StatusName = "Pendiente"

                },
              new Status
              {
                  StatusName = "En curso"

              },
              new Status
              {
                  StatusName = "Completada"

              },
          };

     


        public static void Setup(TaskContext context, IConfiguration config)
        {

             var BasicUser = new Users
             {
                 Username = config["DefaultUsername"],
                 Password = Utils.PassEncrypter.EncryptPassword(config["DefaultPassword"]),
                 RoleName = "Admin" //Los roles son case-sensitive
             };

            if (!context.Statuses.Any()) {
                foreach (var status in BasicStatuses)
                {
                    context.Statuses.Add(status);
                }
            }

            if (!context.Users.Where(u => u.RoleName == "Admin").Any())
            {
              context.Users.Add(BasicUser);
            }
             context.SaveChanges();
        }
    }
}
