using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MyCensus.Models;
using Newtonsoft.Json;

namespace MyCensus.Utils
{
    static class DummyDataProvider
    {

        //Unhandled Exception:
        //Unity.Core.DependencyResolutionException: An error occurred during the activation of a particular registration.See the inner exception for details.Registration: Activator = MainPage(ReflectionActivator), Services = [MainPage(System.Object)], Lifetime = Unity.Core.Lifetime.CurrentScopeLifetime, Sharing = None, Ownership = OwnedByLifetimeScope--->An exception was thrown while invoking the constructor 'Void .ctor()' on type 'MainPage'. ---> An error occurred during the activation of a particular registration. See the inner exception for details.Registration: Activator = NetworksViewModel(ReflectionActivator), Services = [MyCensus.ViewModels.NetworksViewModel], Lifetime = Unity.Core.Lifetime.CurrentScopeLifetime, Sharing = None, Ownership = OwnedByLifetimeScope--->An exception was thrown while invoking the constructor 'Void .ctor(INavigationService)' on type 'NetworksViewModel'. ---> Value cannot be null.
        //Parameter name: stream (See inner exception for details.) (See inner exception for details.) (See inner exception for details.) (See inner exception for details.) 出现了

        /*
         usingSystem.Reflection;
// ...
// use for debugging, not in released app code!
varassembly=typeof(SharedPage).GetTypeInfo().Assembly;
foreach(varresinassembly.GetManifestResourceNames()) {
   System.Diagnostics.Debug.WriteLine("found resource: "+res);
}
             */
        public static List<CardInfo> GetTeams()
        {
            var assembly = typeof(DummyDataProvider).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("MyCensus.Assets.cards.json");
            string json = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<List<CardInfo>>(json);
        }
    }
}
