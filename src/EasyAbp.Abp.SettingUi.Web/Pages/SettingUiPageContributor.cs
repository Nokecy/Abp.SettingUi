﻿using System.Threading.Tasks;
using EasyAbp.Abp.SettingUi.Authorization;
using EasyAbp.Abp.SettingUi.Web.Pages.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

namespace EasyAbp.Abp.SettingUi.Web.Pages
{
    public class SettingUiPageContributor : ISettingPageContributor
    {
        public async Task ConfigureAsync(SettingPageCreationContext context)
        {
            if (!await CheckPermissionsAsync(context))
            {
                return;
            }

            var settingUiAppService = context.ServiceProvider.GetRequiredService<ISettingUiAppService>();
            foreach (var grp in await settingUiAppService.GroupSettingDefinitionsAsync())
            {
                context.Groups.Add(new SettingPageGroup(grp.GroupName, grp.GroupDisplayName, typeof(SettingViewComponent), grp));
            }
        }

        public async Task<bool> CheckPermissionsAsync(SettingPageCreationContext context)
        {
            var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();
            return await authorizationService.IsGrantedAsync(SettingUiPermissions.ShowSettingPage);
        }
    }
}