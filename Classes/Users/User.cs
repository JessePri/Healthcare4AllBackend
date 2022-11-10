using HealthCare4All.Data;
using HealthCare4All.Data.HTTP;
using HealthCare4All.Models;

namespace HealthCare4All.Classes.Users {
    public abstract class User {
        protected string UserName { get; set; } = "";

        public int UserId { get; protected set; } = int.MinValue;

        protected readonly Healthcare4AllDbContext healthcare4AllDbContext;

        protected User(string newUserName, Healthcare4AllDbContext newHealthcare4AllDbContext) {
            UserName = newUserName;
            healthcare4AllDbContext = newHealthcare4AllDbContext;

            var userIdListQuery = from UserInfo in healthcare4AllDbContext.UserInfos
                                  where UserInfo.UserName == UserName
                                  select UserInfo.UserId;
            int[] userIds = userIdListQuery.ToArray();

            if (userIds.Length == 1) {
                UserId = userIds[0];
            }
        }

        protected User() { }

        public ApiUserInfo GetProfile() {
            var profileQuery = from UserInfo in healthcare4AllDbContext.UserInfos
                               where UserInfo.UserId == UserId
                               select new ApiUserInfo {
                                   UserName = UserInfo.UserName,
                                   FirstName = UserInfo.FirstName,
                                   LastName = UserInfo.LastName,
                                   BirthDay = UserInfo.BirthDay
                               };

            ApiUserInfo[] userInfos = profileQuery.ToArray();

            if (userInfos.Length == 1) {
                return userInfos[0];
            } else {
                return new ApiUserInfo();
            }
        
        }

        public void EditProfile() { }
    }
}   