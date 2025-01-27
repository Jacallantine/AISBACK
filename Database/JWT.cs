// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;

// namespace JWT
// {
//     public class JwtHelper
// {
//     private const string SecretKey = "YourSuperSecretKeyHere123!"; 
//     private const int TokenExpiryInMinutes = 60; 

//     public string GenerateToken(string userId, string email, string firstName)
//     {
        
//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
//         var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

     
//         var claims = new[]
//         {
//             new Claim(JwtRegisteredClaimNames.Sub, userId),
//             new Claim(JwtRegisteredClaimNames.Email, email),
//             new Claim("firstname", FirstName), 
//             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
//         };

       
//         var token = new JwtSecurityToken(
//             issuer: "YourAppName",                
//             audience: "YourAppAudience",          
//             claims: claims,
//             expires: DateTime.UtcNow.AddMinutes(TokenExpiryInMinutes),
//             signingCredentials: credentials
//         );

      
//         return new JwtSecurityTokenHandler().WriteToken(token);
//     }
// }

// }