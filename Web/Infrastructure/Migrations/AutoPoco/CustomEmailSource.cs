using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPoco.Engine;

namespace Web.Infrastructure.Migrations.AutoPoco
{
    public class CustomEmailSource : DatasourceBase<string>
    {
        public override string Next(IGenerationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
