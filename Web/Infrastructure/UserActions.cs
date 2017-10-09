using System;
using System.Collections.Generic;
using System.Linq;
using Web.Infrastructure.Extensions;

namespace Web.Infrastructure
{
    public enum CustomClaimType
    {
        [DisplayText("Create")]
        Create,
        [DisplayText("Delete")]
        Delete,
        [DisplayText("Edit")]
        Edit,
        [DisplayText("View")]
        View,
        [DisplayText("AccountLevel")]
        AccountLevel,
        [DisplayText("File")]
        File,
        [DisplayText("License")]
        License,

    }
    public class UserActions
    {
        public static IEnumerable<string> AccessActions()
        {
            return Enum.GetValues(typeof(CustomClaimType)).Cast<CustomClaimType>().Select(c => c.ToDesc());
        }
    }
}
