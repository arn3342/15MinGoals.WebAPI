using System.Threading.Tasks;
using Users.DbAccess;
using Users.Models;

namespace Users.BusinessLayer
{
    public class ProfileRepository
    {
        AccessProfile ap;
        public ProfileRepository(string conStr)
        {
            ap = new AccessProfile(conStr);
        }
        public async Task<Profile> GetProfile(User user)
        {
            if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Password))
            {
                var AsyncProfileCallback = await ap.GetProfile(user.Email, user.Password);
                return AsyncProfileCallback.profile;
            }
            return (null);
        }

        public async Task<bool> EditProfile(User user, User newUser)
        {
            return await ap.EditProfile(user, newUser);
        }
    }
}
