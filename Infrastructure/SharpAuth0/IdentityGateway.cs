using System;
using System.Linq;
using System.Security.Claims;

namespace SharpAuth0
{
    public class IdentityGateway : IIdentityGateway
    {
        public ClaimsIdentity ClaimsIdentity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityGateway"/> class.
        /// </summary>
        public IdentityGateway()
        {
            if ((ClaimsIdentity)ClaimsPrincipal.Current.Identity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }
            ClaimsIdentity = (ClaimsIdentity)ClaimsPrincipal.Current.Identity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityGateway"/> class.
        /// </summary>
        /// <param name="claimsIdentity">The Claims Identity object.</param>
        /// <exception cref="ArgumentNullException">Access token in null or empty.</exception>
        public IdentityGateway(ClaimsIdentity claimsIdentity) : this()
        {
            if (claimsIdentity == null) {
                throw new ArgumentNullException("claimsIdentity");
            }
            ClaimsIdentity = claimsIdentity;
        }

        /// <summary>
        /// Converts the ClaimsIdentiy Claims objects to a SharpAuth0 Claim &lt;string,object&gt;
        /// </summary>
        /// <returns>The converted object</returns>
        public Claim FindIdentiyClaims()
        {
            var claim = new Claim();

            claim.UserId = ClaimsIdentity.Claims.Any(x => x.Type.Contains("sub")) ? ClaimsIdentity.Claims.First(x => x.Type.Contains("sub")).Value : null;
            claim.Name = ClaimsIdentity.Claims.Any(x => x.Type == "name") ? ClaimsIdentity.Claims.First(x => x.Type == "name").Value : null;
            claim.Email = ClaimsIdentity.Claims.Any(x => x.Type == "email") ? ClaimsIdentity.Claims.First(x => x.Type == "email").Value : null;
            return claim.UserId == null ? null : claim;
        }
    }
}
