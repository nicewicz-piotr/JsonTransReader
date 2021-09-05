using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonTransReader
{
    class Program
    {
        public async static Task Main()
        {
            Console.WriteLine("Loading json file...");

            string fileName = "script_workshop_premium_pl.json";

            using FileStream openStream = File.OpenRead(fileName);

            script_workshop_premium_pl[] script_workshop_premium_pl =
                await JsonSerializer.DeserializeAsync<script_workshop_premium_pl[]>(openStream);

            Console.WriteLine("Printing and saving to txt file values...");

            using StreamWriter file = new("TRANS.txt");

            foreach (var item in script_workshop_premium_pl)
            {
                await file.WriteLineAsync($"stepHeader: {item.stepHeader} \n" +  
                                          $"stepBody: {item.stepBody} \n" + 
                                          $"workshopNetwork: {item.workshopNetwork} \n" + 
                                          $"workshopNetworkShown: {item.workshopNetworkShown}"
                );

                        Console.WriteLine($"stepHeader: {item.stepHeader} \n" +  
                                          $"stepBody: {item.stepBody} \n" + 
                                          $"workshopNetwork: {item.workshopNetwork} \n" + 
                                          $"workshopNetworkShown: {item.workshopNetworkShown}"
                );

                foreach (var script in item.scripts)
                {
                    await file.WriteLineAsync($"scriptName: {script.scriptName}");
                   
                        Console.WriteLine($"scriptName: {script.scriptName}");

                    foreach (var nestedscript in script.scripts)
                    {
                        await file.WriteLineAsync($"scriptOrder: {nestedscript.scriptOrder} \n" +  
                                                  $"scriptHeader: {nestedscript.scriptHeader} \n" + 
                                                  $"scriptBody: {nestedscript.scriptBody}"
                );

                                Console.WriteLine($"scriptOrder: {nestedscript.scriptOrder} \n" +  
                                                  $"scriptHeader: {nestedscript.scriptHeader} \n" + 
                                                  $"scriptBody: {nestedscript.scriptBody}");

                    }
                }    
            }

            #region
            //scripts.Select(new {x.scripts.ToList().ForEach(d => d.scriptName)})
            //script_workshop_premium_pl.ToList().ForEach(x => Console.Write(x.stepHeader + "\n" + x.stepBody + "\n"));
            //script_workshop_premium_pl.ToList()
            //.ForEach(async x => await file.WriteLineAsync(x.stepHeader + "\n" + x.stepBody + "\n" + x.scripts.ToList().ForEach(v => v.scriptName) ));
            //script_workshop_premium_pl.SelectMany(m => m.scripts).SelectMany(t => t.scripts);

            //script_workshop_premium_pl.SelectMany(m => m.scripts).Select(p => p.scriptName);

            //script_workshop_premium_pl.ToList().Select(x => Console.Write(x.stepHeader + "\n" + x.stepBody + "\n"));
            //script_workshop_premium_pl.SelectMany(m => m.scripts).SelectMany(t => t.scripts);
            #endregion
        }
    }

    class script_workshop_premium_pl
    {
        public script_workshop_premium_pl() { }
        public string stepHeader { get; set; }
        public string stepBody { get; set; }
        public string workshopNetwork { get; set; }
        public string workshopNetworkShown { get; set; }
        public script[] scripts { get; set; }
    }

    class script
    {
        public script() { }
        public string scriptName { get; set; }
        public nestedscript[] scripts { get; set; }

    }

    class nestedscript
    {
        public nestedscript() { }
        public int scriptOrder { get; set; }
        public string scriptHeader { get; set; }
        public string scriptBody { get; set; }
    }
}
