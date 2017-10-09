//using System.Collections.Generic;
//using System.ComponentModel.Design;
//using Elmah;
//using ErrorLog = Elmah.Io.ErrorLog;
//
//[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Web.ElmahIoConfig), "Start")]
//
//namespace Web
//{
//    public class ElmahIoConfig
//    {
//        public static void Start()
//        {
//            ServiceCenter.Current = CreateServiceProviderQueryHandler(ServiceCenter.Current);
//        }
//
//        private static ServiceProviderQueryHandler CreateServiceProviderQueryHandler(ServiceProviderQueryHandler sp)
//        {
//            return context =>
//            {
//                var container = new ServiceContainer(sp(context));
//
//                var config = new Dictionary<string, string> {["LogId"] = "e9cb7f93-0ee8-4e62-a167-80e6b971836f"};
//                var log = new ErrorLog(config);
//
//                container.AddService(typeof(ErrorLog), log);
//                return container;
//            };
//        }
//    }
//}
