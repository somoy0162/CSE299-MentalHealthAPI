using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.Helpers
{
    public static class Extensions
    {
        private static T GetTokenValue<T>(this HttpRequest context, string claimType)
        {
            try
            {
                var value = (new JwtSecurityTokenHandler()
                    .ReadToken(
                            context.HttpContext.Request
                            .GetTokenFromRequest()
                        ) as JwtSecurityToken
                    )
                    .Claims
                    .First(claim => claim.Type == claimType).Value;
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return default;
            }
        }
        public static string GetTokenFromRequest(this HttpRequest context)
        {
            string fullToken = context.Headers[Constants.Constants.Token];
            if (string.IsNullOrEmpty(fullToken)) fullToken = string.Empty;
            return fullToken.Replace(Constants.Constants.AuthenticationSchema, "").Trim();
        }
        public static T TokenValue<T>(this IHttpContextAccessor httpContextAccessor, string claimType)
        {
            return GetTokenValue<T>(httpContextAccessor.HttpContext.Request, claimType);
        }

        public static DataTable ListToDataTable<T>(this IEnumerable<T> list)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            IEnumerable<PropertyInfo> properties = typeof(T)
                .GetProperties(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.FlattenHierarchy
                );

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType && !property.PropertyType.IsValueType)
                {
                    dt.Columns.Add(
                        property.Name,
                        typeof(DataTable)
                    );
                }
                else
                {
                    dt.Columns.Add(
                        property.Name,
                        Nullable.GetUnderlyingType(property.PropertyType) ??
                        property.PropertyType
                    );
                }
            }

            foreach (var data in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo property in properties)
                {
                    if (property.PropertyType.IsGenericType &&
                        !property.PropertyType.IsValueType &&
                        property.GetValue(data) != null
                    )
                    {
                        row[property.Name] = (List<T>)property.GetValue(data);
                    }
                    else
                    {
                        row[property.Name] = property.GetValue(data) ?? DBNull.Value;
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
