using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Lookif.UI.Common.Models;
using System;
using Lookif.UI.Component.TreeShape.Models;
using Lookif.Library.Common.Enums;
using Blazored.Toast.Services;
using static Newtonsoft.Json.JsonConvert;
using System.Linq;
using Pluralize.NET.Core;
using Lookif.Library.Common.CommonModels;
using System.Text;

namespace Lookif.UI.Report.Main.Extras
{
    public partial class Columns
    {
        #region  ...DI...

        [Inject] HttpClient Http { get; set; }
        [Inject] IToastService toastService { get; set; }

        #endregion

        #region ... Overrides ...

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            // await GetRelatedEntities();

            //CreateFakeData();
            await base.OnAfterRenderAsync(firstRender);
        }


        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            listOfEntites = await GetRelatedEntities();


        }

        #endregion


        #region ... Functions ...

        private async Task<List<BriefPropertyInfo>> GetRelatedEntities(string Entity = "")
        { 
            var res = await Http.GetAsync($"Reflection/GetRelatedEntities/{Entity}");  

            var data = DeserializeObject<ApiResult<List<BriefPropertyInfo>>>(await res.Content.ReadAsStringAsync());
 
            return data.Data;
        }

        private void CreateFakeData()
        {
            //TreeNode EntityL31 = new TreeNode();
            //EntityL31.TableName = "Layer 3";
            //EntityL31.Columns = new List<string>() { "CA", "CB" };
            //EntityL31.TreeNodes = new List<TreeNode>();

            //TreeNode EntityL32 = new TreeNode();
            //EntityL32.TableName = "Layer 3";
            //EntityL32.Columns = new List<string>() { "C2A", "C2B", "C2C" };
            //EntityL32.TreeNodes = new List<TreeNode>();



            //TreeNode EntityL21 = new TreeNode();
            //EntityL21.TableName = "Layer 2";
            //EntityL21.Columns = new List<string>() { "BA", "BB", "BC" };
            //EntityL21.TreeNodes = new List<TreeNode>();
            //EntityL21.TreeNodes.Add(EntityL31);

            //TreeNode EntityL22 = new TreeNode();
            //EntityL22.TableName = "Layer 2";
            //EntityL22.Columns = new List<string>() { "B2A", "B2B", "B2C" };
            //EntityL22.TreeNodes = new List<TreeNode>();
            //EntityL22.TreeNodes.Add(EntityL32);


            //Entity = new TreeNode();
            //Entity.TableName = "Base";
            //Entity.Columns = new List<string>() { "AA", "AB" };
            //Entity.TreeNodes = new List<TreeNode>();

            //Entity.TreeNodes.Add(EntityL21);
            //Entity.TreeNodes.Add(EntityL22);
        }
        #endregion


        #region ... Events ...
        private async Task Close()
        {
            HttpContent httpContent = new StringContent(SerializeObject(Entity), Encoding.UTF8, "application/json");

            var res =await Http.PostAsync("PredefinedReport/Create", httpContent);
             
        }

       
        private async Task SelectEntity(string entityName)
        {
            var res = await Http.GetAsync($"Reflection/GetAllPropertiesAndRelatedEntities/{entityName}"); //Because we have a enum to convert, we need to use GetAsync
            
            var deserializedString = DeserializeObject<ApiResult<List<BriefPropertyInfo>>>(await res.Content.ReadAsStringAsync());

            if (!res.IsSuccessStatusCode || !deserializedString.IsSuccess)
                toastService.ShowError(deserializedString.Message.ToString(), "خطا");
            
            Entity = new TreeNode();
            Entity.TableName =new (entityName, Entity.TableName.displayName) ;
            
            foreach (BriefPropertyInfo entry in deserializedString.Data)
            {


                if (entry.TypeOfProperty == TypeOfProperty.Class)
                    Entity.AddNode(entry.PropertyTypeName, entry.PropertyDisplayName);
                else
                    Entity.AddColumn(entry.PropertyName, entry.PropertyDisplayName);


            }  
        }


        #endregion

        #region ... Parameters ... 

        [Parameter]
        public TreeNode Entity { get; set; }





        [CascadingParameter]
        BlazoredModalInstance BlazoredModal { get; set; }

        #endregion


        #region ... Internal Properties ...
        private Pluralizer pluralizer = new();
        private List<BriefPropertyInfo> listOfEntites { get; set; }
        protected List<TreeNode> Selected { get; set; } = new List<TreeNode>();

        #endregion
    }
}
