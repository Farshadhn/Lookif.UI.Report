using Blazored.Modal;
using Blazored.Modal.Services;
using Lookif.UI.Component.TreeShape.Models;
using Lookif.UI.Report.Main.Extras;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;
namespace Lookif.UI.Report.Main.Container
{
    public partial class ReportContainer
    {
        //This is used to store current state
        //current state of all columns and related entities
        private TreeNode reportEntity;
        async Task SelectNewEntity()
        {
            
            var parameters = new ModalParameters();
            parameters.Add("Entity", reportEntity);

            var MessageForm = Modal.Show<Columns>("انتخاب پدیده جدید", parameters);
            var result = await MessageForm.Result;
            Console.WriteLine(SerializeObject(result));
        }



        [CascadingParameter]
        public IModalService Modal { get; set; }
    }
}
