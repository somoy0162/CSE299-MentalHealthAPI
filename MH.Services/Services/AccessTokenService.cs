using MH.Common.Constants;
using MH.Common.Enums;
using MH.Common.Models;
using MH.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Services
{
    public interface IAccessTokenService
    {
        Task<AccessToken> Create(AccessToken accessToken);
    }
    public class AccessTokenService : IAccessTokenService
    {
        private readonly MHDbContext _dbContext;
        public AccessTokenService(MHDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<AccessToken> Create(AccessToken objAccessToken)
        {
            UserSession userSession = new UserSession();
            string token = BuildToken(objAccessToken);
            objAccessToken.Token = token;
            UserSession existingUserSession = await _dbContext
                .UserSession.Where(x =>
                    x.Token == token &&
                    x.Status == (int)Enums.Status.Active &&
                    x.SessionEnd > DateTime.Now
                )
                .AsNoTracking()
                .OrderByDescending(x => x.UserSessionID)
                .FirstOrDefaultAsync();


            if (existingUserSession != null && existingUserSession.UserSessionID > 0)
            {
                DateTime dateTime = DateTime.Now.AddMinutes(CommonConstant.SessionExpired);
                existingUserSession.SessionEnd = dateTime;
                _dbContext.UserSession.Update(existingUserSession);

                //set return session infomaiton 
                objAccessToken.IssuedOn = existingUserSession.SessionStart;
                objAccessToken.ExpiredOn = existingUserSession.SessionEnd;
            }
            else
            {

                //remove previously active session 
                List<UserSession> lstAllPreviousSession = await _dbContext
                    .UserSession
                    .Where(x =>
                        x.SystemUserID == objAccessToken.SystemUserID &&
                        x.Status == (int)Enums.Status.Active
                    )
                    .AsNoTracking()
                    .OrderByDescending(x => x.UserSessionID)
                    .ToListAsync();
                foreach (var item in lstAllPreviousSession)
                {
                    item.Status = (int)Enums.Status.Inactive;
                }
                _dbContext.UserSession.UpdateRange(lstAllPreviousSession);

                userSession = new UserSession();
                DateTime now = DateTime.Now;
                userSession.SessionStart = now;
                userSession.SessionEnd = now.AddMinutes(CommonConstant.SessionExpired);
                userSession.Token = token;
                //userSession.RoleId = objAccessToken.RoleId;
                userSession.SystemUserID = objAccessToken.SystemUserID;
                userSession.Status = (int)Enums.Status.Active;
                await _dbContext.UserSession.AddAsync(userSession);
                //set return session infomaiton 
                objAccessToken.IssuedOn = userSession.SessionStart;
                objAccessToken.ExpiredOn = userSession.SessionEnd;

            }
            await _dbContext.SaveChangesAsync();

            return objAccessToken;
        }
        private static string BuildToken(AccessToken accessToken)
        {
            Claim[] claims = GetClaims(accessToken);
            string secretKey = "<RSAKeyValue><Modulus>xo8s7Hm6CjgRk0+lfBY7LOsErmL/i/tuscBAGWgxVaWysbE=</D></RSAKeyValue>";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("CRM", "CRM", claims, expires: accessToken.ExpiredOn, signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static Claim[] GetClaims(AccessToken accessToken)
        {
            return new[] {
                    new Claim(JwtClaim.UserId, accessToken.SystemUserID.ToString()),
                    new Claim(JwtClaim.UserType, accessToken.RoleId.ToString()),
                    new Claim(JwtClaim.ExpiresOn,accessToken.ExpiredOn.ToString()),
                    new Claim(JwtClaim.IssuedOn, accessToken.IssuedOn.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
        }
    }
}
