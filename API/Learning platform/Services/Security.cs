
using Learning_platform.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Learning_platform.Services
{
    public class Security
    {
        private readonly JwtConfig _jwtConfig;

        public Security(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value ?? throw new ArgumentNullException(nameof(jwtConfig));
        }

        public static string DecryptPassword(string encryptedPassword, string secretKey)
        {
            // Convert the encrypted password from Base64 to bytes
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);

            // Convert the secret key to bytes
            byte[] key = Encoding.UTF8.GetBytes(secretKey);

            // Extract the IV (first 16 bytes) from the encrypted password
            byte[] iv = new byte[16];
            Buffer.BlockCopy(encryptedBytes, 0, iv, 0, 16);

            // Extract the encrypted data (remaining bytes) from the encrypted password
            byte[] encryptedData = new byte[encryptedBytes.Length - 16];
            Buffer.BlockCopy(encryptedBytes, 16, encryptedData, 0, encryptedData.Length);

            // Create an AES decryptor with the key and IV
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create a decryptor to perform the stream transform
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create streams for decryption
                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Return the decrypted password
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public string GenerateEncodedToken(string userId, string device, IList<string> roles = null)
        {
            // Initialize a list of claims for the JWT. These include the user's ID and device information,
            // a unique identifier for the JWT, and the time the token was issued.
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.Ticks.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.System, device)
            };

            DateTime expires = DateTime.Now.AddDays(1);

            // If any roles are provided, add them to the list of claims. Each role is a separate claim.
            if (roles?.Any() == true)
            {
                foreach (string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Create the JWT security token and encode it.
            // The JWT includes the claims defined above, the issuer and audience from the config
            // It's signed with a symmetric key, also from the config, and the HMAC-SHA256 algorithm.
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _jwtConfig.JwtIssuer,
                audience: _jwtConfig.JwtAudience,
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.JwtKey)),
                        SecurityAlgorithms.HmacSha256)
            );

            // Convert the JWT into a string format that can be included in an HTTP header.
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }



        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                // Create token validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.JwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtConfig.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtConfig.JwtAudience,
                    ValidateLifetime = true
                };

                // Create token handler
                var tokenHandler = new JwtSecurityTokenHandler();

                // Validate token and return claims principal
                SecurityToken validatedToken;
                return tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch (Exception ex)
            {
                // Token validation failed
                throw new SecurityTokenValidationException("Token validation failed.", ex);
            }
        }
    }




}
